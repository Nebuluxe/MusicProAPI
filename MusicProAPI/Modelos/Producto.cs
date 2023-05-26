namespace MusicProAPI.Modelos
{
	public class Producto
	{
		public int Id_Producto { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
		public string Descripcion { get; set; } = string.Empty;
		public string SerieProducto { get; set; } = string.Empty;
		public string Marca { get; set; } = string.Empty;
		public int Categoria_id { get; set; } = 0;
        public int Precio { get; set; } = 0;
        public string FechaCreacion { get; set; } = string.Empty;
		public string FechaModificacion { get; set; } = string.Empty;
		public bool Estado { get; set; } = true;
    }
	
}
