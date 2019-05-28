using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Storage;
using Shared.Utils;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityManagementApi
{
    [Route("api/v1/passworduser")]
    public class PasswordUserController : Controller
    {
        private readonly IIdentityManagementStore _store;
        private readonly Guid _userIdNamespace = new Guid("41F2CB0F-A6CE-4B58-9AA9-14B9FC1EFB4A");

        public PasswordUserController(IIdentityManagementStore store)
        {
            _store = store;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> UserCreated([FromBody]PasswordUserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dto = new PasswordUser
            {
                Id = CreateUserId(model.UserName),
                Email = model.Email,
                UserName = model.UserName,
                Password = model.Password
            };
            var id = await _store.Create(dto);
            return Ok(id);
        }

        private Guid CreateUserId(string userName)
        {
            return NamedGuidCreator.CreateFrom(_userIdNamespace, userName);
        }

        [HttpPost]
        [Route("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailModel confirmation)
        {
            var user = await _store.Read(CreateUserId(confirmation.UserName));
            user.ConfirmationToken = confirmation.Token;
            await _store.Save(user);
            return Ok();
        }

        [HttpPost]
        [Route("userLoginFailed")]
        public async Task<IActionResult> LoginFailed([FromBody] LoginFailedModel model)
        {
            var user = await _store.Read(CreateUserId(model.UserName));
            user.UnsuccessfulLoginAttempts = model.RetryCount;
            await _store.Save(user);
            return Ok();
        }

        [HttpPost]
        [Route("userLockout")]
        public async Task<IActionResult> UserLockedOut([FromBody] UserNameModel model)
        {
            var user = await _store.Read(CreateUserId(model.UserName));
            user.IsLockedOut = true;
            await _store.Save(user);
            return Ok();
        }

        [HttpPost]
        [Route("userUnlock")]
        public async Task<IActionResult> UserUnlocked([FromBody] UserNameModel model)
        {
            var user = await _store.Read(CreateUserId(model.UserName));
            user.IsLockedOut = false;
            await _store.Save(user);
            return Ok();
        }

        [HttpGet]
        [Route("health")]
        public ActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
