using AutoMapper;
using Fish_Manage.Models;
using Fish_Manage.Models.DTO.Order;
using Fish_Manage.Models.DTO.Product;
using Fish_Manage.Repository.DTO;
using Fish_Manage.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Fish_Manage.Controllers
{
    [Route("api/FishOrderAPI")]
    [ApiController]
    public class FishOrderAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IOrderRepository _dbOrder;
        private readonly IMapper _mapper;

        public FishOrderAPIController(IOrderRepository dbOrder, IMapper mapper)
        {
            _response = new();
            _dbOrder = dbOrder;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "admin")]
        //[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetOrders()
        {
            IEnumerable<Order> productList;
            productList = await _dbOrder.GetAllAsync();
            _response.Result = _mapper.Map<List<OrderDTO>>(productList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpGet("{id:int}", Name = "GetOrder")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var order = await _dbOrder.GetAsync(u => u.OrderId == id);
                if (order == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<OrderDTO>(order);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("CreateOrder")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateOrder([FromBody] OrderCreateDTO createDTO)
        {
            try
            {

                    if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }

                Order order = _mapper.Map<Order>(createDTO);

                await _dbOrder.CreateAsync(order);
                _response.Result = _mapper.Map<OrderDTO>(order);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetOrder", new { id = order.OrderId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteOrder")]
        public async Task<ActionResult<APIResponse>> DeleteOrder(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var order = await _dbOrder.GetAsync(u => u.OrderId == id);
                if (order == null)
                {
                    return NotFound();
                }
                await _dbOrder.RemoveAsync(order);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] OrderUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.OrderId)
                {
                    return BadRequest();
                }

                Order model = _mapper.Map<Order>(updateDTO);

                await _dbOrder.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("GetMoneyPerYear")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public double GetMoneyPerYear(int term)
        {
            switch (term)
            {
                case 1:
                    return _dbOrder.GetMoneyPerTerm(1);
                case 2:
                    return _dbOrder.GetMoneyPerTerm(2);
                case 3:
                    return _dbOrder.GetMoneyPerTerm(3);
                default:
                    return 0.0;

            }
        }

        [HttpPost("GetMostMoneyBill")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApplicationUser GetMostMoneyBill(int term)
        {
            switch (term)
            {
                case 1:
                    return _dbOrder.UserBuyMost(1);
                case 2:
                    return _dbOrder.UserBuyMost(2);
                case 3:
                    return _dbOrder.UserBuyMost(3);
                default:
                    return null;

            }
        }
    }
}
