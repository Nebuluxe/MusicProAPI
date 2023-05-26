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

				carrito.Id_Carrito = Convert.ToInt32(splitArrCar[0]);
				carrito.Id_usuario = Convert.ToInt32(splitArrCar[1]);

				List<DetalleCarritoCompra> ListDetalle = new List<DetalleCarritoCompra>();

				for (int x = 0; x < listDetCarritos.Count(); x++)
				{
					string[] splitArrDet = listDetCarritos[x].Split("||");

					DetalleCarritoCompra detalle = new DetalleCarritoCompra();

					if (carrito.Id_Carrito == Convert.ToInt32(splitArrDet[0]))
					{
						detalle.Id_Carrito = Convert.ToInt32(splitArrDet[0]);
						detalle.Id_Producto = Convert.ToInt32(splitArrDet[1]);
						detalle.Cantidad = Convert.ToInt32(splitArrDet[2]);

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
					carrito.Id_Carrito = Convert.ToInt32(splitArrCar[0]);
					carrito.Id_usuario = Convert.ToInt32(splitArrCar[1]);

					List<DetalleCarritoCompra> ListDetalle = new List<DetalleCarritoCompra>();

					for (int x = 0; x < listDetCarritos.Count(); x++)
					{
						string[] splitArrDet = listDetCarritos[x].Split("||");

						DetalleCarritoCompra detalle = new DetalleCarritoCompra();

						if (carrito.Id_Carrito == Convert.ToInt32(splitArrDet[0]))
						{
							detalle.Id_Carrito = Convert.ToInt32(splitArrDet[0]);
							detalle.Id_Producto = Convert.ToInt32(splitArrDet[1]);
							detalle.Cantidad = Convert.ToInt32(splitArrDet[2]);

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
        [Route("PagarCarrito")]
        public dynamic PagarCarrito(int id_usuario)
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

            bool userEncontrado = false;

            for (int i = 0; i < listUsuarios.Count(); i++)
            {
                string[] splitArr = listUsuarios[i].Split("||");

                if (Convert.ToInt32(splitArr[0]) == id_usuario)
                {
                    userEncontrado = true;
                }
            }

            if (!userEncontrado)
            {
                return new
                {
                    message = "El usuario '" + id_usuario + "' no existe en los registros",
                };
            }

            string[] listCarrito = metods.getContentFile("CarritoCompras");

            bool carritouserExiste = false;
            int idCarrito = 0;

            List<string> contentCarrito = new List<string>();

            for (int i = 0; i < listCarrito.Count(); i++)
            {
                string[] splitArr = listCarrito[i].Split("||");

                if (Convert.ToInt32(splitArr[1]) != id_usuario)
                {
                    contentCarrito.Add(listCarrito[i]);
                    break;
                }
                else
                {
                    idCarrito = Convert.ToInt32(splitArr[0]);
                    carritouserExiste = true;
                }
            }

            if (!carritouserExiste)
            {
                return new
                {
                    message = "El carrito '" + id_usuario + "' no existe",
                };
            }

            string[] listProductos = metods.getContentFile("Productos");
            string[] listStock = metods.getContentFile("Stock");

            string[] listDetCarritos = metods.getContentFile("DetalleCarritoCompras");

            List<Producto> productos = new List<Producto>();

            List<string> contentDetalleCerrito = new List<string>();
			List<string> contentStocks = new List<string>();

			for (int i = 0; i < listDetCarritos.Count(); i++)
            {
                string[] splitArrDetCarr = listDetCarritos[i].Split("||");

                if (Convert.ToInt32(splitArrDetCarr[0]) == idCarrito)
                {
                    for (int x = 0; x < listProductos.Count(); x++)
                    {
                        string[] splitArrProd = listProductos[x].Split("||");

                        if (Convert.ToInt32(splitArrProd[0]) == Convert.ToInt32(splitArrDetCarr[1]))
                        {
                            Producto producto = new Producto();

                            producto.Id_Producto = Convert.ToInt32(splitArrProd[0]);
                            producto.Nombre = splitArrProd[1];
                            producto.Descripcion = splitArrProd[2];
                            producto.SerieProducto = splitArrProd[3];
                            producto.Marca = splitArrProd[4];
                            producto.Categoria_id = Convert.ToInt32(splitArrProd[5]);
                            producto.Precio = Convert.ToInt32(splitArrProd[6]);
                            producto.FechaCreacion = splitArrProd[7];
                            producto.FechaModificacion = splitArrProd[8];
                            producto.Estado = Convert.ToBoolean(splitArrProd[9]);

                            productos.Add(producto);

							bool tieneStock = false;

							for (int y = 0; y < listStock.Count(); y++)
							{
								string[] splitAttStock = listStock[y].Split("||");

								if (Convert.ToInt32(splitAttStock[0]) == producto.Id_Producto)
								{
									if (Convert.ToInt32(splitAttStock[1]) == 0)
									{
										return new
										{
											message = "El producto '" + producto.Id_Producto + " - " +  producto.Nombre + "' no tiene stock",
										};
									}
									
									if (Convert.ToInt32(splitAttStock[1]) <= Convert.ToInt32(splitArrDetCarr[2]))
									{
                                        return new
                                        {
                                            message = "El producto '" + producto.Id_Producto + " - " + producto.Nombre + "' no tiene stock suficiente para realizar la compra",
                                        };
                                    }

									int cantidadFinal = Convert.ToInt32(splitAttStock[1]) - Convert.ToInt32(splitArrDetCarr[2]);

                                    contentStocks.Add(string.Format("{0}||{1}", splitAttStock[0], cantidadFinal));
                                    tieneStock = true;
									break;
								}
                            }

							if (!tieneStock)
							{
                                return new
                                {
                                    message = "El producto '" + producto.Id_Producto + "' no tiene stock",
                                };
                            }
                        }
                    }

                    continue;
                }

                contentDetalleCerrito.Add(listDetCarritos[i]);
            }


			for (int i = 0; i < listStock.Count(); i++)
			{
                string[] splitAttStock = listStock[i].Split("||");

				bool stockencontrado = false;

				foreach (var item in contentStocks)
				{
                    if (Convert.ToInt32(splitAttStock[0]) == Convert.ToInt32(item.Split("||")[0]))
                    {
						stockencontrado = true;
                        break;
                    }
                }

				if (stockencontrado)
				{
					continue;
				}
				else
				{
                    contentStocks.Add(listStock[i]);
                }

            }

			//efectuar el pago de los productos por FakeBankAPI_MSP

            //eliminar carrito pagado
            metods.updateLineFile("CarritoCompras", contentCarrito);
            metods.updateLineFile("DetalleCarritoCompras", contentDetalleCerrito);

            //actualizar stock de productos
            metods.updateLineFile("Stock", contentStocks);

            return new
            {
                mesage = "Mensaje al final de la transaccion undefined"
            };
        }

        [HttpPost]
		[Route("AñadirProductoCarrito")]
		public dynamic AñadirProductoCarrito(int id_usuario, int id_producto, int cantidad)
		{
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
				CarritoCompra carrito = new CarritoCompra();

				carrito.Id_usuario = id_usuario;

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

		[HttpPost]
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
					message = "El usuario '" + id_usuario + "' no tiene productos añadidos en el carrito",
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
			int cantLiCarrito = 0;

			for (int i = 0; i < listDetCarritos.Count(); i++)
			{
				string[] splitArr = listDetCarritos[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == idCarrito && Convert.ToInt32(splitArr[1]) == id_producto)
				{
					cantLiCarrito++;
					encontrado = true;
					continue;
				}

				if (Convert.ToInt32(splitArr[0]) == idCarrito)
				{
					cantLiCarrito++;
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

			if (cantLiCarrito == 1)
			{
				string[] listaCarros = metods.getContentFile("CarritoCompras");

				List<string> lines = new List<string>();

				for (int i = 0; i < listCarrito.Count(); i++)
				{
					string[] splitArr = listCarrito[i].Split("||");

					if (Convert.ToInt32(splitArr[1]) == id_usuario)
					{
						continue;
					}

					lines.Add(listCarrito[i]);
				}

				metods.updateLineFile("CarritoCompras", lines);

				return new
				{
					mesage = "Producto quitado del carrito"
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
