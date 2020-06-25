using System;
using System.Linq.Expressions;

namespace SASSADirectCapture.EntityModels
{

    public partial class DC_EXCLUSIONS
    {
        public static DC_EXCLUSIONS FromCsv(string csvLine, string etype, int eregion, string euser)
        {
            //string[] values = csvLine.Split(',');
            DC_EXCLUSIONS exclusion = new SASSADirectCapture.EntityModels.DC_EXCLUSIONS
            {
                EXCLUSION_TYPE = etype,
                EXCL_DATE = DateTime.Now,
                ID_NO = csvLine,
                REGION_ID = eregion,
                USERNAME = euser
            };
            return exclusion;
        }

    }
    public partial class DC_DESTRUCTION
    {
        public static Expression<Func<DC_DESTRUCTION, bool>> IsBefore(int year)
        {
            return p => int.Parse(p.DESTRUCTIO_DATE) < year;
        }
    }
}
