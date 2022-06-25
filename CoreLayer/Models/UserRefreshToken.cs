using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class UserRefreshToken:BaseModel<long>
    {
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
