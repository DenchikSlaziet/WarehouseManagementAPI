using WarehouseManagement.Services.Contracts.Models;

namespace WarehouseManagement.Services.Contracts.Contracts
{
    /// <summary>
    /// Сервис <see cref="ProductModel"/>
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Получить список всех <see cref="ProductModel"/>
        /// </summary>
        Task<IEnumerable<ProductModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="ProductModel"/> по идентификатору
        /// </summary>
        Task<ProductModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет новый <see cref="Product"/>
        /// </summary>
        Task<ProductModel> AddAsync(ProductModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Редактирует существующий <see cref="Product"/>
        /// </summary>
        Task<ProductModel> EditAsync(ProductModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет существующий <see cref="Product"/>
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
