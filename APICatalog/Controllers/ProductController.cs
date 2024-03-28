using APICatalog.Context;
using APICatalog.DTOs;
using APICatalog.Models;
using APICatalog.Pagination;
using APICatalog.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;

        public ProductController(ILogger<ProductController> logger, IProductRepository productRepository, IUnityOfWork unityOfWork, IMapper mapper)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> GetProducts() 
        {
            var products = _unityOfWork.ProductRepository.GetAll();
            if (products is null)
            {
                _logger.LogWarning("Produtos não encontrados");
                return NotFound("Produtos não encontrados"); 
            }
            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);
        }
        [HttpGet("{id:int:min(1)}", Name="GetProductById")]
        public ActionResult<ProductDTO> GetProductById([FromQuery]int id)
        {
            var product = _unityOfWork.ProductRepository.Get(p=> p.ProductId == id);
            if (product is null)
            {
                _logger.LogWarning("Produto não encontrado");
                return NotFound("Produto não encontrado");
            }
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }
        [HttpGet("products/{id:int}")]
        public ActionResult <IEnumerable<ProductDTO>> GetProductsByCategory(int id)
        {
            var products = _unityOfWork.ProductRepository.GetProductsByCategory(id);
            if (products is null)
            {
                _logger.LogWarning($"Nenhum produto da categoria {id} foi encontrado");
                return NotFound($"Nenhum produto da categoria {id} foi encontrado");
            }
            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);
        }
        [HttpGet("pagination")]
        public ActionResult <IEnumerable<ProductDTO>> Get([FromQuery] ProductsParameters parameters)
        {
            var products = _unityOfWork.ProductRepository.GetProducts(parameters);

            object metadata = GetMetadata(products);
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata)); 
            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);
        }
        [HttpGet("filtering/price/pagination")]
        public ActionResult<IEnumerable<ProductDTO>> GetProductsFilteringForPrice([FromQuery] ProductsFilteringForPrice filters)
        {
            var products = _unityOfWork.ProductRepository.GetProductsFilteringForPrice(filters);

            object metadata = GetMetadata(products);
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);
        }

        [HttpPost]
        public ActionResult<ProductDTO> CreateProduct(ProductDTO productDTO)
        {
            if (productDTO is null)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var product = _mapper.Map<Product>(productDTO);
            _unityOfWork.ProductRepository.Create(product);
            _unityOfWork.Commit();
            var newProductDTO = _mapper.Map<ProductDTO>(product);
            return new CreatedAtRouteResult("GetProductById", new { id = newProductDTO.ProductId }, newProductDTO);
        }
        [HttpPatch("{id:int}/updatePartial")]
        public ActionResult<ProductDTOUpdateResponse> UpdateProduct(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchProductDTO)
        {
            if(patchProductDTO is null || id <= 0)
            {
                return BadRequest();
            }
            var product = _unityOfWork.ProductRepository.Get(p => p.ProductId == id);
            if (product is null)
            {
                return NotFound("Produto não encontrado!");
            }
            var productUpdateRequest = _mapper.Map<ProductDTOUpdateRequest>(product);
            patchProductDTO.ApplyTo(productUpdateRequest, ModelState);
            if(!ModelState.IsValid || TryValidateModel(productUpdateRequest))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(productUpdateRequest, product);
            _unityOfWork.Commit();
            var updatedProductResponse = _mapper.Map<ProductDTOUpdateResponse>(product);
            return Ok($"Produto atualizado!\nProduct: {updatedProductResponse}");
        }

        [HttpPut("{id:int}")]
        public ActionResult<ProductDTO> EditProduct(int id, ProductDTO productDTO)
        {
            if(id != productDTO.ProductId) {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var product = _mapper.Map<Product>(productDTO);
            _unityOfWork.ProductRepository.Update(product);
            _unityOfWork.Commit();
            return Ok("Produto editado");
        }
        [HttpDelete("{id:int}")]
        public ActionResult<ProductDTO> DeleteProduct(int id)
        {
            var product = _unityOfWork.ProductRepository.Get(p=> p.ProductId == id);
            if(product is null)
            {
                _logger.LogWarning($"Produto com id {id} não encontrado");
                return NotFound($"Produto com id {id} não encontrado");
            }
            _unityOfWork.ProductRepository.Delete(product);
            _unityOfWork.Commit();
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok($"Produto deletado\nProduct:{productDTO}");
        }
        private static object GetMetadata(PagedList<Product> products)
        {
            return new
            {
                products.PageSize,
                products.TotalCount,
                products.CurrentPage,
                products.TotalPages,
                products.hasNext,
                products.hasPrevious
            };
        }
    }
}
