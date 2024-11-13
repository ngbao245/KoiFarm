using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Repository.Model.Payment;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Repository.PaymentService
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreatePaymentUrl(PaymentRequestModel model, HttpContext context, decimal total, string orderId)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((long)total * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {total}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            //pay.AddRequestData("vnp_TxnRef", tick);
            pay.AddRequestData("vnp_TxnRef", orderId);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public PaymentResponseModel PaymentExecute(IQueryCollection collections, string orderID)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"], orderID);

            return response;
        }


        public RefundResponseModel ProcessRefund(RefundRequestModel refundRequest, HttpContext context, DateTimeOffset paymentDate, decimal amount, string userName, string orderId)
        {
            try
            {
                var pay = new VnPayLibrary();

                var vnp_Api = _configuration["Vnpay:RefundUrl"];
                var vnp_HashSecret = _configuration["Vnpay:HashSecret"];
                var vnp_TmnCode = _configuration["Vnpay:TmnCode"];

                var vnp_RequestId = DateTime.Now.Ticks.ToString();
                var vnp_Version = _configuration["Vnpay:Version"]; ;
                var vnp_Command = "refund";
                var vnp_TransactionType = "02";
                //var vnp_Amount = (refundRequest.Amount * 100).ToString();
                var vnp_Amount = ((long)amount * 100).ToString();
                //var vnp_TxnRef = refundRequest.OrderId;
                var vnp_TxnRef = orderId;
                //var vnp_OrderInfo = $"Refund for Order {refundRequest.OrderId}";
                var vnp_OrderInfo = $"Refund for Order {orderId}";
                var vnp_TransactionNo = refundRequest.TransactionNo;

                //var vnp_TransactionDate = refundRequest.PaymentDate;
                var vnp_TransactionDate = paymentDate.ToString("yyyyMMddHHmmss");

                var vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                //var vnp_CreateBy = refundRequest.CreatedBy;
                var vnp_CreateBy = userName;
                var vnp_IpAddr = pay.GetIpAddress(context);

                var signData = $"{vnp_RequestId}|{vnp_Version}|{vnp_Command}|{vnp_TmnCode}|{vnp_TransactionType}|{vnp_TxnRef}|{vnp_Amount}|{vnp_TransactionNo}|{vnp_TransactionDate}|{vnp_CreateBy}|{vnp_CreateDate}|{vnp_IpAddr}|{vnp_OrderInfo}";
                var vnp_SecureHash = pay.HmacSha512(vnp_HashSecret, signData);

                var refundData = new
                {
                    vnp_RequestId,
                    vnp_Version,
                    vnp_Command,
                    vnp_TmnCode,
                    vnp_TransactionType,
                    vnp_TxnRef,
                    vnp_Amount,
                    vnp_OrderInfo,
                    vnp_TransactionNo,
                    vnp_TransactionDate,
                    vnp_CreateBy,
                    vnp_CreateDate,
                    vnp_IpAddr,
                    vnp_SecureHash
                };

                var jsonData = JsonConvert.SerializeObject(refundData);

                // Enforce TLS 1.2
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(vnp_Api);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = 10000;

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonData);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var jsonResponse = streamReader.ReadToEnd();

                    // Deserialize the JSON response to RefundResponseModel
                    var refundResponse = JsonConvert.DeserializeObject<RefundResponseModel>(jsonResponse);

                    return refundResponse;
                }
            }
            catch (WebException webEx)
            {
                throw new Exception("Refund request failed: " + webEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing refund: " + ex.Message);
            }
        }
    }
}
