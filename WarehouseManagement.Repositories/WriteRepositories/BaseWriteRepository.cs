using System.Diagnostics.CodeAnalysis;
using WarehouseManagement.Common.Entity.EntityInterface;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Repositories.Contracts.WriteRepositories;

namespace WarehouseManagement.Repositories.WriteRepositories
{
    /// <summary>
    /// Абстрактный класс работы с БД, реализующий <see cref="IRepositoryWriter"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseWriteRepository<T> : IRepositoryWriter<T> where T : class, IEntity
    {
        /// <inheritdoc cref="IDbWriterContext"/>
        private readonly IDbWriterContext _writerContext;

        /// <summary>
        /// Инициализирует новый экземпляр <see cref="BaseWriteRepository{T}"/>
        /// </summary>
        protected BaseWriteRepository(IDbWriterContext writerContext)
        {
            _writerContext = writerContext;
        }

        /// <inheritdoc cref="IRepositoryWriter{T}"/>
        public virtual void Add([NotNull] T entity)
        {
            if (entity is IEntityWithId entityWithId &&
                entityWithId.Id == Guid.Empty)
            {
                entityWithId.Id = Guid.NewGuid();
            }
            AuditForCreate(entity);
            AuditForUpdate(entity);
            _writerContext.Writer.Add(entity);
        }

        /// <inheritdoc cref="IRepositoryWriter{T}"/>
        public void Update([NotNull] T entity)
        {
            AuditForUpdate(entity);
            _writerContext.Writer.Update(entity);
        }

        /// <inheritdoc cref="IRepositoryWriter{T}"/>
        public void Delete([NotNull] T entity)
        {
            AuditForUpdate(entity);
            AuditForDelete(entity);
            if (entity is IEntityAuditDeleted)
            {
                _writerContext.Writer.Update(entity);
            }
            else
            {
                _writerContext.Writer.Delete(entity);
            }
        }

        private void AuditForCreate([NotNull] T entity)
        {
            if (entity is IEntityAuditCreated auditCreated)
            {
                auditCreated.CreatedAt = _writerContext.DateTimeProvider.UtcNow;
                auditCreated.CreatedBy = _writerContext.UserName;
            }
        }

        private void AuditForUpdate([NotNull] T entity)
        {
            if (entity is IEntityAuditUpdated auditUpdate)
            {
                auditUpdate.UpdatedAt = _writerContext.DateTimeProvider.UtcNow;
                auditUpdate.UpdatedBy = _writerContext.UserName;
            }
        }

        private void AuditForDelete([NotNull] T entity)
        {
            if (entity is IEntityAuditDeleted auditDeleted)
            {
                auditDeleted.DeletedAt = _writerContext.DateTimeProvider.UtcNow;
            }
        }
    }
}
