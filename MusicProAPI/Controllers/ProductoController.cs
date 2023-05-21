using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("productos")]
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

		[HttpGet("listaProductos")]
		public List<Producto> Get()
		{
			List<Producto> ListProduct = new List<Producto>();

			for (int i = 0; i < Productos.Count(); i++)
			{
				var obj = new Producto();

				obj.Id = i + 1;
				obj.Nommbre = Productos[i];
				obj.Descripcion = "Lorem ipsum dolor sit amet, consectetur adipiscing";
				obj.Image = "Image Default";
				obj.ImageURL = "URL Image Default";

				ListProduct.Add(obj);
			}

			return ListProduct;
		}

	}
}
