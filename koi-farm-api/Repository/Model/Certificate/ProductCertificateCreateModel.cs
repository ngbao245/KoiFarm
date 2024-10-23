using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Certificate
{
    public class ProductCertificateCreateModel
    {
        public string Provider { get; set; }
        public string CertificateId { get; set; }
        public string ProductItemId { get; set; }
    }
}
