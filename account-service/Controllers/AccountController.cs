using System;
using System.Threading.Tasks;
using account_service.Exceptions;
using account_service.Models;
using account_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace account_service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel model)
        {
            try
            {
                return Ok(await _accountService.Authenticate(model.Email, model.Password));
            }
            catch (AccountByEmailOrPasswordNotFoundException ex)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            try
            {
                // create user
                _accountService.Create(model.Name, model.Email, model.Username, model.Password);
                return Ok();
            }
            catch (AlreadyInUseException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                return Ok(await _accountService.GetUserByGuid(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        // [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _accountService.GetAll());
        }

        [HttpPost("fill")]
        public async Task<IActionResult> Fill()
        {
            await _accountService.Fill();
            return Ok();
        }
    }
}