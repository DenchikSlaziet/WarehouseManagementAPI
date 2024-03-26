using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Common.Entity.InterfaceToWorkDB
{
    /// <summary>
    /// Интерфейс получения даты
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Текущий момент (utc)
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
