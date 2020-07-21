using System.Collections.Generic;
using System.Linq;

namespace SASSADirectCapture.Services
{
    public class UserSession
    {
        public UserSession()
        {
            Roles = new List<string>();
            Office = new UserOffice();
        }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string AdName { get; set; }
        public string SamName { get; set; }
        public List<string> Roles { get; set; }
        public UserOffice Office { get; set; }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => r == role);
        }

        public string GetRole()
        {
            return Roles.First();
        }
        public bool IsIntitialized { get; set; }

    }

    public class UserOffice
    {

        public string OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string OfficeType { get; set; }
        public string RegionId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }

    }
}
