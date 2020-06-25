using SASSADirectCapture.EntityModels;
using System;
using System.Collections.Generic;

namespace SASSADirectCapture.BL
{
    public static class BoxFileActions
    {
        /// <summary>
        /// Method calculates the correct File Action, determined by the SOCPEN Primary and Secondary status for the file.
        /// </summary>
        /// <param name="primaryStatusCode"></param>
        /// <param name="secondaryStatusCode"></param>
        /// <param name="socpenAppDate"></param>
        /// <param name="grantType"></param>
        /// <param name="ExistInMIS"></param>
        /// <param name="differentLocation"></param>
        /// <returns>List<string> - list of File Actions ie. Main, Legal, Transfer etc.</string></returns>
        public static List<string> CalculateAllowedFileActions(string primaryStatusCode, string secondaryStatusCode, string socpenAppDate, string socpenStatusDate, string grantType, bool ExistInMIS = true, bool differentLocation = false)
        {
            List<string> listRegistryTypes = new List<string>();

            bool isActive = false;
            DateTime? AppDate = null;
            DateTime? StatusDate = null;

            //Add default 'Please Select' item.
            listRegistryTypes.Add(SocpenFileActions.NONE);

            //If the statusses are empty, then they were not found on SOCPEN.
            if (string.IsNullOrEmpty(primaryStatusCode) && string.IsNullOrEmpty(secondaryStatusCode))
            {
                listRegistryTypes.Add(SocpenFileActions.NOTFOUND);
                return listRegistryTypes;
            }

            //CALCULATE ACTIVE STATUS
            if (secondaryStatusCode.Equals(SocpenSecondaryStatusBase.CODE_2))
            {
                if (primaryStatusCode.Equals(SocpenPrimaryStatusBase.CODE_A) ||
                    primaryStatusCode.Equals(SocpenPrimaryStatusBase.CODE_B) ||
                    primaryStatusCode.Equals(SocpenPrimaryStatusBase.CODE_9))
                {
                    isActive = true;
                }
            }

            //IF STATUS IS ACTIVE
            if (isActive)
            {
                listRegistryTypes.Add(SocpenFileActions.MAIN);

                //If AppDate is available, convert to a valid date
                if (!string.IsNullOrEmpty(socpenAppDate))
                {
                    int year = 0;
                    int mnth = 0;
                    int day = 0;

                    try
                    {
                        if (int.TryParse(socpenAppDate.Substring(0, 4), out year) &&
                        int.TryParse(socpenAppDate.Substring(4, 2), out mnth) &&
                        int.TryParse(socpenAppDate.Substring(6, 2), out day))
                        {
                            AppDate = new DateTime(year, mnth, day);
                        }
                    }
                    catch { /*invalid date*/ }
                }

                //If the record is active for more than 60 days (SOCPEN Application date)
                if (AppDate.HasValue && AppDate.Value < DateTime.Now.AddDays(-60))
                {
                    listRegistryTypes.Add(SocpenFileActions.MISSING);

                    //If the record does not exist in MIS, but only in SOCPEN
                    if (!ExistInMIS)
                    {
                        listRegistryTypes.Add(SocpenFileActions.MISPLACED);
                    }

                    listRegistryTypes.Add(SocpenFileActions.REVIEW);
                }

                // If beneficiary location on SOCPEN differs to the Registry Province and not Grant Type "S"
                if (differentLocation && !grantType.Equals("S"))
                {
                    listRegistryTypes.Add(SocpenFileActions.TRANSFER);
                }
            }
            else //IF INACTIVE
            {
                //If StatusDate is available, convert to a valid date
                if (!string.IsNullOrEmpty(socpenStatusDate))
                {
                    int year = 0;
                    int mnth = 0;
                    int day = 0;

                    try
                    {
                        if (int.TryParse(socpenStatusDate.Substring(0, 4), out year) &&
                        int.TryParse(socpenStatusDate.Substring(4, 2), out mnth) &&
                        int.TryParse(socpenStatusDate.Substring(6, 2), out day))
                        {
                            StatusDate = new DateTime(year, mnth, day);
                        }
                    }
                    catch { /*invalid date*/ }
                }

                listRegistryTypes.Add(SocpenFileActions.LEGAL);
                listRegistryTypes.Add(SocpenFileActions.FRAUD);
                listRegistryTypes.Add(SocpenFileActions.DEBTORS);

                //All Grant-Type = "S" on SOCPEN to be archived; In the case of multiple grant files all the grants on file must be inactive prior to archiving file
                if (grantType.Equals("S"))
                {
                    listRegistryTypes.Add(SocpenFileActions.ARCHIVE);
                }

                //If the record is inactive for more than 5 years (SOCPEN Application date)
                //Changed to 6 years, since we're only comparing years, not months or days.
                if (StatusDate.HasValue && StatusDate.Value < DateTime.Now.AddYears(-6))
                {
                    listRegistryTypes.Add(SocpenFileActions.DESTRUCTION); //TODO; include destruction year in results
                }
            }

            return listRegistryTypes;
        }

        public static void CalculateSocpenStatus(ref MISBoxFiles newfile)
        {
            //SET THE CORRECT GRANT TYPE, APP DATE, PRIMARY STATUS AND SECONDARY STATUS BEFORE STORING RECORD
            //WORK OUT THE CORRECT GRANT TYPE TO USE BASED ON THE RUBBISH DATA IN SOCPENID.
            int latestAppDate = 0;
            int latestStatusDate = 0;
            string latestGrantType = string.Empty;
            string latestPrimStatus = string.Empty;
            string latestSecStatus = string.Empty;

            if (!string.IsNullOrEmpty(newfile.GRANT_TYPE1))
            {
                latestGrantType = newfile.GRANT_TYPE1.Trim();
            }
            if (!string.IsNullOrEmpty(newfile.GRANT_TYPE2))
            {
                latestGrantType = newfile.GRANT_TYPE2.Trim();
            }
            if (!string.IsNullOrEmpty(newfile.GRANT_TYPE3))
            {
                latestGrantType = newfile.GRANT_TYPE3.Trim();
            }
            if (!string.IsNullOrEmpty(newfile.GRANT_TYPE4))
            {
                latestGrantType = newfile.GRANT_TYPE4.Trim();
            }

            try
            {
                //Application dates are not always filled in.
                //Check if APP_DATE4 is the latest
                if (!string.IsNullOrEmpty(newfile.APP_DATE4) && !string.IsNullOrEmpty(newfile.GRANT_TYPE4))
                {
                    int.TryParse(newfile.APP_DATE4, out latestAppDate);
                    latestGrantType = newfile.GRANT_TYPE4;
                }

                //Check if APP_DATE3 is the latest
                if (!string.IsNullOrEmpty(newfile.APP_DATE3) && !string.IsNullOrEmpty(newfile.GRANT_TYPE3))
                {
                    int appD3 = 0;
                    if (int.TryParse(newfile.APP_DATE3, out appD3) && appD3 > latestAppDate)
                    {
                        latestAppDate = int.Parse(newfile.APP_DATE3);
                        latestGrantType = newfile.GRANT_TYPE3;
                    }
                }

                //Check if APP_DATE2 is the latest
                if (!string.IsNullOrEmpty(newfile.APP_DATE2) && !string.IsNullOrEmpty(newfile.GRANT_TYPE2))
                {
                    int appD2 = 0;
                    if (int.TryParse(newfile.APP_DATE2, out appD2) && appD2 > latestAppDate)
                    {
                        latestAppDate = int.Parse(newfile.APP_DATE2);
                        latestGrantType = newfile.GRANT_TYPE2;
                    }
                }

                //Check if APP_DATE1 is the latest
                if (!string.IsNullOrEmpty(newfile.APP_DATE1) && !string.IsNullOrEmpty(newfile.GRANT_TYPE1))
                {
                    int appD1 = 0;
                    if (int.TryParse(newfile.APP_DATE1, out appD1) && appD1 > latestAppDate)
                    {
                        latestAppDate = int.Parse(newfile.APP_DATE1);
                        latestGrantType = newfile.GRANT_TYPE1;
                    }
                }
            }
            catch { }

            try
            {
                //Status dates are not always filled in.
                //Check if STATUS_DATE4 is the latest
                if (!string.IsNullOrEmpty(newfile.STATUS_DATE4) && !string.IsNullOrEmpty(newfile.PRIM_STATUS4) && !string.IsNullOrEmpty(newfile.SEC_STATUS4))
                {
                    int.TryParse(newfile.STATUS_DATE4, out latestStatusDate);
                    latestPrimStatus = newfile.PRIM_STATUS4;
                    latestSecStatus = newfile.SEC_STATUS4;
                }

                //Check if STATUS_DATE3 is the latest
                if (!string.IsNullOrEmpty(newfile.STATUS_DATE3) && !string.IsNullOrEmpty(newfile.PRIM_STATUS3) && !string.IsNullOrEmpty(newfile.SEC_STATUS3))
                {
                    int statD3 = 0;
                    if (int.TryParse(newfile.STATUS_DATE3, out statD3) && statD3 > latestStatusDate)
                    {
                        latestStatusDate = statD3;
                        latestPrimStatus = newfile.PRIM_STATUS3;
                        latestSecStatus = newfile.SEC_STATUS3;
                    }
                }

                //Check if STATUS_DATE2 is the latest
                if (!string.IsNullOrEmpty(newfile.STATUS_DATE2) && !string.IsNullOrEmpty(newfile.PRIM_STATUS2) && !string.IsNullOrEmpty(newfile.SEC_STATUS2))
                {
                    int statD2 = 0;
                    if (int.TryParse(newfile.STATUS_DATE2, out statD2) && statD2 > latestStatusDate)
                    {
                        latestStatusDate = statD2;
                        latestPrimStatus = newfile.PRIM_STATUS2;
                        latestSecStatus = newfile.SEC_STATUS2;
                    }
                }

                //Check if STATUS_DATE1 is the latest
                if (!string.IsNullOrEmpty(newfile.STATUS_DATE1) && !string.IsNullOrEmpty(newfile.PRIM_STATUS1) && !string.IsNullOrEmpty(newfile.SEC_STATUS1))
                {
                    int statD1 = 0;
                    if (int.TryParse(newfile.STATUS_DATE1, out statD1) && statD1 > latestStatusDate)
                    {
                        latestStatusDate = statD1;
                        latestPrimStatus = newfile.PRIM_STATUS1;
                        latestSecStatus = newfile.SEC_STATUS1;
                    }
                }
            }
            catch { }

            newfile.SOCPEN_APP_DATE = latestAppDate > 0 ? latestAppDate.ToString() : string.Empty;
            newfile.SOCPEN_STATUS_DATE = latestStatusDate > 0 ? latestStatusDate.ToString() : string.Empty;
            newfile.SOCPEN_GRANT_TYPE = latestGrantType;
            newfile.SOCPEN_PRIM_STATUS = latestPrimStatus;
            newfile.SOCPEN_SEC_STATUS = latestSecStatus;
        }

        /// <summary>
        /// Helper function to convert the MIS Grant Type to the SOCPEN Grant Type due to discrepancies in a few of the codes.
        /// </summary>
        /// <param name="misGrantType"></param>
        /// <returns></returns>
        ///

        public static void GetSocpenAccountStatus(ref MISBoxFiles newfile)
        {
            //  IF ANY OF THE GRANTS FOR THIS PERSON IS STILL ACTIVE ALL ARE REGISTRY TYPE MAIN
            int latestAppDate = 0;
            int latestStatusDate = 0;
            string latestGrantType = string.Empty;
            string latestPrimStatus = string.Empty;
            string latestSecStatus = string.Empty;

            if (!string.IsNullOrEmpty(newfile.GRANT_TYPE1))
            {
                latestGrantType = newfile.GRANT_TYPE1.Trim();
            }
            if (!string.IsNullOrEmpty(newfile.GRANT_TYPE2))
            {
                latestGrantType = newfile.GRANT_TYPE2.Trim();
            }
            if (!string.IsNullOrEmpty(newfile.GRANT_TYPE3))
            {
                latestGrantType = newfile.GRANT_TYPE3.Trim();
            }
            if (!string.IsNullOrEmpty(newfile.GRANT_TYPE4))
            {
                latestGrantType = newfile.GRANT_TYPE4.Trim();
            }

            try
            {
                //Application dates are not always filled in.
                //Check if APP_DATE4 is the latest
                if (!string.IsNullOrEmpty(newfile.APP_DATE4) && !string.IsNullOrEmpty(newfile.GRANT_TYPE4))
                {
                    int.TryParse(newfile.APP_DATE4, out latestAppDate);
                    latestGrantType = newfile.GRANT_TYPE4;
                }

                //Check if APP_DATE3 is the latest
                if (!string.IsNullOrEmpty(newfile.APP_DATE3) && !string.IsNullOrEmpty(newfile.GRANT_TYPE3))
                {
                    int appD3 = 0;
                    if (int.TryParse(newfile.APP_DATE3, out appD3) && appD3 > latestAppDate)
                    {
                        latestAppDate = int.Parse(newfile.APP_DATE3);
                        latestGrantType = newfile.GRANT_TYPE3;
                    }
                }

                //Check if APP_DATE2 is the latest
                if (!string.IsNullOrEmpty(newfile.APP_DATE2) && !string.IsNullOrEmpty(newfile.GRANT_TYPE2))
                {
                    int appD2 = 0;
                    if (int.TryParse(newfile.APP_DATE2, out appD2) && appD2 > latestAppDate)
                    {
                        latestAppDate = int.Parse(newfile.APP_DATE2);
                        latestGrantType = newfile.GRANT_TYPE2;
                    }
                }

                //Check if APP_DATE1 is the latest
                if (!string.IsNullOrEmpty(newfile.APP_DATE1) && !string.IsNullOrEmpty(newfile.GRANT_TYPE1))
                {
                    int appD1 = 0;
                    if (int.TryParse(newfile.APP_DATE1, out appD1) && appD1 > latestAppDate)
                    {
                        latestAppDate = int.Parse(newfile.APP_DATE1);
                        latestGrantType = newfile.GRANT_TYPE1;
                    }
                }
            }
            catch { }

            try
            {
                //Status dates are not always filled in.
                //Check if STATUS_DATE4 is the latest
                if (!string.IsNullOrEmpty(newfile.STATUS_DATE4) && !string.IsNullOrEmpty(newfile.PRIM_STATUS4) && !string.IsNullOrEmpty(newfile.SEC_STATUS4))
                {
                    int.TryParse(newfile.STATUS_DATE4, out latestStatusDate);
                    latestPrimStatus = newfile.PRIM_STATUS4;
                    latestSecStatus = newfile.SEC_STATUS4;
                }

                //Check if STATUS_DATE3 is the latest
                if (!string.IsNullOrEmpty(newfile.STATUS_DATE3) && !string.IsNullOrEmpty(newfile.PRIM_STATUS3) && !string.IsNullOrEmpty(newfile.SEC_STATUS3))
                {
                    int statD3 = 0;
                    if (int.TryParse(newfile.STATUS_DATE3, out statD3) && statD3 > latestStatusDate)
                    {
                        latestStatusDate = statD3;
                        latestPrimStatus = newfile.PRIM_STATUS3;
                        latestSecStatus = newfile.SEC_STATUS3;
                    }
                }

                //Check if STATUS_DATE2 is the latest
                if (!string.IsNullOrEmpty(newfile.STATUS_DATE2) && !string.IsNullOrEmpty(newfile.PRIM_STATUS2) && !string.IsNullOrEmpty(newfile.SEC_STATUS2))
                {
                    int statD2 = 0;
                    if (int.TryParse(newfile.STATUS_DATE2, out statD2) && statD2 > latestStatusDate)
                    {
                        latestStatusDate = statD2;
                        latestPrimStatus = newfile.PRIM_STATUS2;
                        latestSecStatus = newfile.SEC_STATUS2;
                    }
                }

                //Check if STATUS_DATE1 is the latest
                if (!string.IsNullOrEmpty(newfile.STATUS_DATE1) && !string.IsNullOrEmpty(newfile.PRIM_STATUS1) && !string.IsNullOrEmpty(newfile.SEC_STATUS1))
                {
                    int statD1 = 0;
                    if (int.TryParse(newfile.STATUS_DATE1, out statD1) && statD1 > latestStatusDate)
                    {
                        latestStatusDate = statD1;
                        latestPrimStatus = newfile.PRIM_STATUS1;
                        latestSecStatus = newfile.SEC_STATUS1;
                    }
                }
            }
            catch { }

            newfile.SOCPEN_APP_DATE = latestAppDate > 0 ? latestAppDate.ToString() : string.Empty;
            newfile.SOCPEN_STATUS_DATE = latestStatusDate > 0 ? latestStatusDate.ToString() : string.Empty;
            newfile.SOCPEN_GRANT_TYPE = latestGrantType;
            newfile.SOCPEN_PRIM_STATUS = latestPrimStatus;
            newfile.SOCPEN_SEC_STATUS = latestSecStatus;
        }

        public static string convertMISGrantTypeToSocpen(string misGrantType)
        {
            if (string.IsNullOrEmpty(misGrantType))
            {
                return string.Empty;
            }

            switch (misGrantType)
            {
                case "1": return "0";
                case "2": return "1";
                case "10": return "C";
                case "11": return "S";
                case "GIA": return "8";
                case "O": return "0";
                default: return misGrantType;
            }
        }
    }
}