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

			if (valid == null)
			{
				//en caso de que no hayan categorias registradas

				var valid2 = Assert.IsType<TransaccionResult>(resut);

				Assert.True(valid2 != null);
			}
			else
			{
				//caso de exito si existen categorias

				Assert.True(valid != null);
			}
		}

		[Fact]
		public void GetCategoria_Test()
		{
			//prueba en caso de que un id no exista

			int id = 17894161;

			var resut = CategoriaProductoController.GetCategoria(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			//caso de exito en caso de que la categoria exista 

			id = categoria.Id_Categoria;

			resut = CategoriaProductoController.GetCategoria(id);

			valid = Assert.IsType<CategoriaProducto>(resut);

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
			//prueba en caso de mandarle datos en vacio al endpoint

			CategoriaProducto updatecategoria = categoria;

			updatecategoria.Nombre = "";
			updatecategoria.Descripcion = "";

			var resut = CategoriaProductoController.CrearProducto(updatecategoria);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de exito en la creacion de la categoria

			resut = CategoriaProductoController.CrearProducto(categoria);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void ModificarCategoria_Test()
		{
			CategoriaProducto updatecategoria = new CategoriaProducto();
			updatecategoria.Id_Categoria = categoria.Id_Categoria;
			updatecategoria.Nombre = "CambioNameCategoriaTest";
			updatecategoria.Descripcion = "CambioDescripcionCategoriaTest";


			//prueba en caso de que el id ingresado no exista

			updatecategoria.Id_Categoria = 1645189;

			var resut = CategoriaProductoController.ModificarCategoria(updatecategoria);

			var valid = Assert.IsType<TransaccionResult>(resut);

			//prueba en caso de exito que se modifique correctamente la categoria

			updatecategoria.Id_Categoria = categoria.Id_Categoria;

			resut = CategoriaProductoController.ModificarCategoria(updatecategoria);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}
	}
}
