using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("productos")]
	public class ProductoController : ControllerBase
	{
		public string directorio = "C:\\txtMusicPro";

		[HttpGet]
		[Route("GetProductos")]
		public dynamic GetProductos()
		{
			VerificarDirectorio("Productos.txt");

			string[] list = System.IO.File.ReadAllLines(directorio + "\\Productos.txt");

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

				producto.Id = Convert.ToInt32(splitArr[0]);
				producto.Nombre = splitArr[1];
				producto.Descripcion = splitArr[2];
				producto.SerieProducto = splitArr[3];
				producto.Marca = splitArr[4];
				producto.Categoria_id = Convert.ToInt32(splitArr[5]);
				producto.Precio = Convert.ToInt32(splitArr[6]);
				producto.fechaCreacion = splitArr[7];
				producto.fechaModificacion = splitArr[8];

				productos.Add(producto);
			}

			return productos;
		}

		[HttpGet]
		[Route("GetProducto")]
		public dynamic GetProducto(int id)
		{
			VerificarDirectorio("Productos.txt");

			string[] list = System.IO.File.ReadAllLines(directorio + "\\Productos.txt");

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

				if (Convert.ToInt32(splitArr[0]) == id)
				{
					producto.Id = Convert.ToInt32(splitArr[0]);
					producto.Nombre = splitArr[1];
					producto.Descripcion = splitArr[2];
					producto.SerieProducto = splitArr[3];
					producto.Marca = splitArr[4];
					producto.Categoria_id = Convert.ToInt32(splitArr[5]);
					producto.Precio = Convert.ToInt32(splitArr[6]);
					producto.fechaCreacion = splitArr[7];
					producto.fechaModificacion = splitArr[8];

					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El producto '" + id + "' no existe en los registros"
				};
			}

			return producto;
		}

		[HttpPost]
		[Route("CrearProducto")]
		public dynamic CrearProducto(Producto producto)
		{
			VerificarDirectorio("Productos.txt");

			string[] list = System.IO.File.ReadAllLines(directorio + "\\Productos.txt");

			if (list.Count() == 1)
			{
				string[] splitArr = list[0].Split("||");
				producto.Id = Convert.ToInt32(splitArr[0]) + 1;
			}
			else
			{
				producto.Id = list.Count() != 0 ? (Convert.ToInt32(list[list.Count() - 1].Split("||")[0])) + 1 : 1;
			}

			producto.fechaCreacion = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

			producto.fechaModificacion = "00-00-0000 00:00:00";

			StreamWriter sw = System.IO.File.AppendText(directorio + "\\Productos.txt");

			sw.WriteLine(producto.Id + "||" + producto.Nombre.Trim().Replace("|", "") + "||" + producto.Descripcion.Trim().Replace("|", "") + "||" + producto.SerieProducto.Trim().Replace("|", "") + "||" +
						 producto.Marca.Trim().Replace("|", "") + "||" + producto.Categoria_id + "||" + producto.Precio + "||" + producto.fechaCreacion.Trim().Replace("|", "") + "||" + producto.fechaModificacion.Trim().Replace("|", ""));
			sw.Close();

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
			VerificarDirectorio("Productos.txt");

			string[] list = System.IO.File.ReadAllLines(directorio + "\\Productos.txt");

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

				if (Convert.ToInt32(splitArr[0]) == producto.Id)
				{
					content.Add(Convert.ToInt32(splitArr[0]) + "||" + producto.Nombre.Trim().Replace("|", "") + "||" + producto.Descripcion.Trim().Replace("|", "") + "||" + producto.SerieProducto.Trim().Replace("|", "") + "||" +
								 producto.Marca.Trim().Replace("|", "") + "||" + producto.Categoria_id + "||" + producto.Precio + "||" + splitArr[6] + "||" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
					encontrado = true;

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El usaurio '" + producto.Id + "' no existe en los registros"
				};
			}

			System.IO.File.WriteAllLines(directorio + "\\Productos.txt", content);

			return new
			{
				mesage = "Producto modificado",
				result = producto
			};
		}

		[HttpDelete]
		[Route("EliminarProducto")]
		public dynamic EliminarProducto(int id)
		{
			VerificarDirectorio("Productos.txt");

			string[] list = System.IO.File.ReadAllLines(directorio + "\\Productos.txt");

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

				if (Convert.ToInt32(splitArr[0]) != id)
				{
					content.Add(list[i]);
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El usaurio '" + id + "' no existe en los registros"
				};
			}

			System.IO.File.WriteAllLines(directorio + "\\Productos.txt", content);

			return new
			{
				mesage = "El usaurio '" + id + "' fue eliminado exitosamente"
			};
		}
		private void VerificarDirectorio(string NomDoc)
		{
			if (!Directory.Exists(directorio))
			{
				Directory.CreateDirectory(directorio);
			}

			if (!System.IO.File.Exists(directorio + "\\" + NomDoc))
			{
				System.IO.File.Create(directorio + "\\" + NomDoc).Close();
			}
		}

	}
}
