using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        #region Read Methods
        IQueryable<T> FindAll(bool trackChanges);
        // added trackChanges parameter for gaining performance improvement.
        // If you want all of the entities but you won't neither change nor delete them, you wont need Tracking
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression ,bool trackChanges);
        // applied an expression for finding for a specific condition
        #endregion

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
