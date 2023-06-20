using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("Producto")]
	public class ProductoController
	{
		GlobalMetods metods = new GlobalMetods();
		TransaccionResult result = new TransaccionResult();

		[HttpGet]
		[Route("GetProductos")]
		public dynamic GetProductos()
		{
			string[] list = metods.getContentFile("Productos");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay productos registradas";

				return result;
			}

			List<Producto> productos = new List<Producto>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");
				Producto producto = new Producto();

				producto.Id_Producto = Convert.ToInt32(splitArr[0]);
				producto.Nombre = splitArr[1];
				producto.Descripcion = splitArr[2];
				producto.SerieProducto = splitArr[3];
				producto.Marca = splitArr[4];
				producto.Categoria_id = Convert.ToInt32(splitArr[5]);
				producto.Precio = Convert.ToInt32(splitArr[6]);
				producto.FechaCreacion = splitArr[7];
				producto.FechaModificacion = splitArr[8];
				producto.Estado = Convert.ToBoolean(splitArr[9]);

				productos.Add(producto);
			}

			return productos;
		}

		[HttpGet]
		[Route("GetProducto")]
		public dynamic GetProducto(int id_producto)
		{
			string[] list = metods.getContentFile("Productos");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay productos registradas";

				return result;
			}

			Producto producto = new Producto();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_producto)
				{
					producto.Id_Producto = Convert.ToInt32(splitArr[0]);
					producto.Nombre = splitArr[1];
					producto.Descripcion = splitArr[2];
					producto.SerieProducto = splitArr[3];
					producto.Marca = splitArr[4];
					producto.Categoria_id = Convert.ToInt32(splitArr[5]);
					producto.Precio = Convert.ToInt32(splitArr[6]);
					producto.FechaCreacion = splitArr[7];
					producto.FechaModificacion = splitArr[8];
					producto.Estado = Convert.ToBoolean(splitArr[9]);

					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "El producto '" + id_producto + "' no existe en los registros";

				return result;
			}

			return producto;
		}

		[HttpPost]
		[Route("CrearProducto")]
		public dynamic CrearProducto(Producto producto)
		{
			if (producto.Precio == 0)
			{
				result.resultTransaccion = false;
				result.message = "El precio no puede ser '0'";

				return result;
			}

            if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrEmpty(producto.SerieProducto) || string.IsNullOrEmpty(producto.Marca) || string.IsNullOrEmpty(producto.Marca))
			{
				result.resultTransaccion = false;
				result.message = "Faltan datos para almacenar el producto";

				return result;
			}

			string[] listCategorias = metods.getContentFile("CategoriaProductos");
			bool catEncontrada = false;

			for (int i = 0; i < listCategorias.Count(); i++)
			{
				string[] splitArr = listCategorias[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == producto.Categoria_id)
				{
					catEncontrada = true;
				}
			}

			if (!catEncontrada)
			{
				result.resultTransaccion = false;
				result.message = "La categoria '" + producto.Categoria_id + "' no existe en los registros";

				return result;
			}

			string[] list = metods.getContentFile("Productos");

			if (list.Count() == 1)
			{
				string[] splitArr = list[0].Split("||");
				producto.Id_Producto = Convert.ToInt32(splitArr[0]) + 1;
			}
			else
			{
				producto.Id_Producto = list.Count() != 0 ? (Convert.ToInt32(list[list.Count() - 1].Split("||")[0])) + 1 : 1;
			}

			producto.Estado = true;
			producto.FechaCreacion = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
			producto.FechaModificacion = "00-00-0000 00:00:00";

			metods.saveLineFile("Productos", String.Format("{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}||{8}||{9}", producto.Id_Producto, producto.Nombre.Trim().Replace("|", ""), producto.Descripcion.Trim().Replace("|", ""), producto.SerieProducto.Trim().Replace("|", ""), producto.Marca.Trim().Replace("|", ""), producto.Categoria_id, producto.Precio, producto.FechaCreacion.Trim().Replace("|", ""), producto.FechaModificacion.Trim().Replace("|", ""),producto.Estado));

			result.resultTransaccion = true;
			result.message = "El producto " + producto.Nombre + " fue registrado exitosamente";

			return result;
		}

		[HttpPut]
		[Route("ModificarProducto")]
		public dynamic ModificarProducto(Producto producto)
		{
            string[] listCategorias = metods.getContentFile("CategoriaProductos");
			bool catEncontrada = false;

			if (producto.Categoria_id != 0)
			{
				for (int i = 0; i < listCategorias.Count(); i++)
				{
					string[] splitArr = listCategorias[i].Split("||");

					if (Convert.ToInt32(splitArr[0]) == producto.Categoria_id)
					{
						catEncontrada = true;
					}
				}

				if (!catEncontrada)
				{
					result.resultTransaccion = false;
					result.message = "La categoria '" + producto.Id_Producto + "' no existe en los registros";

					return result;
				}
			}

			string[] list = metods.getContentFile("Productos");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay productos registrados";

				return result;
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			string nombreProd = "";

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == producto.Id_Producto)
				{
					producto.Id_Producto = producto.Id_Producto;
					producto.FechaCreacion = splitArr[7];
					producto.FechaModificacion = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

					nombreProd = string.IsNullOrEmpty(producto.Nombre) ? splitArr[1] : producto.Nombre.Trim().Replace("|", "");

					content.Add(String.Format("{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}||{8}||{9}", producto.Id_Producto, string.IsNullOrEmpty(producto.Nombre) ? splitArr[1] : producto.Nombre.Trim().Replace("|", ""), string.IsNullOrEmpty(producto.Descripcion) ? splitArr[2] : producto.Descripcion.Trim().Replace("|", ""), string.IsNullOrEmpty(producto.SerieProducto) ? splitArr[3] : producto.SerieProducto.Trim().Replace("|", ""), string.IsNullOrEmpty(producto.Marca) ? splitArr[4] : producto.Marca.Trim().Replace("|", ""), producto.Categoria_id == 0 ? splitArr[5] : producto.Categoria_id, producto.Precio == 0 ? splitArr[6] : producto.Precio, producto.FechaCreacion.Trim().Replace("|", ""), producto.FechaModificacion.Trim().Replace("|", ""), producto.Estado));

					encontrado = true;

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "El producto '" + producto.Id_Producto + "' no existe en los registros";

				return result;
			}

			metods.updateLineFile("Productos", content);

			result.resultTransaccion = true;
			result.message = "El producto " + nombreProd + " fue modificado exitosamente";

			return result;
		}

		[HttpDelete]
		[Route("EliminarProducto")]
		public dynamic EliminarProducto(int id_producto)
		{
			string[] list = metods.getContentFile("Productos");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay productos registrados";

				return result;
			}

			bool encontrado = false;
			List<string> content = new List<string>();
			string nomnreProducto = "";

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) != id_producto)
				{
					content.Add(list[i]);
				}
				else
				{
					nomnreProducto = splitArr[1];
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "El usaurio '" + id_producto + "' no existe en los registros";

				return result;
			}

			metods.updateLineFile("Productos", content);

			result.resultTransaccion = false;
			result.message = "El usaurio '" + nomnreProducto + "' fue eliminado exitosamente";

			return result;
		}
	}
}
