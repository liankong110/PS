using System;
using VisualSmart.BizService.Implements.Base;
using VisualSmart.BizService.Pro;
using VisualSmart.Dao.DataQuickStart.Pro;
using VisualSmart.Domain.Pro;


namespace VisualSmart.BizService.Implements.Pro
{
    public class Pro_PSBizService : EntityBizService<Pro_PS, Pro_PSDao>, IPro_PSBizService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddGetId(Pro_PS entity)
        {
            return EntityDao.AddGetId(entity);
        }
        public Pro_PS GetPSBySchedulingLineId(int SchedulingLineId)
        {           
            var schedulingLineModel = new Pro_SchedulingLineBizService().GetAllDomain(
                Util.QueryCondition.Instance.AddEqual("Id", SchedulingLineId.ToString()))[0];
            Pro_PS model = new Pro_PS {
                FinalEveningNum = schedulingLineModel.EveningShift,
                FinalMiddleNum = schedulingLineModel.MiddleShift,
                FinalMorningNum = schedulingLineModel.MorningShift,
                ProLineNo = schedulingLineModel.ProLineNo,
                ProDate = DateTime.Now
            };
            return model;
        }
            
    }
}
