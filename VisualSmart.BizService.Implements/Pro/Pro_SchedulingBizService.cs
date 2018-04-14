using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;


namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_SchedulingBizService : EntityBizService<Pro_Scheduling, Pro_SchedulingDao>, IPro_SchedulingBizService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_Scheduling entity)
        {
            return EntityDao.AddGetId(entity); 
        }
    }
}
