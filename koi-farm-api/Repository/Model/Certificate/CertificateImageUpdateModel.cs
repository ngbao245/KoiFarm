using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Certificate
{
    public class CertificateImageUpdateModel
    {
        [Required]
        public string ImageUrl { get; set; }
    }
}
