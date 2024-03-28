using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.Services.Contracts.ModelsRequest;

namespace WarehouseManagement.Services.Contracts.Contracts
{
    /// <summary>
    /// Сервис <see cref="WarehouseModel"/>
    /// </summary>
    public interface IWarehouseService
    {
        /// <summary>
        /// Получить список всех <see cref="WarehouseModel"/>
        /// </summary>
        Task<IEnumerable<WarehouseModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="WarehouseModel"/> по идентификатору
        /// </summary>
        Task<WarehouseModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет новый <see cref="Warehouse"/>
        /// </summary>
        Task<WarehouseModel> AddAsync(WarehouseModelRequest modelRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Редактирует существующий <see cref="Warehouse"/>
        /// </summary>
        Task<WarehouseModel> EditAsync(WarehouseModelRequest modelRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет существующий <see cref="Warehouse"/>
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
