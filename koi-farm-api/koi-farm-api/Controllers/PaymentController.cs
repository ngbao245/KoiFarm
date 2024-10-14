using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Data.Entity;
using Repository.Model;
using Repository.Model.Order;
using Repository.Model.Payment;
using Repository.Model.Product;
using Repository.PaymentService;
using Repository.Repository;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace koi_farm_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentController(IVnPayService vnPayService, UnitOfWork unitOfWork, IMapper mapper)
        {
            _vnPayService = vnPayService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

            if (response.Success)
            {

                //order.Status = "Paid";

                var payment = new Payment
                {
                    Id = response.PaymentId,
                    Amount = order.Total,
                    Status = "Success",
                    Method = response.PaymentMethod,
                    OrderId = order.Id
                };

                _unitOfWork.PaymentRepository.Create(payment);
                _unitOfWork.OrderRepository.Update(order);
            }
            else
            {
                order.Status = "Failed";
                _unitOfWork.OrderRepository.Update(order);
            }

            if (response.Success)
            {
                return Redirect($"http://localhost:3000/payment-success?orderId={orderId}&paymentId={response.PaymentId}&status=success");
            }
            else
            {
                return Redirect($"http://localhost:3000/payment-failed?orderId={orderId}&status=failed");
            }
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
            var userId = User.FindFirst("UserID")?.Value; ;
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
    }
}
