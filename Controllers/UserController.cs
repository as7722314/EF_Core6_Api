using CoreApiTest.Models;
using CoreApiTest.Resource.Helpers;
using CoreApiTest.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreApiTest.Controllers
{
    [Route("api/user/")]
    [ApiController]    
    public class UserController : ControllerBase
    {
        // GET: api/<UsersController>
        private readonly IUserService _userService;
        private readonly ToUserApiResource _toUserApiResource;

        public UserController(IUserService userService, ToUserApiResource toUserApiResource)
        {
            _userService = userService;
            _toUserApiResource = toUserApiResource;
        }

        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllUsers();
            return Ok(new { data = _toUserApiResource.DoConvertForList(users) });
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user is null) return NoContent();
            return Ok(_toUserApiResource.DoConvertForModel(user));
        }

        // POST api/<UsersController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var new_user = await _userService.CreateUser(user);
            return Ok(new_user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] User users)
        {
            var o_user = await _userService.GetUserById(id);
            if (o_user is null) return NoContent();
            var new_user = await _userService.UpdateUser(o_user, users);
            return Ok(new_user);            
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user is null) return NoContent();
            _userService.DeleteUser(user);
            return Ok();
        }
    }
}
