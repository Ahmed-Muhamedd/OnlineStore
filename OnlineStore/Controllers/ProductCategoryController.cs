using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Core.Interfaces;
using OnlineStore.Core.Models;
using OnlineStore.Dtos;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IBaseRepository<ProductCategory> _IBaseRepository;
        public ProductCategoryController(IBaseRepository<ProductCategory> categoryService)
        {
            _IBaseRepository = categoryService;
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddCategory([FromBody]AddCategoryDTO PrdCategory)
        {
            bool Exist = await _IBaseRepository.IsExist(n => n.Name == PrdCategory.CategoryName);

            if (Exist) return BadRequest(new ResponseModel
            {
                IsSuccess = false,
                Message = "Category Already Exist"
            });
 
            var prdCat =  await _IBaseRepository.AddAsync(new ProductCategory { Name = PrdCategory.CategoryName});


            if (prdCat.ID == 0)
                return StatusCode(500, new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Failed to add category"
                });

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Category Added Successfully",
                Data = new { ID = prdCat.ID, Category =  PrdCategory }
            });
        }

        [HttpDelete("delete/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute]int ID)
        {
            bool deleted = await _IBaseRepository.DeleteAsync(ID);
            if (!deleted) return NotFound(new ResponseModel
            {
                IsSuccess = false,
                Message = "Not deleted",
                Data = ID
            });


            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Deleted Successfully",
                Data = ID
            });
        }


        [HttpGet("find/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel>> Find([FromRoute] string name)
        {
            var Found = await _IBaseRepository.Find(n => n.Name == name);
            
            if (Found is null) return NotFound(new ResponseModel
            {
                IsSuccess = false,
                Message = "Not found",
                Data = name
            });

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "succeeded",
                Data = new CategoryResDto
                {
                    ID = Found.ID,
                    CategoryName = Found.Name
                }
            });

        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel>> Find([FromRoute] int id)
        {
            var Found = await _IBaseRepository.GetByIDAsync(id);

            if (Found is null) return NotFound(new ResponseModel
            {
                IsSuccess = false,
                Message = "Not found",
                Data = id
            });

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "succeeded",
                Data = new CategoryResDto
                {
                    ID = Found.ID,
                    CategoryName = Found.Name
                }
            });

        }


        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel>> GetAll()
        {
            IEnumerable<ProductCategory> categories = await _IBaseRepository.GetAllAsync();

            if (categories.Count() == 0 ) return NotFound(new ResponseModel
            {
                IsSuccess = false,
                Message = "No data",
            });

            List<CategoryResDto> categList = new List<CategoryResDto>();

            foreach (var cat in categories)
                categList.Add(new CategoryResDto { ID = cat.ID , CategoryName = cat.Name});
            

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "succeeded",
                Data = categList
            });

        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel>> Update([FromBody]CategoryResDto UpdatedCategory)
        {
            bool IDExist = await _IBaseRepository.IsExist(pk => pk.ID == UpdatedCategory.ID);

            if (!IDExist) return NotFound(new ResponseModel
            {
                IsSuccess = false,
                Message = "ID not exist",
                Data = UpdatedCategory.ID
            });

            bool updated = await _IBaseRepository.UpdateAsync(new ProductCategory { ID = UpdatedCategory.ID 
                , Name = UpdatedCategory.CategoryName});

            if (!updated) return StatusCode(500,new ResponseModel
            {
                IsSuccess = false,
                Message = "No data",
            });

         

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Updated",
                Data = UpdatedCategory
            });

        }


        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel>> Update([FromBody] CategoryResDto UpdatedCategory, [FromRoute]int id)
        {
            bool IDExist = await _IBaseRepository.IsExist(pk => pk.ID == id);

            if (!IDExist) return BadRequest(new ResponseModel
            {
                IsSuccess = false,
                Message = "ID not exist",
                Data = UpdatedCategory.ID
            });

            bool updated = await _IBaseRepository.UpdateAsync(new ProductCategory
            {
                ID = UpdatedCategory.ID
                ,
                Name = UpdatedCategory.CategoryName
            } , id);

            if (!updated) return StatusCode(500, new ResponseModel
            {
                IsSuccess = false,
                Message = "No data",
            });



            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = "Updated",
                Data = UpdatedCategory
            });

        }
    }
}
