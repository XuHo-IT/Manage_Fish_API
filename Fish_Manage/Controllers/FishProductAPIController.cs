﻿using AutoMapper;
using Fish_Manage.Models;
using Fish_Manage.Models.DTO.Product;
using Fish_Manage.Repository.DTO;
using Fish_Manage.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Fish_Manage.Controllers
{
    [Route("api/FishProductAPI")]
    [ApiController]
    public class FishProductAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IProductRepository _dbProduct;
        private readonly IMapper _mapper;

        public FishProductAPIController(IProductRepository dbProduct, IMapper mapper)
        {
            _response = new();
            _dbProduct = dbProduct;
            _mapper = mapper;
        }
        [HttpGet]
        //[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetProducts()
        {
            IEnumerable<Product> productList;
            productList = await _dbProduct.GetAllAsync();
            _response.Result = _mapper.Map<List<ProductDTO>>(productList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        [HttpGet("{id:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _dbProduct.GetAsync(u => u.ProductId == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ProductDTO>(villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromForm] ProductCreateDTO createDTO, IFormFile imageFile, [FromServices] CloudinaryService cloudinaryService)
        {
            try
            {
                if (await _dbProduct.GetAsync(u => u.ProductName.ToLower() == createDTO.ProductName.ToLower()) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "Product already Exists!");
                    return BadRequest(ModelState);
                }

                if (createDTO == null || imageFile == null)
                {
                    return BadRequest("Invalid product data or image file.");
                }

                // Upload image to Cloudinary
                string imageUrl = await cloudinaryService.UploadImageAsync(imageFile);
                Product product = _mapper.Map<Product>(createDTO);
                product.ImageURl = imageUrl;

                await _dbProduct.CreateAsync(product);
                _response.Result = _mapper.Map<ProductDTO>(product);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetProduct", new { id = product.ProductId }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }



        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteProuct")]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var product = await _dbProduct.GetAsync(u => u.ProductId == id);
                if (product == null)
                {
                    return NotFound();
                }
                await _dbProduct.RemoveAsync(product);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(
        int id,
        [FromForm] ProductUpdateDTO updateDTO,  // ✅ Fix: Ensure correct data binding for multipart/form-data
        IFormFile imageFile,
        [FromServices] CloudinaryService cloudinaryService)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.ProductId)
                {
                    return BadRequest(new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessages = new List<string> { "Invalid product data." }
                    });
                }

                var existingProduct = await _dbProduct.GetAsync(p => p.ProductId == id);
                if (existingProduct == null)
                {
                    return NotFound(new APIResponse
                    {
                        IsSuccess = false,
                        StatusCode = HttpStatusCode.NotFound,
                        ErrorMessages = new List<string> { "Product not found." }
                    });
                }

                // Preserve existing image if no new file is uploaded
                string imageUrl = existingProduct.ImageURl;
                if (imageFile != null)
                {
                    imageUrl = await cloudinaryService.UploadImageAsync(imageFile);
                }

                // Map updated fields to the existing product
                _mapper.Map(updateDTO, existingProduct);
                existingProduct.ImageURl = imageUrl;

                await _dbProduct.UpdateAsync(existingProduct);
                _response.Result = _mapper.Map<ProductDTO>(existingProduct);

                return Ok(new APIResponse
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.NoContent
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessages = new List<string> { ex.Message }
                });
            }
        }




    }
}
