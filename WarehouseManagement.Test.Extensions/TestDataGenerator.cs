using WarehouseManagement.Context.Contracts.Models;

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

    }
}
