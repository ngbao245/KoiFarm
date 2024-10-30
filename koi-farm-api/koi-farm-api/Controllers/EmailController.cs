using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.EmailService;
using Repository.Model;
using Repository.Model.Email;

namespace koi_farm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IServiceProvider serviceProvider)
        {
            _emailService = serviceProvider.GetRequiredService<IEmailService>();
        }
        [HttpPost]
        public IActionResult SendMail([FromBody] SendMailModel request)
        {
            try
            {
                _emailService.SendMail(request);
                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    MessageError = "Send mail success!"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = ex.Message
                });
            }
        }
    }
}
