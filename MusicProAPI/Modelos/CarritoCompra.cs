namespace MusicProAPI.Modelos
{
	public class CarritoCompra
	{
		public int Id_Carrito { get; set; } = 0;
		public int Id_usuario { get; set; } = 0;
		public List<DetalleCarritoCompra> DetalleCarrito { get; set; } = new List<DetalleCarritoCompra>();
	}

	public class DetalleCarritoCompra
	{
		public int Id_Carrito { get; set; } = 0;
		public int Id_Producto { get; set; } = 0;
		public int Cantidad { get; set; } = 0;
	
	}
}
