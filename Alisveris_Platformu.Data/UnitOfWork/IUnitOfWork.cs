using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Data.UnitOfWork
{
    //unitofwork kaydetme gibi zincir işlemleri kullanmak için açılır
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        //kaç kayda etki eder? -bu yüzden int-

        Task BeginTransaction();
        //task asenkronların voidi

        Task CommitTransaction();

        Task RollBackTransaction();
    }
}
