using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("CategoriaProductos")]

	public class CategoriaProductoController
	{
		GlobalMetods metods = new GlobalMetods();

		[HttpGet]
		[Route("GetCategorias")]
		public dynamic GetCategorias()
		{
			string[] list = metods.getContentFile("CategoriaProductos");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay categorias registradas"
				};
			}

			List<CategoriaProducto> categorias = new List<CategoriaProducto>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");
				CategoriaProducto categoria = new CategoriaProducto();

				categoria.Id_Categoria = Convert.ToInt32(splitArr[0]);
				categoria.Nombre = splitArr[1];
				categoria.Descripcion = splitArr[2];

				categorias.Add(categoria);
			}

			return categorias;
		}

		[HttpGet]
		[Route("GetCategoria")]
		public dynamic GetCategoria(int id_categoria)
		{
			string[] list = metods.getContentFile("CategoriaProductos");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay categorias registradas"
				};
			}

			CategoriaProducto categoria = new CategoriaProducto();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_categoria)
				{
					categoria.Id_Categoria = Convert.ToInt32(splitArr[0]);
					categoria.Nombre = splitArr[1];
					categoria.Descripcion = splitArr[2];

					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "La categoria '" + id_categoria + "' no existe en los registros"
				};
			}

			return categoria;
		}

		[HttpPost]
		[Route("CrearCategoria")]
		public dynamic CrearProducto(string nombre, string descripcion)
		{
			if (string.IsNullOrEmpty(descripcion) || string.IsNullOrEmpty(nombre))
			{
				return new
				{
					message = "Faltan datos para almacenar la categoria",
				};
			}

			string[] list = metods.getContentFile("CategoriaProductos");

			int categoriaID = 0;

			if (list.Count() == 1)
			{
				string[] splitArr = list[0].Split("||");
				categoriaID = Convert.ToInt32(splitArr[0]) + 1;
			}
			else
			{
				categoriaID = list.Count() != 0 ? (Convert.ToInt32(list[list.Count() - 1].Split("||")[0])) + 1 : 1;
			}

			CategoriaProducto categoria = new CategoriaProducto();
			categoria.Id_Categoria = categoriaID;
			categoria.Nombre = nombre;
			categoria.Descripcion = descripcion;

			metods.saveLineFile("CategoriaProductos", String.Format("{0}||{1}||{2}", categoria.Id_Categoria, categoria.Nombre.Trim().Replace("|",""), categoria.Descripcion.Trim().Replace("|", "")));

			return new
			{
				message = "Categoria registrado",
				result = categoria
			};
		}

		[HttpPut]
		[Route("ModificarCategoria")]
		public dynamic ModificarProducto(int id_categoria, string nombre, string descripcion)
		{
			if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(descripcion))
			{
				return new
				{
					message = "Faltan datos para almacenar la categoria",
				};
			}

			string[] list = metods.getContentFile("CategoriaProductos");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay categorias registradas"
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();
			CategoriaProducto categoria = new CategoriaProducto();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_categoria)
				{

					content.Add(String.Format("{0}||{1}||{2}", id_categoria, string.IsNullOrEmpty(nombre) ? splitArr[1] : nombre.Trim().Replace("|", ""), string.IsNullOrEmpty(descripcion) ? splitArr[2] : descripcion.Trim().Replace("|", "")));

					encontrado = true;

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "La categoria '" + id_categoria + "' no existe en los registros"
				};
			}

			metods.updateLineFile("CategoriaProductos", content);

			return new
			{
				mesage = "Categoria modificada",
				result = GetCategoria(categoria.Id_Categoria)
			};
		}

		[HttpDelete]
		[Route("EliminarCategoria")]
		public dynamic EliminarCategoria(int id_categoria)
		{
			string[] list = metods.getContentFile("CategoriaProductos");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay categorias registrados"
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) != id_categoria)
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
					mesage = "La categoria '" + id_categoria + "' no existe en los registros"
				};
			}

			metods.updateLineFile("CategoriaProductos", content);

			return new
			{
				mesage = "La categoria '" + id_categoria + "' fue eliminado exitosamente"
			};
		}
	}
}
