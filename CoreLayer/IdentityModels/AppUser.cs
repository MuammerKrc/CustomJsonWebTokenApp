using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CoreLayer.IdentityModels
{
    public class AppUser:IdentityUser<long>
    {
        public string City { get; set; } 
    }
}
