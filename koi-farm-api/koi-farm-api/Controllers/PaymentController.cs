using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.Data.Entity;
using Repository.EmailService;
using Repository.Model;
using Repository.Model.Email;
using Repository.Model.Order;
using Repository.Model.Payment;
using Repository.Model.Product;
using Repository.PaymentService;
using Repository.Repository;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace koi_farm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public PaymentController(IVnPayService vnPayService, UnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _vnPayService = vnPayService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = serviceProvider.GetRequiredService<IEmailService>();
        }

        private string GetUserIdFromClaims()
        {
            return User.FindFirst("UserID")?.Value;
        }


        // POST method to create a payment request and generate VNPAY payment URL
        [HttpPost("create-payment-url")]
        public IActionResult CreatePaymentUrl([FromBody] PaymentRequestModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.OrderId))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid payment request model or missing OrderId."
                });
            }

            var order = _unitOfWork.OrderRepository.GetById(model.OrderId);
            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            var url = _vnPayService.CreatePaymentUrl(model, HttpContext, order.Total, order.Id);

            if (string.IsNullOrEmpty(url))
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    MessageError = "Failed to create payment URL."
                });
            }

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = url
            });
        }

        [AllowAnonymous]
        // GET method to handle the payment callback from VNPAY
        [HttpGet("payment-callback")]
        public IActionResult PaymentCallback()
        {
            var orderID = Request.Query["vnp_TxnRef"].ToString();
            var response = _vnPayService.PaymentExecute(Request.Query, orderID);

            if (response == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Payment response not found."
                });
            }

            var orderId = response.OrderId;
            var order = _unitOfWork.OrderRepository.GetById(orderId);

            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            var user = _unitOfWork.UserRepository.GetById(order.UserId);
            if (user == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "User not found."
                });
            }

            if (response.Success)
            {
                if (!string.IsNullOrEmpty(order.ConsignmentId))
                {
                    order.Status = "Completed";
                }
                else
                {
                    order.Status = "Pending";
                }

                var payment = new Payment
                {
                    Id = response.PaymentId,
                    Amount = order.Total,
                    Status = (!string.IsNullOrEmpty(order.ConsignmentId)) ? "SuccessConsignment" : "Success",
                    Method = response.PaymentMethod,
                    OrderId = order.Id
                };

                _unitOfWork.PaymentRepository.Create(payment);
                _unitOfWork.OrderRepository.Update(order);

                //Send confirmation mail
                var emailRequest = new SendMailModel
                {
                    ReceiveAddress = user.Email,
                    Title = "Xác Nhận Thanh Toán",
                    Content = $@"
                    <p>Kính chào {user.Name},</p>
                    <p>Cảm ơn quý khách đã mua hàng. Thanh toán cho Đơn hàng <strong>ID: {orderId}</strong> của quý khách đã thành công.</p>
                    <p>Số giao dịch của quý khách: <strong>{payment.Id}</strong>.</p>
                    <p>Số tiền: <strong>{order.Total.ToString("C0", new CultureInfo("vi-VN"))}</strong>.</p>
                    <br>
                    <p>Trân trọng,</p>
                    <p>KoiShop</p>"
                };

                _emailService.SendMail(emailRequest);
            }
            else
            {
                order.Status = "Failed";
                _unitOfWork.OrderRepository.Update(order);

                //Send confirmation mail
                var emailRequest = new SendMailModel
                {
                    ReceiveAddress = user.Email,
                    Title = "Thanh Toán Thất Bại",
                    Content = $@"
                    <p>Kính chào {user.Name},</p>
                    <p>Thanh toán cho Đơn hàng <strong>ID: {orderId}</strong> của quý khách đã thất bại.</p>
                    <p>Quý khách có thể mua lại hoặc hủy đơn hàng này.</p>
                    <br>
                    <p>Trân trọng,</p>
                    <p>KoiShop</p>"
                };

                _emailService.SendMail(emailRequest);
            }

            var paymentUrl = _configuration["FrontEndPort:PaymentUrl"];

            if (response.Success)
            {
                return Redirect($"{paymentUrl}/payment-success?orderId={orderId}&paymentId={response.PaymentId}&status=success");
            }
            else
            {
                return Redirect($"{paymentUrl}/payment-failed?orderId={orderId}&status=failed");
            }
        }

        [HttpPost("create-payment")]
        public IActionResult CreatePayment(RequestPaymentModel response)
        {
            var orderId = response.OrderId;
            var order = _unitOfWork.OrderRepository.GetById(orderId);

            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            if (!string.IsNullOrEmpty(order.ConsignmentId))
            {
                order.Status = "Completed";
            }

            var payment = new Payment
            {
                Amount = order.Total,
                Status = (!string.IsNullOrEmpty(order.ConsignmentId)) ? "SuccessConsignmentCOD" : "SuccessCOD",
                Method = "Cash on Delivery",
                OrderId = order.Id
            };
            _unitOfWork.PaymentRepository.Create(payment);
            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = payment
            });

        }

        [HttpGet("get-all-payments")]
        public IActionResult GetAllPayments()
        {
            var payments = _unitOfWork.PaymentRepository.Get(includeProperties: p => p.Order).ToList();

            if (!payments.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No payments found."
                });
            }

            var responsePayments = _mapper.Map<List<ResponsePaymentModel>>(payments);


            foreach (var responsePayment in responsePayments)
            {
                var order = _unitOfWork.OrderRepository.GetSingle(o => o.Id == responsePayment.OrderId, o => o.Items);

                responsePayment.Order.OrderId = order.Id;
                responsePayment.Order.Address = order.Address;
                responsePayment.Order.StaffId = order.StaffId;

                responsePayment.Order.Items = order.Items.Select(item => new OrderItemResponseModel
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price
                }).ToList();

            }


            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responsePayments
            });
        }

        [HttpGet("get-user-payments")]
        public IActionResult GetUserPayments()
        {
            var userId = User.FindFirst("UserID")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ResponseModel
                {
                    StatusCode = 401,
                    MessageError = "Unauthorized. User ID not found in claims."
                });
            }

            var payments = _unitOfWork.PaymentRepository.Get(includeProperties: p => p.Order).Where(p => p.Order.UserId == userId).ToList();

            if (!payments.Any())
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "No payments found."
                });
            }


            var responsePayments = _mapper.Map<List<ResponsePaymentModel>>(payments);


            foreach (var responsePayment in responsePayments)
            {
                var order = _unitOfWork.OrderRepository.GetSingle(o => o.Id == responsePayment.OrderId, o => o.Items);

                responsePayment.Order.OrderId = order.Id;
                responsePayment.Order.Address = order.Address;
                responsePayment.Order.StaffId = order.StaffId;

                responsePayment.Order.Items = order.Items.Select(item => new OrderItemResponseModel
                {
                    ProductItemId = item.ProductItemId,
                    Quantity = item.Quantity,
                    Price = _unitOfWork.ProductItemRepository.GetById(item.ProductItemId).Price
                }).ToList();

            }


            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responsePayments
            });
        }



        [HttpPost("process-refund")]
        public IActionResult ProcessRefund([FromBody] RefundRequestModel refundRequest)
        {
            var payment = _unitOfWork.PaymentRepository.GetById(refundRequest.TransactionNo);
            if (payment == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Payment not found."
                });
            }

            if (refundRequest == null || string.IsNullOrEmpty(payment.OrderId))
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "Invalid refund request model or missing OrderId."
                });
            }

            var order = _unitOfWork.OrderRepository.GetById(payment.OrderId);
            if (order == null)
            {
                return NotFound(new ResponseModel
                {
                    StatusCode = 404,
                    MessageError = "Order not found."
                });
            }

            if (order.Status != "Cancelled")
            {
                return BadRequest(new ResponseModel
                {
                    StatusCode = 400,
                    MessageError = "This order has not been cancelled"
                });
            }

            var userId = order.UserId;
            var user = _unitOfWork.UserRepository.GetById(userId);

            try
            {
                var response = _vnPayService.ProcessRefund(refundRequest, HttpContext, payment.CreatedTime, payment.Amount, user.Name, payment.OrderId);

                if (response.ResponseCode == "00")
                {
                    payment.Status = "Refunded";
                    _unitOfWork.PaymentRepository.Update(payment);
                }

                return Ok(new ResponseModel
                {
                    StatusCode = 200,
                    Data = response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseModel
                {
                    StatusCode = 500,
                    MessageError = "Failed to process refund: " + ex.InnerException
                });
            }
        }
    }
}
