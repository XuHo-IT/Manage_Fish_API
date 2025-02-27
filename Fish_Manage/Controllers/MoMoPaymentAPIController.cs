using Fish_Manage.Models;
using Fish_Manage.Models.Momo;
using Fish_Manage.Repository.IRepository;
using Fish_Manage.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Fish_Manage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoMoPaymentAPIController : ControllerBase
    {
        private readonly FishManageContext _db;
        private readonly IMomoService _momoService;
        private readonly IOrderRepository _orderRepository;

        public MoMoPaymentAPIController(IMomoService momoService, IOrderRepository orderRepository, FishManageContext db)
        {
            _momoService = momoService;
            _orderRepository = orderRepository;
            _db = db;
        }

        [HttpPost("CreatePaymentMomo")]
        public async Task<IActionResult> CreatePaymentMomo([FromBody] OrderInfoModel model)
        {
            Console.WriteLine($"[INFO] Received MoMo Payment Request: {JsonConvert.SerializeObject(model)}");

            if (model == null)
            {
                Console.WriteLine("[ERROR] Request body is missing or invalid.");
                return BadRequest(new { message = "Request body is missing or invalid." });
            }

            if (model.Amount == "")
            {
                Console.WriteLine("[ERROR] Invalid payment amount. Amount must be greater than zero.");
                return BadRequest(new { message = "Invalid payment amount" });
            }

            try
            {
                var response = await _momoService.CreatePaymentMomo(model);

                if (response == null)
                {
                    Console.WriteLine("[ERROR] Failed to create MoMo payment.");
                    return BadRequest(new { message = "Failed to create MoMo payment." });
                }

                Console.WriteLine($"[SUCCESS] MoMo payment created successfully: {JsonConvert.SerializeObject(response)}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EXCEPTION] Error creating MoMo payment: {ex.Message}");
                return StatusCode(500, new { message = "An internal server error occurred.", error = ex.Message });
            }
        }



        [HttpGet]
        [Route("api/[controller]/callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var requestQuery = HttpContext.Request.Query;
            Console.WriteLine(requestQuery.ToString());
            var response = _momoService.PaymentExecuteAsync(requestQuery);
            //string fullName = requestQuery["extraData"];
            //string userId = await FindUserIdByFullName(fullName);
            var newOrder = new Order
            {
                OrderId = response.OrderId,
                UserId = DateTime.Now.Ticks.ToString(),
                OrderDate = DateTime.Now,
                TotalAmount = response.Amount,
                PaymentMethod = "MoMo",
            };
            Console.WriteLine("The detail order" + newOrder);
            await _orderRepository.CreateAsync(newOrder);

            return Redirect($"http://localhost:5173/CallBack?resultCode={requestQuery["resultCode"]}&orderId={response.OrderId}");
        }

        //private async Task<string> FindUserIdByFullName(string fullName)
        //{
        //    var user = await _db.ApplicationUsers.FindAsync(fullName);

        //    if (user == null)
        //    {
        //        throw new Exception("User not found.");
        //    }

        //    return user.Id;


        //}
    }
}

