using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSmart.Domain.Base
{
    public interface IEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        int Id { get; set; }
    }
}
