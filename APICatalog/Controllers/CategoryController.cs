using APICatalog.Context;
using APICatalog.DTOs;
using APICatalog.DTOs.Mappings;
using APICatalog.Filters;
using APICatalog.Models;
using APICatalog.Pagination;
using APICatalog.Repositories;
using APICatalog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;


namespace APICatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnityOfWork _unityOfWork;

        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ICategoryRepository repository, IUnityOfWork unityOfWork)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categories = _unityOfWork.CategoryRepository.GetAll();
            if(categories == null)
            {
                return NotFound("Nenhuma categoria cadastrada!");
            }
 
            var categoriesDTO = categories.ToCategoryDTOList();

            return Ok(categoriesDTO);   
        }
        [HttpGet("{id:int}", Name = "GetCategory")]
        public ActionResult<CategoryDTO> GetCategoryById(int id)
        {
            var category = _unityOfWork.CategoryRepository.Get(c => c.CategoryId == id);
            if (category is null)
            {
                _logger.LogWarning($"Categoria com id {id} não encontrada!");
                return NotFound("Categoria não encontrada");
            }
            var categoryDTO = category.ToCategoryDTO();
            return Ok(categoryDTO);
        }
        [HttpGet("pagination")]
        public ActionResult<IEnumerable<CategoryDTO>> Get([FromQuery] CategoriesParameters parameters)
        {
            var categories = _unityOfWork.CategoryRepository.GetCategories(parameters);

            var metadata = new
            {
                categories.PageSize,
                categories.TotalCount,
                categories.CurrentPage,
                categories.TotalPages,
                categories.hasNext,
                categories.hasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            var categoriesDTO = categories.ToCategoryDTOList();
            return Ok(categoriesDTO);
        }

        [HttpPost]
        public ActionResult<CategoryDTO> CreateCategory(CategoryDTO categoryDTO)
        {
            if(categoryDTO is null) {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var category = categoryDTO.ToCategory();

            _unityOfWork.CategoryRepository.Create(category);
            _unityOfWork.Commit();
            var newCategoryDTO = category.ToCategoryDTO();

            return new CreatedAtRouteResult("GetCategory", new { id = newCategoryDTO.CategoryId }, newCategoryDTO);
        }
        [HttpPut("{id:int}")]
        public ActionResult<CategoryDTO> EditCategory(int id, CategoryDTO categoryDTO)
        {
            if(id != categoryDTO.CategoryId)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var category = categoryDTO.ToCategory();
            _unityOfWork.CategoryRepository.Update(category);
            _unityOfWork.Commit();
            var newCategoryDTO = category.ToCategoryDTO();
            return Ok($"Categoria editada\nCategory: {newCategoryDTO}");
        }
        [HttpDelete("{id:int}")]
        public ActionResult<CategoryDTO> DeleteCategory(int id)
        {
            var category = _unityOfWork.CategoryRepository.Get(c=> c.CategoryId == id);
            if(category is null) {
                _logger.LogWarning("Categoria não encontrada");
                return NotFound("Categoria não encontrada");
            }
            var categoryExcluded = _unityOfWork.CategoryRepository.Delete(category);
            _unityOfWork.Commit();  
            var categoryDTO = categoryExcluded.ToCategoryDTO();
            _logger.LogInformation($"Categoria excluida\nCategoria: {categoryDTO}");
            return Ok($"Categoria excluída\nCategory: {categoryDTO}");     
        }
    }
}
