namespace MusicProAPI.Modelos
{
	public class CarritoCompra
	{
		public int Id { get; set; }
		public int Id_usuario { get; set; }
		public List<DetalleCarritoCompra> DetalleCarrito { get; set; }
	}

	public class DetalleCarritoCompra
	{
		public int Id_carrito { get; set; }
		public int Id_producto { get; set; }
		public int cantidad { get; set; }
	
	}
}
