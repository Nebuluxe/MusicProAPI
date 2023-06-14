using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("CategoriaProductos")]

	public class CategoriaProductoController
	{
		GlobalMetods metods = new GlobalMetods();
		TransaccionResult result = new TransaccionResult();

		[HttpGet]
		[Route("GetCategorias")]
		public dynamic GetCategorias()
		{
			string[] list = metods.getContentFile("CategoriaProductos");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay categorias registradas";

				return result;
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
				result.resultTransaccion = false;
				result.message = "No hay categorias registradas";

				return result;
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
				result.resultTransaccion = false;
				result.message = "La categoria '" + id_categoria + "' no existe en los registros";

				return result;
			}

			return categoria;
		}

		[HttpPost]
		[Route("CrearCategoria")]
		public dynamic CrearProducto(CategoriaProducto categoria)
		{
			if (string.IsNullOrEmpty(categoria.Nombre) && string.IsNullOrEmpty(categoria.Descripcion))
			{
				result.resultTransaccion = false;
				result.message = "Faltan datos para almacenar la categoria";

				return result;
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

			categoria.Id_Categoria = categoriaID;

			metods.saveLineFile("CategoriaProductos", String.Format("{0}||{1}||{2}", categoria.Id_Categoria, categoria.Nombre.Trim().Replace("|",""), categoria.Descripcion.Trim().Replace("|", "")));

			result.resultTransaccion = true;
			result.message = "La categoria " + categoria.Nombre + " fue resgistrada exitosamente";

			return result;

		}

		[HttpPut]
		[Route("ModificarCategoria")]
		public dynamic ModificarCategoria(CategoriaProducto categoria)
		{
			string[] list = metods.getContentFile("CategoriaProductos");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay categorias registradas";

				return result;
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			string nombreCategoria = "";

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == categoria.Id_Categoria)
				{

					nombreCategoria = string.IsNullOrEmpty(categoria.Nombre) ? splitArr[1] : categoria.Nombre.Trim().Replace("|", "");

					content.Add(String.Format("{0}||{1}||{2}", categoria.Id_Categoria, string.IsNullOrEmpty(categoria.Nombre) ? splitArr[1] : categoria.Nombre.Trim().Replace("|", ""), string.IsNullOrEmpty(categoria.Descripcion) ? splitArr[2] : categoria.Descripcion.Trim().Replace("|", "")));

					encontrado = true;

					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "La categoria '" + nombreCategoria + "' no existe en los registros";

				return result;
			}

			metods.updateLineFile("CategoriaProductos", content);

			result.resultTransaccion = true;
			result.message = "La categoria " + nombreCategoria + " fue modificada exitosamente";

			return result;
		}

		[HttpDelete]
		[Route("EliminarCategoria")]
		public dynamic EliminarCategoria(int id_categoria)
		{
			string[] list = metods.getContentFile("CategoriaProductos");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay categorias registrados";

				return result;
			}

			bool encontrado = false;
			List<string> content = new List<string>();
			string nombreCategoria = "";

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) != id_categoria)
				{
					content.Add(list[i]);
				}
				else
				{
					nombreCategoria = splitArr[1];
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "La categoria '" + id_categoria + "' no existe en los registros";

				return result;
			}

			metods.updateLineFile("CategoriaProductos", content);

			result.resultTransaccion = false;
			result.message = "La categoria " + nombreCategoria + " fue eliminada exitosamente";

			return result;
		}
	}
}
