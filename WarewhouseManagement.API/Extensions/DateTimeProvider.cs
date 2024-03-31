using WarehouseManagement.Common.Entity.InterfaceToWorkDB;

namespace WarehouseManagement.API.Extensions
{
    /// <summary>
    /// Реализация <see cref="IDateTimeProvider"/>
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        DateTimeOffset IDateTimeProvider.UtcNow => DateTimeOffset.UtcNow;
    }
}
