namespace MusicProAPI.Modelos
{
	public class Producto
	{
		public int Id_Producto { get; set; }
		public string Nombre { get; set; }
		public string Descripcion { get; set; }
		public string SerieProducto { get; set;}
		public string Marca { get; set; }
		public int Categoria_id { get; set; }
		public int Precio { get; set; }
		public string FechaCreacion { get; set; }
		public string FechaModificacion { get; set; }
		public bool Estado { get; set; }
	}
	
}
