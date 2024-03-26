namespace WarehouseManagement.Common.Entity.InterfaceToWorkDB
{
    /// <summary>
    /// Контекст для работы с записями в БД
    /// </summary>
    public interface IDbWriterContext
    {
        /// <inheritdoc cref="IDbWriter"/>
        IDbWriter Writer { get; }

        /// <inheritdoc cref="IUnitOfWork"/>
        IUnitOfWork UnitOfWork { get; }

        /// <inheritdoc cref="IDateTimeProvider"/>
        IDateTimeProvider DateTimeProvider { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        string UserName { get; }
    }
}
