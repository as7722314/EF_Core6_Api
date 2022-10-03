using AutoMapper;
using CoreApiTest.Interface;
using CoreApiTest.Models;
using CoreApiTest.Resource;
using CoreApiTest.Resource.Helpers;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreApiTest.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly ToOrderApiResource _toOrderApiResource;

        public OrderController(IOrderService orderService, ToOrderApiResource toOrderApiResource, IMapper mapper)
        {
            _orderService = orderService;
            _toOrderApiResource = toOrderApiResource;
            _mapper = mapper;
        }

        // GET: api/<OrderController>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders([FromQuery] string sortName, string sort)
        {
            var orders = await _orderService.GetOrders(sortName, sort);
            var _mappedOrder = _mapper.Map<List<OrderApiResource>>(orders);
            return Ok(_mappedOrder);
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            var _mappedOrder = _mapper.Map<OrderApiResource>(order);
            return Ok(_mappedOrder);
        }

        // POST api/<OrderController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddOrder([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                string id = User.FindFirstValue(ClaimTypes.Name);
                var new_order = await _orderService.CreateOrder(int.Parse(id), order);
                return Ok(new { data = _toOrderApiResource.DoConvertForModel(new_order) });
            }
            return BadRequest();
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}