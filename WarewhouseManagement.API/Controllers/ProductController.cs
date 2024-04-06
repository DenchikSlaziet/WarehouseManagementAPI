using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WarehouseManagement.Services.Contracts.Contracts;
using WarehouseManagement.Services.Contracts.Models;
using WarehouseManagement.API.Exceptions;
using WarehouseManagement.API.Models.CreateRequest;
using WarehouseManagement.API.Models.Request;
using WarehouseManagement.API.Models.Response;

namespace WarehouseManagement.API.Controllers
{
    /// <summary>
    /// CRUD контроллер по работе с товарами
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [ApiExplorerSettings(GroupName = "Product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить список товаров
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ProductResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllAsync(cancellationToken);
            return Ok(result.Select(x => _mapper.Map<ProductResponse>(x)));
        }

        /// <summary>
        /// Получить товар по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> GetById([Required] Guid id, CancellationToken cancellationToken)
        {
            var item = await _productService.GetByIdAsync(id, cancellationToken);
            return Ok(_mapper.Map<ProductResponse>(item));
        }

        /// <summary>
        /// Добавить товар
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Add(ProductCreateRequest model, CancellationToken cancellationToken)
        {
            var productModel = _mapper.Map<ProductModel>(model);
            var result = await _productService.AddAsync(productModel, cancellationToken);
            return Ok(_mapper.Map<ProductResponse>(result));
        }

        /// <summary>
        /// Изменить товар
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationExceptionDetail), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Edit(ProductRequest request, CancellationToken cancellationToken)
        {
            var model = _mapper.Map<ProductModel>(request);
            var result = await _productService.EditAsync(model, cancellationToken);
            return Ok(_mapper.Map<ProductResponse>(result));
        }

        /// <summary>
        /// Удалить товар по Id
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiExceptionDetail), StatusCodes.Status417ExpectationFailed)]
        public async Task<IActionResult> Delete([Required] Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeleteAsync(id, cancellationToken);
            return Ok();
        }
    }
}
