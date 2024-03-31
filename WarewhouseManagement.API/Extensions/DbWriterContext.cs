using WarehouseManagement.Common.Entity.InterfaceToWorkDB;

namespace WarehouseManagement.API.Extensions
{
    /// <summary>
    /// Реализация <see cref="IDbWriterContext"/>
    /// </summary>
    public class DbWriterContext : IDbWriterContext
    {
        /// <inheritdoc/>
        public IDbWriter Writer { get; }

        /// <inheritdoc/>
        public IUnitOfWork UnitOfWork { get; }

        /// <inheritdoc/>
        public IDateTimeProvider DateTimeProvider { get; }

        /// <inheritdoc/>
        public string UserName { get; } = "WarehouseManagement.Api";

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="DbWriterContext"/>
        /// </summary>
        public DbWriterContext(
            IDbWriter writer,
            IUnitOfWork unitOfWork, IDateTimeProvider provider)
        {
            Writer = writer;
            UnitOfWork = unitOfWork;
            DateTimeProvider = provider;
        }
    }
}
