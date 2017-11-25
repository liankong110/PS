using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;
using VisualSmart.Util;

namespace VisualSmart.Dao.DataQuickStart.Base
{ 
    /// <summary>
    /// 实体 DAO 接口
    /// </summary>
    /// <typeparam name="T">实体类</typeparam>
    public interface IEntityDao<T> where T : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int Id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<T> GetAllDomain(QueryCondition query);


    }
}
