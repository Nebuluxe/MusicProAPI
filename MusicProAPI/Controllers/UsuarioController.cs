using Microsoft.AspNetCore.Mvc;
using MusicProAPI.Modelos;
using System.Text.RegularExpressions;

namespace MusicProAPI.Controllers
{
	[ApiController]
	[Route("Usuario")]
	public class UsuarioController
	{
		GlobalMetods metods = new GlobalMetods();

		[HttpGet]
		[Route("GetUsuarios")]
		public dynamic GetUsuarios()
		{
			TransaccionResult result = new TransaccionResult();

			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay usuarios registrados";

				return result;
			}

			List<Usuario> Usuarios = new List<Usuario>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");
				Usuario Usuario = new Usuario();

				Usuario.Id_Usuario = Convert.ToInt32(splitArr[0]);
				Usuario.Nombre = splitArr[1];
				Usuario.Apellido = splitArr[2];
				Usuario.Correo = splitArr[3];
				Usuario.Password = splitArr[4];

				Usuarios.Add(Usuario);
			}

			return Usuarios;
		}

		[HttpGet]
		[Route("GetUsuario")]
		public dynamic GetUsuario(int id_usuario)
		{
			TransaccionResult result = new TransaccionResult();

			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay usuarios registrados";

				return result;
			}

			Usuario Usuario = new Usuario();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_usuario)
				{
					Usuario.Id_Usuario = Convert.ToInt32(splitArr[0]);
					Usuario.Nombre = splitArr[1];
					Usuario.Apellido = splitArr[2];
					Usuario.Correo = splitArr[3];
					Usuario.Password = splitArr[4];
					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "El usaurio '" + id_usuario + "' no existe en los registros";

				return result;
			}

			return Usuario;
		}

		[HttpPost]
		[Route("CrearUsuario")]
		public dynamic CrearUsuario(Usuario usuario)
		{
			TransaccionResult result = new TransaccionResult();

			if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Apellido) || string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrEmpty(usuario.Password))
			{
				result.resultTransaccion = false;
				result.message = "Faltan datos para almacenar el usaurio";

				return result;
			}

			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 1)
			{
				string[] splitArr = list[0].Split("||");
				usuario.Id_Usuario = Convert.ToInt32(splitArr[0]) + 1;
			}
			else
			{
				usuario.Id_Usuario = list.Count() != 0 ? (Convert.ToInt32(list[list.Count() - 1].Split("||")[0])) + 1 : 1;
			}

			string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(usuario.Correo.Trim().Replace("|", ""), patron))
			{
				result.resultTransaccion = false;
				result.message = "Formato de correo incorrecto";

				return result;
			}

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[3] == usuario.Correo)
				{
					result.resultTransaccion = false;
					result.message = "El corre ingresado ya existe en los registros";

					return result;
				}
			}

			metods.saveLineFile("Usuarios", String.Format("{0}||{1}||{2}||{3}||{4}", usuario.Id_Usuario, usuario.Nombre.Trim().Replace("|", ""), usuario.Apellido.Trim().Replace("|", ""), usuario.Correo.Trim().Replace("|", ""), usuario.Password.Trim().Replace("|", "")));

			result.resultTransaccion = true;
			result.message = "El usuario " + usuario.Nombre + " " + usuario.Apellido + " fue registrado correcctamente";

			return result;
		}

		[HttpPut]
		[Route("ModificarUsuario")]
		public dynamic ModificarUsuario(Usuario usuario)
		{
			TransaccionResult result = new TransaccionResult();

			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay usuarios registrados";

				return result;
			}

			if (!string.IsNullOrEmpty(usuario.Correo))
			{
				string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

				if (!Regex.IsMatch(usuario.Correo.Trim().Replace("|", ""), patron))
				{
					result.resultTransaccion = false;
					result.message = "Formato de correo incorrecto";

					return result;
				}
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			string nombre = "";
			string apellido = "";

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == usuario.Id_Usuario)
				{
					usuario.Id_Usuario = Convert.ToInt32(splitArr[0]);

					nombre = splitArr[1];
					apellido = splitArr[2];

					content.Add(String.Format("{0}||{1}||{2}||{3}||{4}", usuario.Id_Usuario, string.IsNullOrEmpty(usuario.Nombre) ? splitArr[1] : usuario.Nombre.Trim().Replace("|", ""), string.IsNullOrEmpty(usuario.Apellido) ? splitArr[2] : usuario.Apellido.Trim().Replace("|", ""), string.IsNullOrEmpty(usuario.Correo) ? splitArr[3] : usuario.Correo.Trim().Replace("|", ""), string.IsNullOrEmpty(usuario.Password) ? splitArr[4] : usuario.Password.Trim().Replace("|", "")));
					encontrado = true;
					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "El usaurio '" + usuario.Id_Usuario + "' no existe en los registros";

				return result;
			}

			metods.updateLineFile("Usuarios", content);

			result.resultTransaccion = false;
			result.message = "El usuario " + nombre + " " + apellido + " fue modificcado correcctamente";

			return result;
		}

		[HttpDelete]
		[Route("EliminarUsuario")]
		public dynamic EliminarUsuario(int id_usuario)
		{
			TransaccionResult result = new TransaccionResult();

			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 0)
			{
				result.resultTransaccion = false;
				result.message = "No hay usuarios registrados";

				return result;
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			string nombre = "";
			string apellido = "";

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) != id_usuario)
				{
					content.Add(list[i]);
				}
				else
				{

					nombre = splitArr[1];
					apellido = splitArr[2];
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				result.resultTransaccion = false;
				result.message = "El usaurio '" + id_usuario + "' no existe en los registros";

				return result;
			}

			metods.updateLineFile("Usuarios", content);

			result.resultTransaccion = false;
			result.message = "El usuario " + nombre + " " + apellido + " fue eliminado correcctamente";

			return result;
		}
	}
}
