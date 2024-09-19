using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Entity
{
    [Table("Certificate")]
    public class Certificate : Entity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<ProductCertificate> CertificateProduct { get; set; }
    }
}
