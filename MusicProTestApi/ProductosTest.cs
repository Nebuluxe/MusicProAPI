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

			Assert.True(valid != null);

			List<Producto> prods = valid;
		}

		[Fact]
		public void GetProducto_Test()
		{
			int id = producto.Id_Producto;

			var resut = ProductoController.GetProducto(id);

			var valid = Assert.IsType<Producto>(resut);

			Assert.True(valid != null);

			Producto prod = valid;

			Assert.Equal(prod?.Id_Producto, id);
		}

		[Fact]
		public void EliminarProducto_Test()
		{
			int id = producto.Id_Producto;

			var resut = ProductoController.EliminarProducto(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void CrearProducto_Test()
		{
			var resut = ProductoController.CrearProducto(producto);

			var valid = Assert.IsType<TransaccionResult>(resut);

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

			var resut = ProductoController.ModificarProducto(updateproducto);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}
	}
}
