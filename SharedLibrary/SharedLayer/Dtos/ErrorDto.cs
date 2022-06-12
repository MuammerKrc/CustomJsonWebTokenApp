using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer.Dtos
{
    public class ErrorDto
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsShow { get; set; }

        public ErrorDto(List<string> errors, bool isShow = true)
        {
            Errors = errors;
            IsShow = isShow;
        }

        public ErrorDto(string error,bool isShow=true)
        {
            Errors.Add(error);
            IsShow = isShow;
        }
    }
}
