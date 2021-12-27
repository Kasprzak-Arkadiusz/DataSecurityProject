using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Entities;

namespace Application.Repositories.SecretRepository
{
    public interface ISecretRepository
    {
        public Task<IEnumerable<string>> GetServiceNames();
        public Task<string> GetPasswordByServiceName(string serviceName);
        public Task CreateSecret(Secret secret);
        // public Task DeleteSecret(Secret secret); TODO Think if it is needed
    }
}