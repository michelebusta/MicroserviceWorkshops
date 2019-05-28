namespace Shared.Models
{
    public class LoginFailedModel: UserNameModel
    {
        public int RetryCount { get; set; }
    }
}