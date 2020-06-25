using System;

namespace SASSADirectCapture.BL
{
    public class DestructionProcess
    {
        public DestructionData dData { get; set; }
        private int dRegionId;
        private string dUserName;
        public DestructionProcess(int regionId, string userName)
        {
            if (string.IsNullOrEmpty(userName)) throw new System.Exception("Destruction process error : Invalid user.");
            if (regionId < 1) throw new System.Exception("Destruction process error : Invalus region.");

            dUserName = userName;
            dRegionId = regionId;
            dData = new DestructionData(dRegionId);

        }

        public void AddNewExclusion(string PensionNo, String ExclusionType)
        {
            if (string.IsNullOrEmpty(PensionNo))
            {
                //throw new System.Exception("Destruction process error : Invalid pension No.");
                return;
            }
            dData.AddExclusion(ExclusionType, PensionNo, dUserName);
        }
    }
}