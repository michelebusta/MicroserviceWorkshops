using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IdentityRuntimeConsumer
{
    class MessageProcessor
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;

        public MessageProcessor(IConfiguration configuration, ILogger logger)
        {
            _logger = logger;
            Uri baseAddress;
            if (!Uri.TryCreate(configuration["apiEndpoint"], UriKind.Absolute, out baseAddress))
            {
                throw new ArgumentException("configuration does not contain a baseAddress", nameof(configuration));
            }
            _client = new HttpClient { BaseAddress = baseAddress };
        }

        public async Task Handle(string messageType, JObject message)
        {
            _logger.LogInformation("Processing message of type {0}", messageType);
            try
            {
                string path;
                switch (messageType)
                {
                    case "UserCreated":
                        path = "create";
                        break;
                    case "EmailConfirmed":
                        path = "confirmEmail";
                        break;
                    case "UserLoginFailed":
                        path = "userLoginFailed";
                        break;
                    case "UserLockedOut":
                        path = "userLockout";
                        break;
                    case "UserUnlocked":
                        path = "userUnlock";
                        break;
                    case "UserLoggedOut":
                    case "UserLoggedIn":
                    case "PasswordReset":
                    case "TopicCheck":
                        return;
                    default: throw new InvalidOperationException($"Unknown message type '{messageType}'");
                }
                var response = await _client.PostAsync(path, new StringContent(message.ToString(), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                _logger.LogError(1, e, "Error processing message");
                throw;
            }
        }
    }
}
