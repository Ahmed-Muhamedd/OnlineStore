using Microsoft.AspNetCore.Mvc;
using OnlineStore.Common;
using OnlineStore.Dtos;
using OnlineStore.Interfaces;
using OnlineStore.Models;
using System.Collections;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService IProductRepository)
        {
            _productService = IProductRepository;
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ResponseModel>> CreateAsync([FromBody]CreateProductDto productDto)
        {
            var result = await _productService.AddAsync(productDto);


            return ReturnResponse(result);

            //if (!result.IsSuccess)
            //    return NotFound(new ResponseModel
            //    {
            //        IsSuccess = false,
            //        Message = result.ErrorMessage!,
            //    });

            //return Ok(new ResponseModel
            //{
            //    IsSuccess = true,
            //    Message = "Success",
            //    Data = productDto
            //});
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> UpdateAsync( [FromRoute]int id ,[FromBody] UpdateProductDto productDto)
        {
            var result = await _productService.UpdateAsync(id,productDto);

            return ReturnResponse(result);
       
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> GetAllAsync()
        {
            var result = await _productService.GetAllAsync(withInclude: true);

            return ReturnResponse(result);

        }

        [HttpGet("get/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> GetAsync([FromRoute]int id)
        {
            var result = await _productService.GetAsync( p => p.ID == id);
          return  ReturnResponse(result);

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> PaginationAsync([FromQuery]int takeNum ,[FromQuery] int pageNum)
        {
            var list = await _productService.PaginationAsync(take:takeNum, page:pageNum);

            return ReturnResponse(list);
        }


        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseModel>> DeleteAsync([FromRoute] int productID)
        {
            if (productID <= 0)
                return BadRequest("Product ID must be negative");

            var productDeleted = await _productService.DeleteAsync(productID);
            if (!productDeleted.IsDeleted)
                return NotFound(new ResponseModel
                {
                    IsSuccess = false,
                    Message = productDeleted.Message
                });

            return Ok(new ResponseModel
            {
                IsSuccess = true,
                Message = productDeleted.Message
            });
        }

        private ActionResult<ResponseModel> ReturnResponse<T>(Result<T> result)
        {
            return result.Status switch
            {
                ResultStatus.NotFound => NotFound(new ResponseModel
                {
                    IsSuccess = false,
                    Message = result.ErrorMessage!,
                }),
                ResultStatus.BadRequest => BadRequest(new ResponseModel
                {
                    IsSuccess = false,
                    Message = result.ErrorMessage!,
                }),
                ResultStatus.ServerError => StatusCode(500, new ResponseModel { IsSuccess = false, Message = result.ErrorMessage! }),
                ResultStatus.Success => Ok(new ResponseModel
                {
                    IsSuccess = true,
                    Message = result.ErrorMessage!,
                    Data = result.Data!,
                }),
                _ => StatusCode(500, new { message = "Unknown error" })
            };
        }
    }
}
