using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model.Certificate;
using Repository.Model;
using Repository.Repository;


namespace koi_farm_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CertificateController : Controller
    {
        private UnitOfWork _unitOfWork;

        public CertificateController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private string GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst("UserID");
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in claims.");
            }
            return userIdClaim.Value;
        }

        [HttpPost("create-certificate")]

        public IActionResult CreateCertficate(CertificateCreateModel certificateModel)
        {
            if (certificateModel == null || string.IsNullOrEmpty(certificateModel.Name))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid certificate data."
                });
            }

            var existingCertificate= _unitOfWork.CertificateRepository.GetSingle(p => p.Name == certificateModel.Name);
            if (existingCertificate != null)
            {
                return Conflict(new ResponseModel
                {
                    StatusCode = 409,
                    MessageError = "Name already exists."
                });
            }

            var certificate = new Certificate
            {
                Name = certificateModel.Name,
                ImageUrl = certificateModel.ImageUrl,
                 
            };

            var certificatedata = _unitOfWork.CertificateRepository.Create(certificate);

            _unitOfWork.CertificateRepository.Create(certificatedata);

            return Ok(new ResponseModel
            {
                StatusCode = 201,
                Data = certificatedata
            });
        }
    }
}
