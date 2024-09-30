using Microsoft.AspNetCore.Http;
using Repository.Model.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.PaymentService
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentRequestModel model, HttpContext context, decimal total, string orderId);
        PaymentResponseModel PaymentExecute(IQueryCollection collections, string orderID);
    }
}
