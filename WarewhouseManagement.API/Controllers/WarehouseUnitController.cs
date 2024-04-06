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
    [ApiExplorerSettings(GroupName = "WarehouseUnit")]
    public class WarehouseUnitController : ControllerBase
    {
        private readonly IWarehouseUnitService _warehouseUnitService;
        private readonly IMapper _mapper;

        public WarehouseUnitController(IWarehouseUnitService warehouseUnitService, IMapper mapper)
        {
            _mapper = mapper;
            _warehouseUnitService = warehouseUnitService;
        }

        /// <summary>
        /// Получить список SKU
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<WarehouseUnitResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _warehouseUnitService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => _mapper.Map<WarehouseUnitResponse>(x)));
        }

        /// <summary>
        /// Получить SKU по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(WarehouseUnitResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var item = await _warehouseUnitService.GetByIdAsync(id, cancellationToken);
            return Ok(_mapper.Map<WarehouseUnitResponse>(item));
        }

        /// <summary>
        /// Добавить SKU
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(WarehouseUnitResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(WarehouseUnitCreateRequest model, CancellationToken cancellationToken)
        {
            var warehouseUnitModel = _mapper.Map<WarehouseUnitModelRequest>(model);
            var result = await _warehouseUnitService.AddAsync(warehouseUnitModel, cancellationToken);
            return Ok(_mapper.Map<WarehouseUnitResponse>(result));
        }

        /// <summary>
        /// Изменить SKU
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(WarehouseUnitResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(WarehouseUnitRequest request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<WarehouseUnitModelRequest>(request);
            var result = await _warehouseUnitService.EditAsync(model, cancellationToken);
            return Ok(_mapper.Map<WarehouseUnitResponse>(result));
        }

        /// <summary>
        /// Удалить SKU по Id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Delete([Required] Guid id, CancellationToken cancellationToken)
        {
            await _warehouseUnitService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
