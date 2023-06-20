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
	public class ProductosTest
	{
		private readonly ProductoController ProductoController;

		Producto producto = new Producto();
		public ProductosTest()
		{
			ProductoController = new ProductoController();

			PrepareTestEscenary();
		}

		private void PrepareTestEscenary()
		{
			GlobalMetods metods = new GlobalMetods();

			CategoriaProducto categoria = new CategoriaProducto();

			categoria.Id_Categoria = 999999999;
			categoria.Nombre = "NameCategoriaTest";
			categoria.Descripcion = "DescripcionCategoriaTest";

			string[] catelist = metods.getContentFile("CategoriaProductos");

			bool cateEncontrada = false;

			for (int i = 0; i < catelist.Count(); i++)
			{
				string[] splitArr = catelist[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == categoria.Id_Categoria)
				{
					cateEncontrada = true;
					break;
				}
			}

			if (!cateEncontrada)
			{
				metods.saveLineFile("CategoriaProductos", String.Format("{0}||{1}||{2}", categoria.Id_Categoria, categoria.Nombre.Trim().Replace("|", ""), categoria.Descripcion.Trim().Replace("|", "")));
			}

			producto.Id_Producto = 999999999;
			producto.Nombre = "NombreProductoTest";
			producto.Descripcion = "DescripcionProductoTest";
			producto.SerieProducto = "SA56456D54S";
			producto.Marca = "MarcaProductoTest";
			producto.Categoria_id = 999999999;
			producto.Precio = 999999;
			producto.FechaCreacion = "00-00-0000 00:00:00";
			producto.FechaModificacion = "00-00-0000 00:00:00";
			producto.Estado = true;

			string[] list = metods.getContentFile("Productos");

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == producto.Id_Producto)
				{
					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				metods.saveLineFile("Productos", String.Format("{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}||{8}||{9}", producto.Id_Producto, producto.Nombre.Trim().Replace("|", ""), producto.Descripcion.Trim().Replace("|", ""), producto.SerieProducto.Trim().Replace("|", ""), producto.Marca.Trim().Replace("|", ""), producto.Categoria_id, producto.Precio, producto.FechaCreacion.Trim().Replace("|", ""), producto.FechaModificacion.Trim().Replace("|", ""), producto.Estado));
			}
		}

		[Fact]
		public void GetProductos_Test()
		{
			var resut = ProductoController.GetProductos();

			var valid = Assert.IsType<List<Producto>>(resut);

			if (valid == null)
			{
				//prueba en caso de que no existan productos registrados

				var valid2 = Assert.IsType<TransaccionResult>(resut);

				Assert.True(valid2 != null);
			}
			else
			{
				//prueba en caso de exito y que existan productos registrados 

				Assert.True(valid != null);
			}
		}

		[Fact]
		public void GetProducto_Test()
		{
			//prueba en caso de que el id ingresado no exista en los registros

			int id = 519465;

			var resut = ProductoController.GetProducto(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de exito que el id exista 

			id = producto.Id_Producto;

			resut = ProductoController.GetProducto(id);

			valid = Assert.IsType<Producto>(resut);

			Assert.True(valid != null);

			Producto prod = valid;

			Assert.Equal(prod?.Id_Producto, id);
		}

		[Fact]
		public void EliminarProducto_Test()
		{
			//prueba en caso de que el id ingresado no existe dentro de los registros

			int id = 8489491;

			var resut = ProductoController.EliminarProducto(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			//pruea en caso de exito que el id exita

			id = producto.Id_Producto;

			resut = ProductoController.EliminarProducto(id);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void CrearProducto_Test()
		{
			Producto testproducto = producto;

			//prueba en caso de que el precio tenga un valor de 0

			testproducto.Precio = 0;

			var resut = ProductoController.CrearProducto(testproducto);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de que alguno de los datos vengan en vacio

			testproducto = producto;
			testproducto.Nombre = "";
			testproducto.Descripcion = "";
			testproducto.SerieProducto = "";

			resut = ProductoController.CrearProducto(testproducto);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de que el id de categoria asociado al producto no exista 

			testproducto = producto;

			resut = ProductoController.CrearProducto(testproducto);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//caso de exito de creacion de producto

			resut = ProductoController.CrearProducto(producto);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void ModificarProducto_Test()
		{
			Producto updateproducto = new Producto();
			updateproducto.Id_Producto = producto.Id_Producto;
			updateproducto.Nombre = "CambioNombreProductoTest";
			updateproducto.Descripcion = "CambioDescripcionProductoTest";
			updateproducto.SerieProducto = "CambioSA56456D54S";
			updateproducto.Marca = "CambioMarcaProductoTest";
			updateproducto.Categoria_id = 999999999;
			updateproducto.Precio = 0;
			updateproducto.Estado = true;

			//prueba en caso de que la categoria asociada al producto no exista

			updateproducto.Categoria_id = 8514656;

			var resut = ProductoController.ModificarProducto(updateproducto);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//caso de prueb en caso de que el producto ingresado no exista

			updateproducto.Id_Producto = 4654646;

			resut = ProductoController.ModificarProducto(updateproducto);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de exito que el producto se modifique exitosamente

			updateproducto.Id_Producto = producto.Id_Producto;
			updateproducto.Categoria_id = 999999999;

			resut = ProductoController.ModificarProducto(updateproducto);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}
	}
}
