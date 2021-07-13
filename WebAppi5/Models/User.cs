using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppi5.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string RefreshedToken { get; set; }
        public DateTime RefreshTokenExpireDate { get; set; }
        public string Salt { get; set; }
    }
}
