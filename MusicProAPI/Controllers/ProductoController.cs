using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("productos")]
	public class ProductoController
	{
		GlobalMetods metods = new GlobalMetods();

		[HttpGet]
		[Route("GetProductos")]
		public dynamic GetProductos()
		{
			string[] list = metods.getContentFile("Productos");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay productos registrados"
				};
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
				return new
				{
					mesage = "No hay productos registrados"
				};
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
				return new
				{
					mesage = "El producto '" + id_producto + "' no existe en los registros"
				};
			}

			return producto;
		}

		[HttpPost]
		[Route("CrearProducto")]
		public dynamic CrearProducto(string nombre, string descripcion, string serieProducto, string marca, int categoriaId, int precio)
		{
            Producto producto = new Producto();
			producto.Nombre = nombre;
			producto.Descripcion = descripcion;
			producto.SerieProducto = serieProducto;
			producto.Marca = marca;
			producto.Categoria_id = categoriaId;
			producto.Precio = precio;

			if (precio == 0)
			{
                return new
                {
                    message = "El precio no puede ser '0'",
                };
            }

            if (string.IsNullOrEmpty(producto.Nombre) || string.IsNullOrEmpty(producto.Descripcion) || string.IsNullOrEmpty(producto.SerieProducto) || string.IsNullOrEmpty(producto.Marca) || string.IsNullOrEmpty(producto.Marca))
			{
				return new
				{
					message = "Faltan datos para almacenar el producto",
				};
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
				return new
				{
					message = "La categoria '" + producto.Id_Producto + "' no existe en los registros",
				};
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

			return new
			{
				message = "Producto registrado",
				result = producto
			};
		}

		[HttpPut]
		[Route("ModificarProducto")]
		public dynamic ModificarProducto(Producto producto)
		{
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
				return new
				{
					message = "La categoria '" + producto.Id_Producto + "' no existe en los registros",
				};
			}

			string[] list = metods.getContentFile("Productos");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay productos registrados"
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == producto.Id_Producto)
				{
					producto.Id_Producto = producto.Id_Producto;
					producto.FechaCreacion = splitArr[7];
					producto.FechaModificacion = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

					content.Add(String.Format("{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}||{8}||{9}", producto.Id_Producto, string.IsNullOrEmpty(producto.Nombre) ? splitArr[1] : producto.Nombre.Trim().Replace("|", ""), string.IsNullOrEmpty(producto.Descripcion) ? splitArr[2] : producto.Descripcion.Trim().Replace("|", ""), string.IsNullOrEmpty(producto.SerieProducto) ? splitArr[3] : producto.SerieProducto.Trim().Replace("|", ""), string.IsNullOrEmpty(producto.Marca) ? splitArr[4] : producto.Marca.Trim().Replace("|", ""), producto.Categoria_id == 0 ? splitArr[5] : producto.Categoria_id, producto.Precio == 0 ? splitArr[6] : producto.Precio, producto.FechaCreacion.Trim().Replace("|", ""), producto.FechaModificacion.Trim().Replace("|", ""), producto.Estado));

					encontrado = true;

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El producto '" + producto.Id_Producto + "' no existe en los registros"
				};
			}

			metods.updateLineFile("Productos", content);

			return new
			{
				mesage = "Producto modificado"
			};
		}

		[HttpDelete]
		[Route("EliminarProducto")]
		public dynamic EliminarProducto(int id_producto)
		{
			string[] list = metods.getContentFile("Productos");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay productos registrados"
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
					mesage = "El usaurio '" + id_producto + "' no existe en los registros"
				};
			}

			metods.updateLineFile("Productos", content);

			return new
			{
				mesage = "El usaurio '" + id_producto + "' fue eliminado exitosamente"
			};
		}
	}
}
