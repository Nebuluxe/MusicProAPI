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

			Assert.True(valid != null);

			List<Stock> stos = valid;
		}

		[Fact]
		public void GetStock_Test()
		{
			int id = stock.Id_Producto;

			var resut = StockController.GetStockProducto(id);

			var valid = Assert.IsType<Stock>(resut);

			Assert.True(valid != null);

			Stock sto = valid;

			Assert.Equal(sto?.Id_Producto, id);
		}

		[Fact]
		public void EliminarStock_Test()
		{
			int id = stock.Id_Producto;

			var resut = StockController.EliminarStock(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void CrearStock_Test()
		{
			var resut = StockController.CrearStock(stock.Id_Producto, stock.CantidadStock);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void AumentarStock_Test()
		{
			var resut = StockController.AumentarStock(stock.Id_Producto, 100);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void RebajarStock_Test()
		{
			var resut = StockController.RebajarStock(stock.Id_Producto, 100);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}
	}
}
