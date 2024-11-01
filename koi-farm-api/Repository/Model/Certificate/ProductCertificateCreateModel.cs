using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Certificate
{
    public class ProductCertificateCreateModel
    {
        [Required]
        public string ProductItemId { get; set; }
        [Required]
        public string CertificateId { get; set; }
        public string Provider { get; set; } = "Alan Walker";


    }
}
