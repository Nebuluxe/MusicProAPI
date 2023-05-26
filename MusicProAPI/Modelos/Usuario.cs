namespace MusicProAPI.Modelos
{
	public class Usuario
	{
		public int Id_Usuario { get; set; } = 0;
        public string Nombre { get; set; } = string.Empty;
		public string Apellido { get; set; } = string.Empty;
		public string Correo { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
