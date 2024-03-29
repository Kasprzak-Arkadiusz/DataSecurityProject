﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ApiLibrary.Entities;

namespace ApiLibrary.Repositories.SecretRepository
{
    public interface ISecretRepository
    {
        public Task<IEnumerable<string>> GetUserServiceNamesAsync(string userName);

        public Task<Secret> GetSecretAsync(string serviceName,string userName);

        public Task CreateSecretAsync(Secret secret);

        // public Task DeleteSecret(Secret secret); TODO Think if it is needed
    }
}