using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WarehouseManagement.Services.Contracts.Contracts;
using WarehouseManagement.Services.Contracts.ModelsRequest;
using WarehouseManagement.API.Exceptions;
using WarehouseManagement.API.Models.CreateRequest;
using WarehouseManagement.API.Models.Request;
using WarehouseManagement.API.Models.Response;

namespace WarehouseManagement.API.Controllers
{
    [ApiController]
    [Route ("[Controller]")]
    [ApiExplorerSettings(GroupName = "Warehouse")]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService warehouseService;
        private readonly IMapper mapper;

        public WarehouseController(IWarehouseService warehouseService, IMapper mapper)
        {
            this.warehouseService = warehouseService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Получить список складов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<WarehouseResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await warehouseService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => mapper.Map<WarehouseResponse>(x)));
        }

        /// <summary>
        /// Получить склад по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WarehouseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var item = await warehouseService.GetByIdAsync(id, cancellationToken);
            return Ok(mapper.Map<WarehouseResponse>(item));
        }

        /// <summary>
        /// Добавить склад
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(WarehouseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(WarehouseCreateRequest model, CancellationToken cancellationToken)
        {
            var warehouseModel = mapper.Map<WarehouseModelRequest>(model);
            var result = await warehouseService.AddAsync(warehouseModel, cancellationToken);
            return Ok(mapper.Map<WarehouseResponse>(result));
        }

        /// <summary>
        /// Изменить склад
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(WarehouseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(WarehouseRequest request, CancellationToken cancellationToken)
        {
            var model = mapper.Map<WarehouseModelRequest>(request);
            var result = await warehouseService.EditAsync(model, cancellationToken);
            return Ok(mapper.Map<WarehouseResponse>(result));
        }

        /// <summary>
        /// Удалить склад по Id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Delete([Required] Guid id, CancellationToken cancellationToken)
        {
            await warehouseService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
