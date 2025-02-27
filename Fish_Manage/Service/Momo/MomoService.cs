using Fish_Manage.Models.Momo;
using Fish_Manage.Service.IService;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace Fish_Manage.Service.Momo
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }
        public async Task<MomoCreatePaymentResponseModel> CreatePaymentMomo(OrderInfoModel model)
        {
            if (_options == null || _options.Value == null)
            {
                throw new NullReferenceException("Momo settings (_options) is not configured properly.");
            }
            var rawData =
      $"partnerCode={_options.Value.PartnerCode}" +
      $"&accessKey={_options.Value.Accesskey}" +
      $"&requestId={model.OrderId}" +
      $"&amount={model.Amount}" +
      $"&orderId={model.OrderId}" +
      $"&orderInfo={model.Orderinformation}" +
      $"&returnUrl={_options.Value.ReturnUrl}" +
      $"&notifyUrl={_options.Value.NotifyUrl}" +
      $"&extraData={model.FullName}";

            var signature = ComputeHmacSha256(rawData, _options.Value.Secretkey);

            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = Method.Post };

            request.AddHeader("Content-Type", "application/json");

            var requestData = new
            {
                partnerCode = _options.Value.PartnerCode,
                accessKey = _options.Value.Accesskey,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                amount = model.Amount,
                orderInfo = model.Orderinformation,
                requestId = model.OrderId,
                extraData = model.FullName,
                lang = "en",
                signature = signature,
            };

            string jsonData = JsonConvert.SerializeObject(requestData);

            request.AddStringBody(jsonData, RestSharp.ContentType.Json);

            RestResponse response = null;

            response = await client.ExecuteAsync(request);

            var paymentResponse = JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
            return paymentResponse;
        }
        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            collection.TryGetValue("amount", out var amount);
            collection.TryGetValue("orderInfo", out var orderInfo);
            collection.TryGetValue("orderId", out var orderId);

            return new MomoExecuteResponseModel
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo
            };
        }


        private string ComputeHmacSha256(string message, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


    }
}
