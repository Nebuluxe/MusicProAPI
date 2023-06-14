using MusicProAPI.Controllers;
using MusicProAPI.Modelos;
using MusicProAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MusicProTestApi
{
	public class CategoriaProductoTest
	{
		private readonly CategoriaProductoController CategoriaProductoController;

		CategoriaProducto categoria = new CategoriaProducto();
		public CategoriaProductoTest()
		{
			CategoriaProductoController = new CategoriaProductoController();

			PrepareTestEscenary();
		}

		private void PrepareTestEscenary()
		{
			GlobalMetods metods = new GlobalMetods();

			categoria.Id_Categoria = 999999999;
			categoria.Nombre = "NameCategoriaTest";
			categoria.Descripcion = "DescripcionCategoriaTest";

			string[] list = metods.getContentFile("CategoriaProductos");

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt64(splitArr[0]) == categoria.Id_Categoria)
				{
					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				metods.saveLineFile("CategoriaProductos", String.Format("{0}||{1}||{2}", categoria.Id_Categoria, categoria.Nombre.Trim().Replace("|", ""), categoria.Descripcion.Trim().Replace("|", "")));
			}
		}

		[Fact]
		public void GetCategorias_Test()
		{
			var resut = CategoriaProductoController.GetCategorias();

			var valid = Assert.IsType<List<CategoriaProducto>>(resut);

			Assert.True(valid != null);

			List<CategoriaProducto> cates = valid;
		}

		[Fact]
		public void GetCategoria_Test()
		{
			int id = categoria.Id_Categoria;

			var resut = CategoriaProductoController.GetCategoria(id);

			var valid = Assert.IsType<CategoriaProducto>(resut);

			Assert.True(valid != null);

			CategoriaProducto cate = valid;

			Assert.Equal(cate?.Id_Categoria, id);
		}

		[Fact]
		public void EliminarCategoria_Test()
		{
			int id = categoria.Id_Categoria;

			var resut = CategoriaProductoController.EliminarCategoria(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void CrearCategoria_Test()
		{
			var resut = CategoriaProductoController.CrearProducto(categoria);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void ModificarCategoria_Test()
		{
			CategoriaProducto updatecategoria = new CategoriaProducto();
			updatecategoria.Id_Categoria = categoria.Id_Categoria;
			updatecategoria.Nombre = "CambioNameCategoriaTest";
			updatecategoria.Descripcion = "CambioDescripcionCategoriaTest";

			var resut = CategoriaProductoController.ModificarCategoria(updatecategoria);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}
	}
}
