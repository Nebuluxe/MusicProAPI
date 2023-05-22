using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;
using System.Collections.Generic;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("CompraController")]

	public class CarritoCompraController
	{
		GlobalMetods metods = new GlobalMetods();

		[HttpGet]
		[Route("GetCarritos")]
		public dynamic GetCarritos()
		{
			string[] listCarrito = metods.getContentFile("CarritoCompras");
			string[] listDetCarritos = metods.getContentFile("DetalleCarritoCompras");

			if (listCarrito.Count() == 0)
			{
				return new
				{
					mesage = "No hay carritos registrados"
				};
			}

			List<CarritoCompra> carritos = new List<CarritoCompra>();

			for (int i = 0; i < listCarrito.Count(); i++)
			{
				CarritoCompra carrito = new CarritoCompra();

				string[] splitArrCar = listCarrito[i].Split("||");

				carrito.Id = Convert.ToInt32(splitArrCar[0]);
				carrito.Id_usuario = Convert.ToInt32(splitArrCar[1]);

				List<DetalleCarritoCompra> ListDetalle = new List<DetalleCarritoCompra>();

				for (int x = 0; x < listDetCarritos.Count(); x++)
				{
					string[] splitArrDet = listDetCarritos[x].Split("||");

					DetalleCarritoCompra detalle = new DetalleCarritoCompra();

					if (carrito.Id == Convert.ToInt32(splitArrDet[0]))
					{
						detalle.Id_carrito = Convert.ToInt32(splitArrDet[0]);
						detalle.Id_producto = Convert.ToInt32(splitArrDet[1]);
						detalle.cantidad = Convert.ToInt32(splitArrDet[2]);

						ListDetalle.Add(detalle);
					}
				}

				carrito.DetalleCarrito = ListDetalle;
				carritos.Add(carrito);
			}

			return carritos;
		}

		[HttpGet]
		[Route("GetCarrito")]
		public dynamic GetCarrito(int id_usuario)
		{
			string[] listCarrito = metods.getContentFile("CarritoCompras");
			string[] listDetCarritos = metods.getContentFile("DetalleCarritoCompras");

			if (listCarrito.Count() == 0)
			{
				return new
				{
					mesage = "No hay carritos registrados"
				};
			}

			CarritoCompra carrito = new CarritoCompra();
			bool encontrado = false;

			for (int i = 0; i < listCarrito.Count(); i++)
			{
				string[] splitArrCar = listCarrito[i].Split("||");

				if (Convert.ToInt32(splitArrCar[1]) == id_usuario)
				{
					carrito.Id = Convert.ToInt32(splitArrCar[0]);
					carrito.Id_usuario = Convert.ToInt32(splitArrCar[1]);

					List<DetalleCarritoCompra> ListDetalle = new List<DetalleCarritoCompra>();

					for (int x = 0; x < listDetCarritos.Count(); x++)
					{
						string[] splitArrDet = listDetCarritos[x].Split("||");

						DetalleCarritoCompra detalle = new DetalleCarritoCompra();

						if (carrito.Id == Convert.ToInt32(splitArrDet[0]))
						{
							detalle.Id_carrito = Convert.ToInt32(splitArrDet[0]);
							detalle.Id_producto = Convert.ToInt32(splitArrDet[1]);
							detalle.cantidad = Convert.ToInt32(splitArrDet[2]);

							ListDetalle.Add(detalle);
						}
					}

					carrito.DetalleCarrito = ListDetalle;

					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El usaurio '" + id_usuario + "' no tiene un carrito creado"
				};
			}

			return carrito;
		}

		[HttpPost]
		[Route("CrearCarrito")]
		public dynamic CrearCarrito(CarritoCompra carrito)
		{
			string[] listUsuarios = metods.getContentFile("Usuarios");

			bool prodEncontrada = false;

			for (int i = 0; i < listUsuarios.Count(); i++)
			{
				string[] splitArr = listUsuarios[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == carrito.Id_usuario)
				{
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				return new
				{
					message = "El usuario '" + carrito.Id_usuario + "' no existe en los registros",
				};
			}

			string[] list = metods.getContentFile("CarritoCompras");

			bool carritouserExiste = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[1]) == carrito.Id_usuario)
				{
					carritouserExiste = true;
					break;
				}
			}

			if (carritouserExiste)
			{
				return new
				{
					message = "El usuario '" + carrito.Id_usuario + "' ya tiene un carrito creado",
				};
			}

			if (carrito.DetalleCarrito.Count() == 0)
			{
				return new
				{
					message = "Para crear un carrito debe existir almenos 1 producto agregado",
				};
			}

			if (list.Count() == 1)
			{
				string[] splitArr = list[0].Split("||");
				carrito.Id = Convert.ToInt32(splitArr[0]) + 1;
			}
			else
			{
				carrito.Id = list.Count() != 0 ? (Convert.ToInt32(list[list.Count() - 1].Split("||")[0])) + 1 : 1;
			}

			string[] listProductos = metods.getContentFile("Productos");
			string[] listStock = metods.getContentFile("Stock");

			foreach (var item in carrito.DetalleCarrito)
			{
				bool prodencontrado = false;

				for (int x = 0; x < listProductos.Count(); x++)
				{
					string[] splitAtt = listProductos[x].Split("||");

					if (Convert.ToInt32(splitAtt[0]) == item.Id_producto)
					{
						prodencontrado = true;
					}
				}

				if (!prodencontrado)
				{
					return new
					{
						mesage = "El producto '" + item.Id_producto + "' no existe en los registros"
					};
				}

				if (item.cantidad <= 0)
				{
					return new
					{
						message = "La cantidad no puede ser igual o menor a 0",
					};
				}

				bool encontradoEncontrado = false;

				for (int x = 0; x < listStock.Count(); x++)
				{
					string[] splitAtt = listStock[x].Split("||");

					if (Convert.ToInt32(splitAtt[0]) == item.Id_producto)
					{
						if (Convert.ToInt32(splitAtt[1]) == 0)
						{
							return new
							{
								message = "El producto '" + item.Id_producto + "' no tiene stock",
							};
						}
						else if (Convert.ToInt32(splitAtt[1]) < item.cantidad)
						{
							return new
							{
								message = "La cantidad no puede ser mayor a la cantidad de stock del producto '" + item.Id_producto + "'",
							};
						}

						encontradoEncontrado = true;
					}
				}

				if (!encontradoEncontrado)
				{
					return new
					{
						message = "El producto '" + item.Id_producto + "' no tiene stock",
					};
				}

				metods.saveLineFile("CarritoCompras", String.Format("{0}||{1}", carrito.Id, carrito.Id_usuario));
				metods.saveLineFile("DetalleCarritoCompras", String.Format("{0}||{1}||{2}", carrito.Id, item.Id_producto, item.cantidad));
			}

			return new
			{
				message = "Carrito registrado",
				result = carrito
			};
		}

		[HttpPut]
		[Route("AñadirProductoCarrito")]
		public dynamic AñadirProductoCarrito(int id_usuario, int id_producto, int cantidad)
		{
			string[] list = metods.getContentFile("DetalleCarritoCompras");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay carritos registrados"
				};
			}

			string[] listUsuarios = metods.getContentFile("Usuarios");

			bool prodEncontrada = false;

			for (int i = 0; i < listUsuarios.Count(); i++)
			{
				string[] splitArr = listUsuarios[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_usuario)
				{
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				return new
				{
					message = "El usuario '" + id_usuario + "' no existe en los registros",
				};
			}

			string[] listCarrito = metods.getContentFile("CarritoCompras");

			bool carritouserExiste = false;
			int idCarrito = 0;

			for (int i = 0; i < listCarrito.Count(); i++)
			{
				string[] splitArr = listCarrito[i].Split("||");

				if (Convert.ToInt32(splitArr[1]) == id_usuario)
				{
					idCarrito = Convert.ToInt32(splitArr[0]);
					carritouserExiste = true;
					break;
				}
			}

			if (!carritouserExiste)
			{
				return new
				{
					message = "El usuario '" + id_usuario + "' no tiene un carrito creado",
				};
			}

			string[] listProductos = metods.getContentFile("Productos");
			string[] listStock = metods.getContentFile("Stock");

				bool prodencontrado = false;

			for (int x = 0; x < listProductos.Count(); x++)
			{
				string[] splitAtt = listProductos[x].Split("||");

				if (Convert.ToInt32(splitAtt[0]) == id_producto)
				{
					prodencontrado = true;
				}
			}

			if (!prodencontrado)
			{
				return new
				{
					mesage = "El producto '" + id_producto + "' no existe en los registros"
				};
			}

			if (cantidad <= 0)
			{
				return new
				{
					message = "La cantidad no puede ser igual o menor a 0",
				};
			}

			bool encontradoEncontrado = false;

			for (int x = 0; x < listStock.Count(); x++)
			{
				string[] splitAtt = listStock[x].Split("||");

				if (Convert.ToInt32(splitAtt[0]) == id_producto)
				{
					if (Convert.ToInt32(splitAtt[1]) == 0)
					{
						return new
						{
							message = "El producto '" + id_producto + "' no tiene stock",
						};
					}
					else if (Convert.ToInt32(splitAtt[1]) < cantidad)
					{
						return new
						{
							message = "La cantidad no puede ser mayor a la cantidad de stock del producto '" + id_producto + "'",
						};
					}

					encontradoEncontrado = true;
				}
			}

			if (!encontradoEncontrado)
			{
				return new
				{
					message = "El producto '" + id_producto + "' no tiene stock",
				};
			}
			string[] listDetCarritos = metods.getContentFile("DetalleCarritoCompras");

			List<DetalleCarritoCompra> ListDetalle = new List<DetalleCarritoCompra>();

			bool encontrado = false;
			List<string> content = new List<string>();

			for (int i = 0; i < listDetCarritos.Count(); i++)
			{
				string[] splitArr = listDetCarritos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == idCarrito && Convert.ToInt32(splitArr[1]) == id_producto)
				{
					content.Add(String.Format("{0}||{1}||{2}", splitArr[0], splitArr[1], Convert.ToInt32(splitArr[2]) + cantidad));

					encontrado = true;

					continue;
				}

				content.Add(listDetCarritos[i]);
			}

			if (!encontrado)
			{
				metods.saveLineFile("DetalleCarritoCompras", String.Format("{0}||{1}||{2}", idCarrito, id_producto, cantidad));
			}
			else
			{
				metods.updateLineFile("DetalleCarritoCompras", content);
			}

			return new
			{
				mesage = "Producto añadido al carrito"
			};
		}

		[HttpPut]
		[Route("QuitarProductoCarrito")]
		public dynamic QuitarProductoCarrito(int id_usuario, int id_producto)
		{
			string[] list = metods.getContentFile("DetalleCarritoCompras");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay carritos registrados"
				};
			}

			string[] listUsuarios = metods.getContentFile("Usuarios");

			bool prodEncontrada = false;

			for (int i = 0; i < listUsuarios.Count(); i++)
			{
				string[] splitArr = listUsuarios[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_usuario)
				{
					prodEncontrada = true;
				}
			}

			if (!prodEncontrada)
			{
				return new
				{
					message = "El usuario '" + id_usuario + "' no existe en los registros",
				};
			}

			string[] listCarrito = metods.getContentFile("CarritoCompras");

			bool carritouserExiste = false;
			int idCarrito = 0;

			for (int i = 0; i < listCarrito.Count(); i++)
			{
				string[] splitArr = listCarrito[i].Split("||");

				if (Convert.ToInt32(splitArr[1]) == id_usuario)
				{
					idCarrito = Convert.ToInt32(splitArr[0]);
					carritouserExiste = true;
					break;
				}
			}

			if (!carritouserExiste)
			{
				return new
				{
					message = "El usuario '" + id_usuario + "' no tiene un carrito creado",
				};
			}

			string[] listProductos = metods.getContentFile("Productos");

			bool prodencontrado = false;

			for (int x = 0; x < listProductos.Count(); x++)
			{
				string[] splitAtt = listProductos[x].Split("||");

				if (Convert.ToInt32(splitAtt[0]) == id_producto)
				{
					prodencontrado = true;
				}
			}

			if (!prodencontrado)
			{
				return new
				{
					mesage = "El producto '" + id_producto + "' no existe en los registros"
				};
			}

			string[] listDetCarritos = metods.getContentFile("DetalleCarritoCompras");

			bool encontrado = false;
			List<string> content = new List<string>();

			for (int i = 0; i < listDetCarritos.Count(); i++)
			{
				string[] splitArr = listDetCarritos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == idCarrito && Convert.ToInt32(splitArr[1]) == id_producto)
				{
					encontrado = true;
					continue;
				}

				content.Add(listDetCarritos[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El producto '" + id_producto + "' no existe en el carrito de compras del usuario '" + id_usuario + "'"
				};
			}

			metods.updateLineFile("DetalleCarritoCompras", content);

			return new
			{
				mesage = "Producto quitado del carrito"
			};
		}

		[HttpDelete]
		[Route("EliminarCarrito")]
		public dynamic EliminarCarrito(int id_usuario)
		{
			string[] list = metods.getContentFile("CarritoCompras");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay carritos registrados"
				};
			}

			string[] listCarrito = metods.getContentFile("CarritoCompras");

			bool carritouserExiste = false;
			int idCarrito = 0;
			List<string> content = new List<string>();

			for (int i = 0; i < listCarrito.Count(); i++)
			{
				string[] splitArr = listCarrito[i].Split("||");

				if (Convert.ToInt32(splitArr[1]) == id_usuario)
				{
					idCarrito = Convert.ToInt32(splitArr[0]);
					carritouserExiste = true;
					continue;
				}

				content.Add(listCarrito[i]);
			}

			if (!carritouserExiste)
			{
				return new
				{
					message = "El usuario '" + id_usuario + "' no tiene un carrito creado",
				};
			}
			else
			{
				metods.updateLineFile("CarritoCompras", content);
			}

			string[] listDetCarritos = metods.getContentFile("DetalleCarritoCompras");

			List<string> contentDetalle = new List<string>();

			for (int i = 0; i < listDetCarritos.Count(); i++)
			{
				string[] splitArr = listDetCarritos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == idCarrito)
				{
					continue;
				}

				contentDetalle.Add(listDetCarritos[i]);
			}

			metods.updateLineFile("DetalleCarritoCompras", contentDetalle);

			return new
			{
				mesage = "El carrito del usaurio '" + id_usuario + "' fue eliminado exitosamente"
			};
		}
	}
}
