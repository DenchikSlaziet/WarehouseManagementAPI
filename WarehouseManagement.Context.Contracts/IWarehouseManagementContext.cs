using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Context.Contracts.Models;

namespace WarehouseManagement.Context.Contracts
{
    public interface IWarehouseManagementContext
    {
        /// <summary>Список <inheritdoc cref="Product"/></summary>
        DbSet<Product> Products { get; }

        /// <summary>Список <inheritdoc cref="Warehouse"/></summary>
        DbSet<Warehouse> Warehouses { get; }

        /// <summary>Список <inheritdoc cref="WarehouseUnit"/></summary>
        DbSet<WarehouseUnit> WarehouseUnits { get; }

        /// <summary>Список <inheritdoc cref="WarehouseWarehouseUnit"/></summary>
        DbSet<WarehouseWarehouseUnit> WarehouseWarehouseUnits { get; }
    }
}