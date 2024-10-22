using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Email
{
    public class SendMailModel
    {
        public string Title { get; set; }
        public string ReceiveAddress { get; set; }
        public string Content { get; set; }
    }
}
