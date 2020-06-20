using System;
using System.Threading.Tasks;
using account_service.Models;
using account_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace account_service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                return Ok(await _profileService.GetProfileByGuid(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpGet("GetFollowers/{id}")]
        public async Task<IActionResult> GetFollowers(Guid id)
        {
            try
            {
                return Ok(await _profileService.GetFollowers(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpGet("GetFollowing/{id}")]
        public async Task<IActionResult> GetFollowing(Guid id)
        {
            try
            {
                return Ok(await _profileService.GetFollowing(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        /// <summary>
        /// Returns true or false based on if the user is following the other user.
        /// </summary>
        /// <param name="followModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("getfollow")]
        public async Task<IActionResult> GetFollow([FromQuery] FollowModel followModel)
        {
            try
            {
                return Ok(await _profileService.GetFollow(followModel.id, followModel.idToFollow));
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromQuery] FollowModel followModel)
        {
            try
            {
                await _profileService.Follow(followModel.id, followModel.idToFollow);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [AllowAnonymous]
        [HttpPost("unfollow")]
        public async Task<IActionResult> UnFollow([FromQuery] FollowModel followModel)
        {
            try
            {
                await _profileService.UnFollow(followModel.id, followModel.idToFollow);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}