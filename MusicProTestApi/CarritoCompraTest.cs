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
	public class CarritoCompraTest
	{
		private readonly CarritoCompraController CarritoCompraController;

		CarritoCompra carrito = new CarritoCompra();
		Usuario usuario = new Usuario();

		public CarritoCompraTest()
		{
			CarritoCompraController = new CarritoCompraController();

			PrepareTestEscenary();
		}

		private void PrepareTestEscenary()
		{
			GlobalMetods metods = new GlobalMetods();

			usuario.Id_Usuario = 999999999;
			usuario.Nombre = "UserNameTest";
			usuario.Apellido = "UserApellidoTes";
			usuario.Correo = "xxxxxx@gmail.com";
			usuario.Password = "123211";

			string[] userlist = metods.getContentFile("Usuarios");

			bool encontrado = false;

			for (int i = 0; i < userlist.Count(); i++)
			{
				string[] splitArr = userlist[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == usuario.Id_Usuario)
				{
					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				metods.saveLineFile("Usuarios", String.Format("{0}||{1}||{2}||{3}||{4}", usuario.Id_Usuario, usuario.Nombre.Trim().Replace("|", ""), usuario.Apellido.Trim().Replace("|", ""), usuario.Correo.Trim().Replace("|", ""), usuario.Password.Trim().Replace("|", "")));
			}

			string[] listCarrito = metods.getContentFile("CarritoCompras");

			bool carritouserExiste = false;
			int idCarrito = 0;

			for (int i = 0; i < listCarrito.Count(); i++)
			{
				string[] splitArr = listCarrito[i].Split("||");

				if (Convert.ToInt32(splitArr[1]) == usuario.Id_Usuario)
				{
					idCarrito = Convert.ToInt32(splitArr[0]);
					carritouserExiste = true;
					break;
				}
			}

			if (!carritouserExiste)
			{
				carrito.Id_usuario = usuario.Id_Usuario;

				if (listCarrito.Count() == 1)
				{
					string[] splitArr = listCarrito[0].Split("||");
					carrito.Id_Carrito = Convert.ToInt32(splitArr[0]) + 1;
				}
				else
				{
					carrito.Id_Carrito = listCarrito.Count() != 0 ? (Convert.ToInt32(listCarrito[listCarrito.Count() - 1].Split("||")[0])) + 1 : 1;
				}

				metods.saveLineFile("CarritoCompras", String.Format("{0}||{1}", carrito.Id_Carrito, carrito.Id_usuario));

				idCarrito = carrito.Id_Carrito;
			}

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

			for (int i = 0; i < 2; i++)
			{
				Producto producto = new Producto();

				if (i == 0)
				{
					producto.Id_Producto = 999999999;
				}
				else
				{
					producto.Id_Producto = 888888888;
				}

				producto.Nombre = "NombreProductoTest";
				producto.Descripcion = "DescripcionProductoTest";
				producto.SerieProducto = "SA56456D54S";
				producto.Marca = "MarcaProductoTest";
				producto.Categoria_id = categoria.Id_Categoria;
				producto.Precio = 999999;
				producto.FechaCreacion = "00-00-0000 00:00:00";
				producto.FechaModificacion = "00-00-0000 00:00:00";
				producto.Estado = true;

				string[] prodlist = metods.getContentFile("Productos");

				bool prodencontrado = false;

				for (int x = 0; x < prodlist.Count(); x++)
				{
					string[] splitArr = prodlist[x].Split("||");

					if (Convert.ToInt32(splitArr[0]) == producto.Id_Producto)
					{
						prodencontrado = true;
						break;
					}
				}

				if (!prodencontrado)
				{
					metods.saveLineFile("Productos", String.Format("{0}||{1}||{2}||{3}||{4}||{5}||{6}||{7}||{8}||{9}", producto.Id_Producto, producto.Nombre.Trim().Replace("|", ""), producto.Descripcion.Trim().Replace("|", ""), producto.SerieProducto.Trim().Replace("|", ""), producto.Marca.Trim().Replace("|", ""), producto.Categoria_id, producto.Precio, producto.FechaCreacion.Trim().Replace("|", ""), producto.FechaModificacion.Trim().Replace("|", ""), producto.Estado));
				}

				Stock stock = new Stock();

				stock.Id_Producto = producto.Id_Producto;
				stock.CantidadStock = 10000;

				string[] list = metods.getContentFile("Stock");

				bool encontrada = false;

				for (int y = 0; y < list.Count(); y++)
				{
					string[] splitArr = list[y].Split("||");

					if (Convert.ToInt32(splitArr[0]) == categoria.Id_Categoria)
					{
						encontrada = true;
						break;
					}
				}

				if (!encontrada)
				{
					metods.saveLineFile("Stock", String.Format("{0}||{1}", stock.Id_Producto, stock.CantidadStock));
				}

				string[] listDetCarritos = metods.getContentFile("DetalleCarritoCompras");

				DetalleCarritoCompra Detalle = new DetalleCarritoCompra();

				bool detcarencontrado = false;

				for (int z = 0; z < listDetCarritos.Count(); z++)
				{
					string[] splitArr = listDetCarritos[z].Split("||");

					if (Convert.ToInt32(splitArr[0]) == idCarrito && Convert.ToInt32(splitArr[1]) == producto.Id_Producto)
					{
						detcarencontrado = true;

						Detalle.Id_Carrito = idCarrito;
						Detalle.Id_Producto = producto.Id_Producto;
						Detalle.Cantidad = 1;

						carrito.DetalleCarrito.Add(Detalle);

						break;
					}
				}

				if (!detcarencontrado)
				{
					metods.saveLineFile("DetalleCarritoCompras", String.Format("{0}||{1}||{2}", idCarrito, producto.Id_Producto, 1));

					Detalle.Id_Carrito = idCarrito;
					Detalle.Id_Producto = producto.Id_Producto;
					Detalle.Cantidad = 1;

					carrito.DetalleCarrito.Add(Detalle);
				}
			}


		}

		[Fact]
		public void GetCarritos_Test()
		{
			var resut = CarritoCompraController.GetCarritos();

			var valid = Assert.IsType<List<CarritoCompra>>(resut);

			if (valid == null)
			{
				var valid2 = Assert.IsType<TransaccionResult>(resut);

				Assert.True(valid2 != null);
			}
			else
			{
				Assert.True(valid != null);
			}
		}

		[Fact]
		public void GetCarrito_Test()
		{
			int id = usuario.Id_Usuario;

			var resut = CarritoCompraController.GetCarrito(id);

			var valid = Assert.IsType<CarritoCompra>(resut);

			if (valid == null)
			{
				var valid2 = Assert.IsType<TransaccionResult>(resut);

				Assert.True(valid2 != null);
			}
			else
			{
				Assert.True(valid != null);

				CarritoCompra carr = valid;

				Assert.Equal(carr?.Id_usuario, id);
			}
		}

		[Fact]
		public void EliminarCarrito_Test()
		{
			int id = carrito.Id_Carrito;

			var resut = CarritoCompraController.EliminarCarrito(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void QuitarProductoCarrito_Test()
		{
			var resut = CarritoCompraController.QuitarProductoCarrito(usuario.Id_Usuario, 999999999);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void AñadirProductoCarrito_Test()
		{
			var resut = CarritoCompraController.AñadirProductoCarrito(usuario.Id_Usuario, 999999999, 1);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}
	}
}
