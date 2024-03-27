using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.Services.Contracts.ModelsRequest;

namespace WarehouseManagement.Services.Contracts.Contracts
{
    /// <summary>
    /// Сервис <see cref="WarehouseUnitModel"/>
    /// </summary>
    public interface IWarehouseUnitService
    {
        /// <summary>
        /// Получить список всех <see cref="WarehouseUnitModel"/>
        /// </summary>
        Task<IEnumerable<WarehouseUnitModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Получить <see cref="WarehouseUnitModel"/> по идентификатору
        /// </summary>
        Task<WarehouseUnitModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет новый <see cref="WarehouseUnit"/>
        /// </summary>
        Task<WarehouseUnitModel> AddAsync(WarehouseUnitModelRequest modelRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Редактирует существующий <see cref="WarehouseUnit"/>
        /// </summary>
        Task<WarehouseUnitModel> EditAsync(WarehouseUnitModelRequest modelRequest, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет существующий <see cref="WarehouseUnit"/>
        /// </summary>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
