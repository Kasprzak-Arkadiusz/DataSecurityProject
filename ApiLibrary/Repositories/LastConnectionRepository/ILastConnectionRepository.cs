using ApiLibrary.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiLibrary.Repositories.LastConnectionRepository
{
    public interface ILastConnectionRepository
    {
        public Task<IEnumerable<LastConnection>> GetByUserIdAsync(int userId, int count);

        public Task CreateAsync(LastConnection connection);
    }
}