using AutoMapper;
using Fish_Manage.Models;
using Fish_Manage.Models.DTO.Order;
using Fish_Manage.Models.Momo;
using Fish_Manage.Repository.DTO;
using Fish_Manage.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Fish_Manage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentAPIController : ControllerBase
    {
        private readonly FishManageContext _db;
        protected APIResponse _response;
        private readonly IMomoService _momoService;
        private readonly IPaymentCODService _paymentCODService;
        private readonly IMapper _mapper;

        public PaymentAPIController(IMomoService momoService, FishManageContext db, IPaymentCODService paymentCODService, IMapper mapper)
        {
            _momoService = momoService;
            _db = db;
            _paymentCODService = paymentCODService;
            _mapper = mapper;
            _response = new APIResponse();
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
        [HttpPost("CreatePaymentCOD")]
        public async Task<ActionResult<APIResponse>> CreatePaymentCOD(OrderCreateDTO createDTO)
        {
            try
            {
                if (createDTO == null)
                {
                    return BadRequest(new APIResponse { IsSuccess = false, ErrorMessages = new List<string> { "Invalid order data" } });
                }

                Order order = _mapper.Map<Order>(createDTO);
                await _paymentCODService.CreateAsync(order);

                _response.Result = _mapper.Map<OrderDTO>(order);
                _response.StatusCode = HttpStatusCode.Created;

                //return CreatedAtAction("GetOrder", "FishOrderAPIController", new { id = order.OrderId }, _response);
                return StatusCode(201, _response);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.InternalServerError;

                return StatusCode(500, _response);
            }
        }


        [HttpGet]
        [Route("api/[controller]/callback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var requestQuery = HttpContext.Request.Query;
            Console.WriteLine($"Request Query: {requestQuery}");

            var response = await _momoService.PaymentExecuteAsync(requestQuery);

            // Ensure response is valid
            if (response == null)
            {
                Console.WriteLine("Error: MoMo payment response is null.");
                return BadRequest("Invalid MoMo payment response.");
            }

            Console.WriteLine($"Response Amount: {response.Amount}");

            var newOrder = new Order
            {
                OrderId = response.OrderId,
                UserId = DateTime.Now.Ticks.ToString(),
                OrderDate = DateTime.Now,
                TotalAmount = response.Amount,
                PaymentMethod = "MoMo",
            };

            Console.WriteLine("Order details: " + Newtonsoft.Json.JsonConvert.SerializeObject(newOrder));

            await _momoService.CreateAsync(newOrder);

            return Redirect($"http://localhost:5173/CallBack?resultCode={requestQuery["resultCode"]}&orderId={response.OrderId}");
        }




    }
}

