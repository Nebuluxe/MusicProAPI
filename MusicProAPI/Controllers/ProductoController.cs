using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProductoController : ControllerBase
	{
		private static string[] Productos = new string[]
		{
			"Guitarra Electrica","Bajo","Trompeta","Tambor","Trombo","guitarra"
		};

		private readonly ILogger<ProductoController> _logger;

		public ProductoController(ILogger<ProductoController> logger)
		{
			_logger = logger;
		}

		[HttpGet(Name = "GetProductos")]

		public IEnumerable<Producto> Get()
		{
			return Enumerable.Range(1, 20).Select(index => new Producto
			{
				Id = index,
				Nommbre = Productos[Random.Shared.Next(Productos.Length)],
				Descripcion = "Lorem ipsum dolor sit amet, consectetur adipiscing",
				Image = "Image Default",
				ImageURL = "URL Image Default"
			}).ToArray();
		}

	}
}
