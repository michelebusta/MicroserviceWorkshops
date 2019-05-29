using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Shared.Storage
{
    public interface IIdentityManagementStore
    {
        Task<string> Create(PasswordUser user);
        Task Create(JObject data);
        Task<PasswordUser> Read(Guid id);
        Task Save(PasswordUser user);
    }
}