using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model.Payment
{
    public class RefundResponseModel
    {
        [JsonProperty("vnp_ResponseId")]
        public string ResponseId { get; set; }

        [JsonProperty("vnp_Command")]
        public string Command { get; set; }

        [JsonProperty("vnp_ResponseCode")]
        public string ResponseCode { get; set; }

        [JsonProperty("vnp_Message")]
        public string Message { get; set; }

        [JsonProperty("vnp_TxnRef")]
        public string OrderId { get; set; }

        [JsonProperty("vnp_Amount")]
        public string Amount { get; set; }

        [JsonProperty("vnp_OrderInfo")]
        public string OrderInfo { get; set; }

        [JsonProperty("vnp_BankCode")]
        public string BankCode { get; set; }

        [JsonProperty("vnp_TransactionNo")]
        public string TransactionNo { get; set; }

        [JsonProperty("vnp_TransactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("vnp_TransactionStatus")]
        public string TransactionStatus { get; set; }
    }

}
