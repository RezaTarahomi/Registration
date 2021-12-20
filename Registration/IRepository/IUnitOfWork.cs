using Registration.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Registration.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<RefreshToken> RefreshTokens { get; }
        IGenericRepository<User> Users { get; }

        Task Save();
    }
}
