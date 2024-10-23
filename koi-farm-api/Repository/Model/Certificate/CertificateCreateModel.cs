using Repository.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Certificate
{
    public class CertificateCreateModel
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
