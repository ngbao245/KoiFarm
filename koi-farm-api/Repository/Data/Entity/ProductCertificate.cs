using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("ProductCertificate")]
    public class ProductCertificate : Entity
    {
        public string Provider { get; set; } = "Alan Walker";
        public string CertificateId { get; set; }
        public string ProductItemId { get; set; }

        [ForeignKey(nameof(ProductItemId))]
        public ProductItem productItem { get; set; }

        [ForeignKey(nameof(CertificateId))]
        public Certificate certificate { get; set; }
        
    }
}
