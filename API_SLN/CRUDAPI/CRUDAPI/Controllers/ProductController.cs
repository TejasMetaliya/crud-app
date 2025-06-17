using CRUDAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUDAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ProductController : Controller
	{
		private readonly ProductRepository _productRepository;

		public ProductController(ProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		[HttpGet]
		[Route("exportData")]
		public async Task<IActionResult> GetAll()
		{
			var products = await _productRepository.GetAllProducts();
			return Ok(products);
		}
	}
}
