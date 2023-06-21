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
	public class StockTest
	{
		private readonly StockController StockController;

		Stock stock = new Stock();
		public StockTest()
		{
			StockController = new StockController();

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

			Producto producto = new Producto();

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

			string[] prodlist = metods.getContentFile("Productos");

			bool prodencontrado = false;

			for (int i = 0; i < prodlist.Count(); i++)
			{
				string[] splitArr = prodlist[i].Split("||");

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

			stock.Id_Producto = 999999999;
			stock.CantidadStock = 10000;

			string[] list = metods.getContentFile("Stock");

			bool encontrada = false;

			for (int i = 0; i < catelist.Count(); i++)
			{
				string[] splitArr = catelist[i].Split("||");

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
		}

		[Fact]
		public void GetStockProductos_Test()
		{
			var resut = StockController.GetStockProductos();

			var valid = Assert.IsType<List<Stock>>(resut);

			if (valid == null)
			{
				//prueba en caso de que no encuentre stock creado

				var valid2 = Assert.IsType<TransaccionResult>(resut);

				Assert.True(valid2 != null);
			}
			else
			{
				//prueba en casoso de que enceuntre stock

				Assert.True(valid != null);
			}
		}

		[Fact]
		public void GetStock_Test()
		{
            //prueba en caso de que el id ingresado no exista en los registros

            int id = 519465;

            var resut = StockController.GetStockProducto(id);

            var valid = Assert.IsType<TransaccionResult>(resut);

            Assert.True(valid != null);

            //prueba en caso de exito que el id exista 

            id = stock.Id_Producto;

            resut = StockController.GetStockProducto(id);

            valid = Assert.IsType<Stock>(resut);

            Assert.True(valid != null);

            Stock prod = valid;

            Assert.Equal(prod?.Id_Producto, id);
        }

		[Fact]
		public void EliminarStock_Test()
		{
            int id = stock.Id_Producto;

			//prueba en caso de que el id  del producto no exista en los registros

			id = 457478654;

            var resut = StockController.EliminarStock(id);

            var valid = Assert.IsType<TransaccionResult>(resut);

            Assert.True(valid != null);

			//prueba en caso de equito que se logre eliminar el stock de producto

            id = stock.Id_Producto;

		    resut = StockController.EliminarStock(id);

		    valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void CrearStock_Test()
		{
			Stock teststock = stock;

			//prueba en caso de que el id  del producto no exista en los registros

			teststock.Id_Producto = 56156165;

			var resut = StockController.CrearStock(teststock.Id_Producto, teststock.CantidadStock);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de que la cantidad de stock inicnial sea 0

			teststock.Id_Producto = stock.Id_Producto;
			teststock.CantidadStock = 0;

			resut = StockController.CrearStock(teststock.Id_Producto, teststock.CantidadStock);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de exito que el stock se crea

			resut = StockController.CrearStock(stock.Id_Producto, stock.CantidadStock);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de que el producto ya tenga un stock creado

			resut = StockController.CrearStock(stock.Id_Producto, stock.CantidadStock);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void AumentarStock_Test()
		{
			Stock teststock = stock;

			//prueba en caso de que el id  del producto no exista en los registros

			teststock.Id_Producto = 56156165;

			var resut = StockController.AumentarStock(teststock.Id_Producto, 100);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de que la cantidad de stock sea 0

			teststock.Id_Producto = stock.Id_Producto;
			teststock.CantidadStock = 0;

			resut = StockController.AumentarStock(teststock.Id_Producto, teststock.CantidadStock);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de que se efectue el aumento de stock

			resut = StockController.AumentarStock(stock.Id_Producto, 100);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void RebajarStock_Test()
		{

            Stock teststock = stock;

            //prueba en caso de que el id  del producto no exista en los registros

            teststock.Id_Producto = 56156165;

            var resut = StockController.RebajarStock(teststock.Id_Producto, 100);

            var valid = Assert.IsType<TransaccionResult>(resut);

            Assert.True(valid != null);

            //prueba en caso de que la cantidad de stock sea 0

            teststock.Id_Producto = stock.Id_Producto;
            teststock.CantidadStock = 0;

            resut = StockController.RebajarStock(teststock.Id_Producto, teststock.CantidadStock);

            valid = Assert.IsType<TransaccionResult>(resut);

            Assert.True(valid != null);

            //prueba en caso de que se efectue la rebaja de stock

            resut = StockController.RebajarStock(stock.Id_Producto, 100);

            valid = Assert.IsType<TransaccionResult>(resut);

            Assert.True(valid != null);
        }
	}
}
