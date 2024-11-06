using Alisveris_Platformu.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Alisveris_PlatformuDbContext _db;
        private IDbContextTransaction _transaction;

        public UnitOfWork(Alisveris_PlatformuDbContext db)
        {
            _db = db;
        }

        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
            //o an silmiiyor, silinebilir yapıyor (garbage collector)
        }

        public async Task RollBackTransaction()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
