using System.Collections.Generic;

namespace Polimerida_CAP.Models
{
    public class EmployeeDelete
    {
        public string EmployeeNo { get; set; }
    }

    public class UserInfoDelCond
    {
        public List<EmployeeDelete> EmployeeNoList { get; set; }
    }

    public class RequestBodyDelete
    {
        public UserInfoDelCond UserInfoDelCond { get; set; }
    }
} 