using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webapi1.Models;

namespace Webapi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AccountController(UserManager<IdentityUser>userManager) 
        {
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Welcome()
        {
            if(User.Identity == null|| !User.Identity.IsAuthenticated)
            {
                return Ok("you are not authenticated");
            }
            return Ok("you are authenticated");
        }
        [Authorize]//this atribute ensure only authorized users can access this end point
        [HttpGet("profile")]//this specifies that the mmethod reponds to get requests
        public async Task< IActionResult> profile()//async means the the method returns a task
            //IactionResult allows flexible return types e.g ok() and badrequest(0 
        {
            var currentUser = await userManager.GetUserAsync(User);//retrieves the information of the currenly logged user
            //await pauses execusion only for this method , but the rest of the app keeps running .
            if (currentUser == null)
            {
                return BadRequest();
            
            
            }
            var userProfile = new UserProfile
            {
                Id = currentUser.Id,
                Name = currentUser.UserName ?? "",
                Email = currentUser.Email ?? "",
                PhoneNumber = currentUser.PhoneNumber ?? "",
            };
            return Ok();
        
        }
    }
}
