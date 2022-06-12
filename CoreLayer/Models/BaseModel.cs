using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Models
{
    public class BaseModel<T>
    {
        public T Id { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? DeletedTime { get; set; }
        public string? CreaterUserId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
