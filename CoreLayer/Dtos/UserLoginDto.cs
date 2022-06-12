using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Dtos
{
    public class UserLoginDto
    {
        public string EmailOrUserName { get; set; }
        public string Password { get; set; }
        
    }
}
