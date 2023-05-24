using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("Stock")]

	public class StockController
	{
		GlobalMetods metods = new GlobalMetods();

		[HttpGet]
		[Route("GetStockProductos")]
		public dynamic GetStockProductos()
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay ningun stock registrados"
				};
			}

			List<Stock> liststock = new List<Stock>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");
				Stock stock = new Stock();

				stock.Id_Producto = Convert.ToInt32(splitArr[0]);
				stock.CantidadStock = Convert.ToInt32(splitArr[1]);

				liststock.Add(stock);
			}

			return liststock;
		}

		[HttpGet]
		[Route("GetStockProducto")]
		public dynamic GetStockProductos(int id_producto)
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay ningun stock registrados"
				};
			}

			Stock stock = new Stock();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					stock.Id_Producto = Convert.ToInt32(splitArr[0]);
					stock.CantidadStock = Convert.ToInt32(splitArr[1]);

					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "No hay registros de stock para el producto '" + id_producto + "'"
				};
			}

			return stock;
		}

		[HttpPost]
		[Route("CrearStock")]
		public dynamic CrearStock(int id_producto, int cantidad)
		{
			string[] listProductos = metods.getContentFile("Productos");
			bool prodEncontrada = false;

			for (int i = 0; i < listProductos.Count(); i++)
			{
				string[] splitArr = listProductos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				return new
				{
					message = "El producto '" + id_producto + "' no existe en los registros",
				};
			}

			string[] listStock = metods.getContentFile("Stock");
			bool encontrado = false;

			for (int i = 0; i < listStock.Count(); i++)
			{
				string[] splitArr = listStock[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					encontrado = true;
				}
			}

			if (encontrado)
			{
				return new
				{
					message = "El producto '" + id_producto + "' ya tiene un stock en los registros",
				};
			}

			if (cantidad <= 0)
			{
				return new
				{
					message = "La cantidad de stock no puede ser igual o menor a 0",
				};
			}

			Stock stock = new Stock();
			stock.Id_Producto = id_producto;
			stock.CantidadStock = cantidad;

			metods.saveLineFile("Stock", String.Format("{0}||{1}", id_producto, cantidad));

			return new
			{
				message = "Stock registrado",
				result = stock
			};
		}

		[HttpPut]
		[Route("AumentarcarStock")]
		public dynamic AumentarcarStock(int id_producto, int cantidad)
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay ningun stock registrados"
				};
			}

			string[] listProductos = metods.getContentFile("Productos");
			bool prodEncontrada = false;

			for (int i = 0; i < listProductos.Count(); i++)
			{
				string[] splitArr = listProductos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				return new
				{
					message = "El producto '" + id_producto + "' no existe en los registros",
				};
			}

			if (cantidad <= 0)
			{
				return new
				{
					message = "La cantidad de stock a aumentar no puede ser igual o menor a 0",
				};
			}

			List<string> content = new List<string>();
			Stock stock = new Stock();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					content.Add(String.Format("{0}||{1}", id_producto, cantidad + Convert.ToInt32(splitArr[i])));
					encontrado = true;

					stock.Id_Producto = id_producto;
					stock.CantidadStock = cantidad + Convert.ToInt32(splitArr[i]);

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "No hay registros de stock para el producto '" + stock.Id_Producto + "'"
				};
			}

			metods.updateLineFile("Stock", content);

			return new
			{
				mesage = "Stock aumentado",
				result = stock
			};
		}

		[HttpPut]
		[Route("RebajarStock")]
		public dynamic RebajarStock(int id_producto, int cantidad)
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay ningun stock registrados"
				};
			}

			string[] listProductos = metods.getContentFile("Productos");
			bool prodEncontrada = false;

			for (int i = 0; i < listProductos.Count(); i++)
			{
				string[] splitArr = listProductos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				return new
				{
					message = "El producto '" + id_producto + "' no existe en los registros",
				};
			}

			if (cantidad <= 0)
			{
				return new
				{
					message = "La cantidad de stock a rebajar no puede ser igual o menor a 0",
				};
			}

			List<string> content = new List<string>();
			Stock stock = new Stock();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					
					if (cantidad > Convert.ToInt32(splitArr[1]))
					{
						return new
						{
							message = "La cantidad de stock a rebajar no puede ser mayor que la cantidad actual.",
						};
					}

					stock.Id_Producto = id_producto;
					stock.CantidadStock = Convert.ToInt32(splitArr[1]) - cantidad;

					content.Add(String.Format("{0}||{1}", stock.Id_Producto, stock.CantidadStock));
					encontrado = true;
					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "No hay registros de stock para el producto '" + stock.Id_Producto + "'"
				};
			}

			metods.updateLineFile("Stock", content);

			return new
			{
				mesage = "Stock rebajado",
				result = stock
			};
		}

		[HttpDelete]
		[Route("EliminarStock")]
		public dynamic EliminarStock(int id_producto)
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay ningun stock registrados"
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) != id_producto)
				{
					content.Add(list[i]);
				}
				else
				{
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "No hay registros de stock para el producto '" + id_producto + "'"
				};
			}

			metods.updateLineFile("Stock", content);

			return new
			{
				mesage = "La categoria '" + id_producto + "' fue eliminado exitosamente"
			};
		}
	}

}
