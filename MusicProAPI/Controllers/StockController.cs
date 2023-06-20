using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("Stock")]

	public class StockController
	{
		GlobalMetods metods = new GlobalMetods();
		TransaccionResult result = new TransaccionResult();

		[HttpGet]
		[Route("GetStockProductos")]
		public dynamic GetStockProductos()
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay ningun stock registradas";

				return result;
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
		public dynamic GetStockProducto(int id_producto)
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay ningun stock registradas";

				return result;
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
				result.resultTransaccion = false;
				result.message = "No hay registros de stock para el producto '" + id_producto + "'";

				return result;
			}

			return stock;
		}

		[HttpPost]
		[Route("CrearStock")]
		public dynamic CrearStock(int id_producto, int cantidad)
		{
			string[] listProductos = metods.getContentFile("Productos");
			bool prodEncontrada = false;
			string nombreProducto = "";

			for (int i = 0; i < listProductos.Count(); i++)
			{
				string[] splitArr = listProductos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					nombreProducto = splitArr[1];
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				result.resultTransaccion = false;
				result.message = "El producto '" + id_producto + "' no existe en los registros";

				return result;
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
				result.resultTransaccion = false;
				result.message = "El producto '" + id_producto + "' ya tiene un stock en los registros";

				return result;
			}

			if (cantidad <= 0)
			{
				result.resultTransaccion = false;
				result.message = "La cantidad de stock no puede ser igual o menor a 0";

				return result;
			}

			Stock stock = new Stock();
			stock.Id_Producto = id_producto;
			stock.CantidadStock = cantidad;

			metods.saveLineFile("Stock", String.Format("{0}||{1}", id_producto, cantidad));

			result.resultTransaccion = true;
			result.message = "Se ha creado stock para el producto " + nombreProducto + ", stock: " + cantidad;

			return result;
		}

		[HttpPut]
		[Route("AumentarStock")]
		public dynamic AumentarStock(int id_producto, int cantidad)
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay ningun stock registrados";

				return result;
			}

			string[] listProductos = metods.getContentFile("Productos");
			bool prodEncontrada = false;
			string nombreProd = "";

			for (int i = 0; i < listProductos.Count(); i++)
			{
				string[] splitArr = listProductos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					nombreProd = splitArr[1];
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				result.resultTransaccion = false;
				result.message = "El producto '" + id_producto + "' no existe en los registros";

				return result;
			}

			if (cantidad <= 0)
			{
				result.resultTransaccion = false;
				result.message = "La cantidad de stock a aumentar no puede ser igual o menor a 0";

				return result;
			}

			List<string> content = new List<string>();
			Stock stock = new Stock();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					content.Add(String.Format("{0}||{1}", id_producto, cantidad + Convert.ToInt32(splitArr[1])));
					encontrado = true;

					stock.Id_Producto = id_producto;
					stock.CantidadStock = cantidad + Convert.ToInt32(splitArr[1]);

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "No hay registros de stock para el producto '" + stock.Id_Producto + "'";

				return result;
			}

			metods.updateLineFile("Stock", content);

			result.resultTransaccion = true;
			result.message = "Se aumento el stock del producto " + nombreProd + ", stock: " + stock.CantidadStock;

			return result;
		}

		[HttpPut]
		[Route("RebajarStock")]
		public dynamic RebajarStock(int id_producto, int cantidad)
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay ningun stock registrados";

				return result;
			}

			string[] listProductos = metods.getContentFile("Productos");
			bool prodEncontrada = false;
			string nombreProd = "";

			for (int i = 0; i < listProductos.Count(); i++)
			{
				string[] splitArr = listProductos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					nombreProd = splitArr[1];
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				result.resultTransaccion = false;
				result.message = "El producto '" + id_producto + "' no existe en los registros";

				return result;
			}

			if (cantidad <= 0)
			{
				result.resultTransaccion = false;
				result.message = "La cantidad de stock a rebajar no puede ser igual o menor a 0";

				return result;
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
						result.resultTransaccion = false;
						result.message = "La cantidad de stock a rebajar no puede ser mayor que la cantidad actual.";

						return result;
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
				result.resultTransaccion = false;
				result.message = "No hay registros de stock para el producto '" + stock.Id_Producto + "'";

				return result;
			}

			metods.updateLineFile("Stock", content);

			result.resultTransaccion = true;
			result.message = "Se rebajo el stock del producto " + nombreProd + ", stock: " + stock.CantidadStock;

			return result;

		}

		[HttpDelete]
		[Route("EliminarStock")]
		public dynamic EliminarStock(int id_producto)
		{
			string[] list = metods.getContentFile("Stock");

			if (list.Count() == 0)
			{
				result.resultTransaccion = true;
				result.message = "No hay ningun stock registrados";

				return result;
			}

			string[] listProductos = metods.getContentFile("Productos");
			bool prodEncontrada = false;
			string nombreProd = "";

			for (int i = 0; i < listProductos.Count(); i++)
			{
				string[] splitArr = listProductos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					nombreProd = splitArr[1];
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				result.resultTransaccion = true;
				result.message = "El producto '" + id_producto + "' no existe en los registros";

				return result;
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
				result.resultTransaccion = true;
				result.message = "No hay registros de stock para el producto '" + nombreProd + "'";

				return result;
			}

			metods.updateLineFile("Stock", content);

			result.resultTransaccion = true;
			result.message = "El stock del producto " + nombreProd + " fue eliminado exitosamente";

			return result;
		}
	}

}
