using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSmart.BizService.Base;
using VisualSmart.Dao.DataQuickStart.Base;
using VisualSmart.Domain.Base;
using VisualSmart.Util;

namespace VisualSmart.BizService.Implements.Base
{   
    /// <summary>
    /// 实体服务抽象基类
    /// </summary>
    /// <typeparam name="T">实体类型</typeparam>
    /// <typeparam name="IdT">Id 类型</typeparam>
    /// <typeparam name="U">实体 DAO 类型</typeparam>
    public abstract class EntityBizService<T, U> : IEntityBizService<T>
        where T : IEntity
        where U : IEntityDao<T>
    {
        private U entityDao;
        /// <summary>
        /// 获取 / 设置 实体 DAO
        /// </summary>
        public U EntityDao
        {
            get
            {
                if (entityDao == null)
                {
                    entityDao = Activator.CreateInstance<U>();
                }
                return entityDao;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Add(T entity)
        {
            return EntityDao.Add(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            return EntityDao.Update(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public bool Delete(int Id)
        {
            return EntityDao.Delete(Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAllDomain(QueryCondition query)
        {
            return EntityDao.GetAllDomain(query);
        }
    }
}
