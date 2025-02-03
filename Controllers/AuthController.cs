using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Revisao_ASP.NET_Web_API.Models;
using Revisao_ASP.NET_Web_API.Models.Entities;

namespace Revisao_ASP.NET_Web_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // sets DI's
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //***********************************************************************************************//
        //                                            Endpoints                                          //
        //***********************************************************************************************//
        //
        // Create User
        [HttpPost]
        //[Route("Register")]
        public async Task<IActionResult> Register([FromBody] Register registerUser)
        {
            var user = new AppUser
            {
                UserName = registerUser.Email,
                Name = registerUser.Name,
                Surname = registerUser.Surname,
                Email = registerUser.Email,
            };

            // create the user into of database
            var createUser = await _userManager.CreateAsync(user, registerUser.Password);

            // check the return from CreateAsync
            if (createUser.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }

        // Login
        [HttpPost]
        //[Route("Login")]
        public async Task<IActionResult> Login([FromBody] Login userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var loginResult = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, userLogin.RememberMe, lockoutOnFailure: false);

            if (loginResult.Succeeded) 
            {
                return Ok(new {success = true});
            }

            return BadRequest(new {success = false, message = "Failed to log in"});
        }

        // Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // logout the user

            return Ok(); // return 200 ok
        }

        // Get the User Role
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var searchUser = await _userManager.GetUserAsync(User); // searh user

            if (searchUser == null)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(searchUser); // get the user role

            return Ok(roles); // return the user role
        }
    }
}
