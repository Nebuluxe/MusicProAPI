namespace MusicProAPI.Modelos
{
	public class Producto
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public string SerieProducto { get; set;}
		public string Marca { get; set; }
		public int Categoria_id { get; set; }
		public int Precio { get; set; }
		//public string StockActual { get; set; }
		public string fechaCreacion { get; set; }
		public string fechaModificacion { get; set; }
	}
	
}
