using WarehouseManagement.Context.Contracts.Models;
using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.Services.Contracts.ModelsRequest;

namespace WarehouseManagement.Test.Extensions
{
    /// <summary>
    /// Статический класс для генирации сущностей
    /// </summary>
    public static class TestDataGenerator
    {
        private static Random random = new Random();

        /// <summary>
        /// Возвращает заполненный товар
        /// </summary>
        static public Product Product(Action<Product>? settings = null)
        {
            var result = new Product
            {
                Title = $"{Guid.NewGuid():N}",
                Description = $"{Guid.NewGuid():N}"                
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        /// <summary>
        /// Возвращает заполненный склад
        /// </summary>
        static public Warehouse Warehouse(Action<Warehouse>? settings = null)
        {
            var result = new Warehouse
            {
                Title = $"{Guid.NewGuid():N}",
                Address = $"{Guid.NewGuid():N}"
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        /// <summary>
        /// Возвращает заполненную единицу складского учета (по дефолту без привязки)
        /// </summary>
        static public WarehouseUnit WarehouseUnit(Action<WarehouseUnit>? settings = null)
        {
            var result = new WarehouseUnit
            {
                Count = random.Next(0, 1000),
                Price = random.Next(10, 10000),
                Unit = string.Join("",Guid.NewGuid().ToString().Take(10))
            };
            result.BaseAuditSetParamtrs();

            settings?.Invoke(result);
            return result;
        }

        /// <summary>
        /// Возвращает заполненную модель товара
        /// </summary>
        static public ProductModel ProductModel(Action<ProductModel>? settings = null)
        {
            var result = new ProductModel
            {
                Id = Guid.NewGuid(),
                Title = $"{Guid.NewGuid():N}",
                Description = $"{Guid.NewGuid():N}"
            };

            settings?.Invoke(result);
            return result;
        }

        /// <summary>
        /// Возвращает заполненную модель склада (по дефолту без привязки)
        /// </summary>
        static public WarehouseModelRequest WarehouseModelRequest(Action<WarehouseModelRequest>? settings = null)
        {
            var result = new WarehouseModelRequest
            {
                Id = Guid.NewGuid(),
                Title = $"{Guid.NewGuid():N}",
                Address = $"{Guid.NewGuid():N}"
            };

            settings?.Invoke(result);
            return result;
        }

        /// <summary>
        /// Возвращает заполненную модель единицы складского учета (по дефолту без привязки)
        /// </summary>
        static public WarehouseUnitModelRequest WarehouseUnitModelRequest(Action<WarehouseUnitModelRequest>? settings = null)
        {
            var result = new WarehouseUnitModelRequest
            {
                Id = Guid.NewGuid(),
                Count = random.Next(1, 1000),
                Price = random.Next(100, 10000),
                Unit = string.Join("", Guid.NewGuid().ToString().Take(10))
            };

            settings?.Invoke(result);
            return result;
        }

    }
}
