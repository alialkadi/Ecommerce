using AutoMapper;
using Ecommerce.APIs.Dtos;
using Ecommerce.APIs.Errors;
using Ecommerce.APIs.Helpers;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories.Interfaces;
using Ecommerce.Core.Specifications;
using Ecommerce.Core.Specifications.ProductSpecs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.APIs.Controllers
{
	
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _product;
		private readonly IGenericRepository<ProductBrand> brands;
		private readonly IGenericRepository<ProductCategory> _categories;
		private readonly IMapper _mapper;

		public ProductsController( IGenericRepository<Product> product,
			IGenericRepository<ProductBrand> brands,
			IGenericRepository<ProductCategory> categories
			, IMapper mapper )
		{
			_product = product;
			this.brands = brands;
			_categories = categories;
			_mapper = mapper;
		}
		//GetAll
		//[Authorize(AuthenticationSchemes = "Bearer")]
		[HttpGet]
		public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productSpec )
		{

			#region explains
			//var products =await _product.GetAllAsync();
			//ObjectResult result = new ObjectResult(products);
			//result.ContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection();
			//result.StatusCode = 200;
			//var spec =new  BaseSpecifications<Product>(); 
			#endregion
			if (!Request.Headers.ContainsKey("ApiToken"))
			{
				return Unauthorized(new ApiResponse(401, "ApiToken header is missing"));
			}

			// Get the token from the Authorization header
			

			var spec =new ProductWithBrandsAndCategorySpecs(productSpec);

			var products = await _product.GetAllWithSpecAsync(spec);

			var countSpec = new ProductWithFilterationsForCountSpecfications(productSpec);
			var count = await _product.GetCountAsync(countSpec);

			var res = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			return Ok( new Paginations<ProductToReturnDto>(productSpec.pagesize,productSpec.pageIndex,res.Count, count, res) );
		 }

		//GetByID
		[ProducesResponseType( typeof(ProductToReturnDto), StatusCodes.Status200OK )]
		[ProducesResponseType( typeof(ApiResponse), StatusCodes.Status404NotFound )]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
		{
			var spec = new ProductWithBrandsAndCategorySpecs(id);

			var product = await _product.GetWithSpecAsync(spec);
			if ( product == null )
			{
				return NotFound(new ApiResponse(404));
			}else
			{
				var res = _mapper.Map<Product, ProductToReturnDto>(product);
				return Ok( res );
			}
		
		}

		[HttpGet("brands")]
		public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
		{

			var Brands = await brands.GetAllAsync();
			return Ok(Brands);
		}
		[ProducesResponseType(typeof(ProductBrand), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("brands/{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetBrandById(int id)
		{
			

			var brand = await brands.GetAsync(id);
			if (brand == null)
			{
				return NotFound(new ApiResponse(404));
			}
			else
			{
				return Ok(brand);
			}

		}


		[HttpGet("categories")]
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{

			var categories = await _categories.GetAllAsync();
			return Ok(categories);
		}
		[ProducesResponseType(typeof(ProductCategory), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("categories/{id}")]
		public async Task<ActionResult<ProductCategory>> GetCategoriesId(int id)
		{


			var category = await _categories.GetAsync(id);
			if (category == null)
			{
				return NotFound(new ApiResponse(404));
			}
			else
			{
				return Ok(category);
			}

		}


	}
}
