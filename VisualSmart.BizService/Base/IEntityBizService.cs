using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.Domain.Base;
using VisualSmart.Util;

namespace VisualSmart.BizService.Base
{ 
    /// <summary>
    /// 实体服务根接口
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    public interface IEntityBizService<T> where T : IEntity
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
        /// <returns></returns>
        IList<T> GetAllDomain(QueryCondition query);
    }
}
