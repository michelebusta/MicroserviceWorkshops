using IdentityManagementWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Messages;
using System;
using System.Threading.Tasks;

namespace IdentityManagementWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEventProducer _producer;

        public HomeController(IEventProducer producer)
        {
            _producer = producer;
        }

        public IActionResult Index()
        {
            return View(new UserStatus());
        }


        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(
            [FromForm]string username,
            [FromForm]string password,
            [FromForm]string email)
        {
            await SendMessage(new UserCreated(email, password, username, DateTimeOffset.UtcNow));
            return View("Index", new UserStatus { UserName = username, });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail([FromForm] string username, [FromForm] string token)
        {

            await SendMessage(new EmailConfirmed(username, token, DateTimeOffset.UtcNow));
            return View("Index", new UserStatus { UserName = username, });
        }

        [HttpPost]
        [Route("LogIn")]
        public async Task<IActionResult> Login([FromForm]string username)
        {
            await SendMessage(new UserLoggedIn(username, DateTimeOffset.UtcNow));
            return View("Index", new UserStatus { UserName = username, });
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout([FromForm]string username)
        {
            await SendMessage(new UserLoggedOut(username, DateTimeOffset.UtcNow));
            return View("Index", new UserStatus { UserName = username, });
        }

        [HttpPost]
        [Route("LoginFailed")]
        public async Task<IActionResult> LoginFailed([FromForm]string username, [FromForm] int numberOfAttempts)
        {
            await SendMessage(new UserLoginFailed(username, numberOfAttempts, DateTimeOffset.UtcNow));
            return View("Index", new UserStatus { UserName = username, });
        }

        [HttpPost]
        [Route("LockAccount")]
        public async Task<IActionResult> LockAccount([FromForm]string username)
        {
            await SendMessage(new UserLockedOut(username, DateTimeOffset.UtcNow));
            return View("Index", new UserStatus { UserName = username, });
        }

        [HttpPost]
        [Route("UnlockAccount")]
        public async Task<IActionResult> UnlockAccount([FromForm]string username)
        {
            await SendMessage(new UserUnlocked(username, DateTimeOffset.UtcNow));
            return View("Index", new UserStatus { UserName = username, });
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm]string username)
        {
            await SendMessage(new PasswordReset(username, DateTimeOffset.UtcNow));
            return View("Index", new UserStatus { UserName = username, });
        }

        private async Task SendMessage(object message)
        {
            try
            {
                await _producer.SendAsync("identity", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
