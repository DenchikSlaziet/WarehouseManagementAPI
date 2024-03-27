using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Common.Entity.InterfaceToWorkDB;
using WarehouseManagement.Context.Configuration.Configurations;
using WarehouseManagement.Context.Contracts;
using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Context
{
    public class WarehouseManagementContext : DbContext,
        IWarehouseManagementContext,
        IDbRead,
        IDbWriter,
        IUnitOfWork
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseUnit> WarehouseUnits { get; set; }
        public DbSet<WarehouseWarehouseUnit> WarehouseWarehouseUnits { get; set; }

        public WarehouseManagementContext(DbContextOptions<WarehouseManagementContext> options): base(options)
        {
                
        }

        /// <summary>
        /// Конфигурация БД
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductEntityTypeConfiguration).Assembly);
        }

        /// <summary>
        /// Добавление сущности из БД
        /// </summary>
        void IDbWriter.Add<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Added;

        /// <summary>
        /// Удаление сущности из БД
        /// </summary>
        void IDbWriter.Delete<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Deleted;

        /// <summary>
        /// Чтение сущностей из БД
        /// </summary>
        IQueryable<TEntity> IDbRead.Read<TEntity>()
         => base.Set<TEntity>()
                .AsNoTracking()
                .AsQueryable();

        /// <summary>
        /// Сохранение изменений в БД
        /// </summary>
        async Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        {
            var count = await base.SaveChangesAsync(cancellationToken);
            foreach (var entry in base.ChangeTracker.Entries().ToArray())
            {
                entry.State = EntityState.Detached;
            }
            return count;
        }

        /// <summary>
        /// Обновление сущности из БД
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        void IDbWriter.Update<TEntity>(TEntity entity)
        => base.Entry(entity).State = EntityState.Modified;
    }
}