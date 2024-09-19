using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string? MessageError { get; set; }
        public object? Data { get; set; }
    }
}
