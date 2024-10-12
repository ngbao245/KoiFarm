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

            //foreach (var item in responsePayments)
            //{
            //    // Check if the order exists
            //    if (item.Order != null)
            //    {
            //        // Find the corresponding order in the original payments list
            //        var originalPayment = payments.FirstOrDefault(p => p.Order != null && p.Order.Id == item.OrderId);

            //        // If originalPayment is found and its Order is valid
            //        if (originalPayment != null && originalPayment.Order?.Items != null)
            //        {
            //            // Map each OrderItem to OrderItemResponseModel
            //            item.Order.Items = originalPayment.Order.Items.Select(orderItem => new OrderItemResponseModel
            //            {
            //                ProductItemId = orderItem.ProductItemId,
            //                Quantity = orderItem.Quantity,
            //                Price = _unitOfWork.ProductItemRepository.GetById(orderItem.ProductItemId)?.Price ?? 0 // Fallback to 0 if product item is not found
            //            }).ToList();
            //        }
            //        else
            //        {
            //            // If there are no items, initialize it to an empty list
            //            item.Order.Items = new List<OrderItemResponseModel>();
            //        }
            //    }
            //}

            return Ok(new ResponseModel
            {
                StatusCode = 200,
                Data = responsePayments
            });
        }
    }
}
