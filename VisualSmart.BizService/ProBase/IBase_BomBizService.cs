using VisualSmart.BizService.Base;
using VisualSmart.Domain.ProBase;

namespace VisualSmart.BizService.ProBase
{
    public interface IBase_BomBizService : IEntityBizService<Base_Bom>
    {
        /// <summary>
        /// 删除-所有信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        bool DeleteAll(int Id);
    }
}
