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
		public int Id_Carrito { get; set; }
		public int Id_Producto { get; set; }
		public int Pantidad { get; set; }
	
	}
}
