using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.Certificate;
using Repository.Repository;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/[controller]")]
public class CertificateController : ControllerBase
{
    private readonly UnitOfWork _unitOfWork;

    public CertificateController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("all-certificate")]
    public IActionResult GetAllCertificate()
    {
        var certificates = _unitOfWork.CertificateRepository.Get(
            c => !c.IsDeleted,
            includeProperties: c => c.CertificateProduct
        ).ToList();

        if (!certificates.Any())
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "No Certificate was found"
            });
        }

        var response = certificates.Select(certificate => new
        {
            CertificateId = certificate.Id,
            CertificateName = certificate.Name,
            ImageUrl = certificate.ImageUrl,
            ProductCertificates = certificate.CertificateProduct.Select(item => new
            {
                ProductItemId = item.ProductItemId ,
                Id = item.Id,
                Provider = item.Provider,
                PublishDate = item.CreatedTime
            }).ToList()
        }).ToList();

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = response
        });
    }

    [HttpGet("certificate/{id}")]
    public IActionResult GetCertificateById(string id)
    {
        var certificate = _unitOfWork.CertificateRepository.Get(
            c => c.Id == id && !c.IsDeleted,
            includeProperties: c => c.CertificateProduct
        ).FirstOrDefault();

        if (certificate == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Certificate not found"
            });
        }

        var response = new
        {
            CertificateId = certificate.Id,
            CertificateName = certificate.Name,
            ImageUrl = certificate.ImageUrl,
            ProductCertificates = certificate.CertificateProduct.Select(item => new
            {
                ProductItemId = item.ProductItemId,
                Id = item.Id,
                Provider = item.Provider,
                PublishDate = item.CreatedTime
            }).ToList()
        };

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = response
        });
    }

    [HttpPost("create-certificate")]
    public IActionResult CreateCertificate([FromBody] CertificateCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Invalid input data"
            });
        }

        var certificate = new Certificate
        {
            Name = model.Name,
            ImageUrl = model.ImageUrl
        };

        _unitOfWork.CertificateRepository.Create(certificate);

        var response = new
        {
            CertificateId = certificate.Id,
            CertificateName = certificate.Name,
            ImageUrl = certificate.ImageUrl,
            CreatedTime = certificate.CreatedTime
        };

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = response
        });
    }

    [HttpPost("add-product-certificate")]
    public IActionResult AddProductCertificate([FromBody] ProductCertificateCreateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Invalid input data"
            });
        }

        var certificate = _unitOfWork.CertificateRepository.Get(
            c => c.Id == model.CertificateId && !c.IsDeleted
        ).FirstOrDefault();

        if (certificate == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Certificate not found"
            });
        }

        var productItem = _unitOfWork.ProductItemRepository.Get(
            p => p.Id == model.ProductItemId && !p.IsDeleted
        ).FirstOrDefault();

        if (productItem == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Product Item not found"
            });
        }

        var existingCertificate = _unitOfWork.ProductCertificateRepository.Get(
            pc => pc.CertificateId == model.CertificateId &&
                pc.ProductItemId == model.ProductItemId &&
                  !pc.IsDeleted
        ).FirstOrDefault();

        if (existingCertificate != null)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "This product already has this certificate"
            });
        }

        var productCertificate = new ProductCertificate
        {
            CertificateId = model.CertificateId,
            ProductItemId = model.ProductItemId,
            Provider = model.Provider
        };

        _unitOfWork.ProductCertificateRepository.Create(productCertificate);

        var response = new
        {
            ProductCertificateId = productCertificate.Id,
            CertificateId = productCertificate.CertificateId,
            ProductItemId = productCertificate.ProductItemId,
            Provider = productCertificate.Provider,
            CreatedTime = productCertificate.CreatedTime
        };

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = response
        });
    }

    [HttpPut("update-product-certificate/{id}")]
    public IActionResult UpdateProductCertificate(string id, [FromBody] ProductCertificateUpdateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Invalid input data"
            });
        }

        var productCertificate = _unitOfWork.ProductCertificateRepository.Get(
            pc => pc.Id == id && !pc.IsDeleted,
            includeProperties: pc => pc.certificate
        ).FirstOrDefault();

        if (productCertificate == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Product Certificate not found"
            });
        }

        productCertificate.Provider = model.Provider;
        productCertificate.LastUpdatedTime = DateTimeOffset.Now;

        _unitOfWork.ProductCertificateRepository.Update(productCertificate);

        var response = new
        {
            ProductCertificateId = productCertificate.Id,
            CertificateId = productCertificate.CertificateId,
            ProductItemId = productCertificate.ProductItemId,
            Provider = productCertificate.Provider,
            LastUpdatedTime = productCertificate.LastUpdatedTime
        };

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = response
        });
    }

    [HttpDelete("remove-product-certificate/{id}")]
    public IActionResult RemoveProductCertificate(string id)
    {
        var productCertificate = _unitOfWork.ProductCertificateRepository.Get(
            pc => pc.Id == id && !pc.IsDeleted,
            includeProperties: pc => pc.certificate
        ).FirstOrDefault();

        if (productCertificate == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Product Certificate not found"
            });
        }

        productCertificate.IsDeleted = true;
        productCertificate.DeletedTime = DateTimeOffset.Now;

        _unitOfWork.ProductCertificateRepository.Update(productCertificate);


        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = new { Message = "Product Certificate removed successfully" }
        });
    }

    [HttpPut("update-certificate-image/{id}")]
    public IActionResult UpdateCertificateImage(string id, [FromBody] CertificateImageUpdateModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResponseModel
            {
                StatusCode = 400,
                MessageError = "Invalid input data"
            });
        }

        var certificate = _unitOfWork.CertificateRepository.Get(
            c => c.Id == id && !c.IsDeleted
        ).FirstOrDefault();

        if (certificate == null)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Certificate not found"
            });
        }

        certificate.ImageUrl = model.ImageUrl;
        certificate.LastUpdatedTime = DateTimeOffset.Now;

        _unitOfWork.CertificateRepository.Update(certificate);

        var response = new
        {
            CertificateId = certificate.Id,
            CertificateName = certificate.Name,
            ImageUrl = certificate.ImageUrl,
            LastUpdatedTime = certificate.LastUpdatedTime
        };

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = response
        });
    }

    [HttpPut("update-certificate/{id}")]
    public IActionResult UpdateCertificate(string id, [FromBody] CertificateUpdateModel model)
    {
        var certificate = _unitOfWork.CertificateRepository.GetById(id);

        if (certificate == null || certificate.IsDeleted)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Certificate not found"
            });
        }

        certificate.Name = model.Name;
        certificate.ImageUrl = model.ImageUrl;
        certificate.LastUpdatedTime = DateTimeOffset.Now;

        _unitOfWork.CertificateRepository.Update(certificate);

        var response = new
        {
            CertificateId = certificate.Id,
            CertificateName = certificate.Name,
            ImageUrl = certificate.ImageUrl,
            LastUpdatedTime = certificate.LastUpdatedTime
        };

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = response
        });
    }

    [HttpDelete("delete-certificate/{id}")]
    public IActionResult DeleteCertificate(string id)
    {
        var certificate = _unitOfWork.CertificateRepository.GetById(id);

        if (certificate == null || certificate.IsDeleted)
        {
            return NotFound(new ResponseModel
            {
                StatusCode = 404,
                MessageError = "Certificate not found"
            });
        }

        _unitOfWork.CertificateRepository.Delete(certificate);

        return Ok(new ResponseModel
        {
            StatusCode = 200,
            Data = new { Message = "Certificate deleted successfully" }
        });
    }
}
