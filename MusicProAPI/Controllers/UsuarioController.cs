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
			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay usuarios registrados"
				};
			}

			List<Usuario> Usuarios = new List<Usuario>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");
				Usuario Usuario = new Usuario();

				Usuario.Id = Convert.ToInt32(splitArr[0]);
				Usuario.Nombre = splitArr[1];
				Usuario.Apellido = splitArr[2];
				Usuario.correo = splitArr[3];
				Usuario.password = splitArr[4];

				Usuarios.Add(Usuario);
			}

			return Usuarios;
		}

		[HttpGet]
		[Route("GetUsuario")]
		public dynamic GetUsuario(int id_usuario)
		{
			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay usuarios registrados"
				};
			}

			Usuario Usuario = new Usuario();

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == id_usuario)
				{
					Usuario.Id = Convert.ToInt32(splitArr[0]);
					Usuario.Nombre = splitArr[1];
					Usuario.Apellido = splitArr[2];
					Usuario.correo = splitArr[3];
					Usuario.password = splitArr[4];
					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El usaurio '" + id_usuario + "' no existe en los registros"
				};
			}

			return Usuario;
		}

		[HttpPost]
		[Route("CrearUsuario")]
		public dynamic CrearUsuario(Usuario usuario)
		{
			if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Apellido) || string.IsNullOrEmpty(usuario.correo) || string.IsNullOrEmpty(usuario.password))
			{
				return new
				{
					message = "Faltan datos para almacenar el usaurio",
				};
			}

			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 1)
			{
				string[] splitArr = list[0].Split("||");
				usuario.Id = Convert.ToInt32(splitArr[0]) + 1;
			}
			else
			{
				usuario.Id = list.Count() != 0 ? (Convert.ToInt32(list[list.Count() - 1].Split("||")[0])) + 1 : 1;
			}

			string patron = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			if (!Regex.IsMatch(usuario.correo.Trim().Replace("|", ""), patron))
			{
				return new
				{
					mesage = "Formato de correo incorrecto"
				};
			}

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (splitArr[3] == usuario.correo)
				{
					return new
					{
						mesage = "El corre ingresado ya existe en los registros"
					};
				}
			}

			metods.saveLineFile("Usuarios", String.Format("{0}||{1}||{2}||{3}||{4}", usuario.Id, usuario.Nombre.Trim().Replace("|", ""), usuario.Apellido.Trim().Replace("|", ""), usuario.correo.Trim().Replace("|", ""), usuario.password.Trim().Replace("|", "")));

			return new
			{
				message = "Usuario registrado",
				result = usuario
			};
		}

		[HttpPut]
		[Route("ModificarUsuario")]
		public dynamic ModificarUsuario(Usuario usuario)
		{
			if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Apellido) || string.IsNullOrEmpty(usuario.correo) || string.IsNullOrEmpty(usuario.password))
			{
				return new
				{
					message = "Faltan datos para almacenar el usaurio",
				};
			}

			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay usuarios registrados"
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == usuario.Id)
				{
					usuario.Id = Convert.ToInt32(splitArr[0]);

					content.Add(String.Format("{0}||{1}||{2}||{3}||{4}", usuario.Id, usuario.Nombre.Trim().Replace("|", ""), usuario.Apellido.Trim().Replace("|", ""), usuario.correo.Trim().Replace("|", ""), usuario.password.Trim().Replace("|", "")));
					encontrado = true;
					continue;
				}

				content.Add(list[i]);
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El usaurio '" + usuario.Id + "' no existe en los registros"
				};
			}

			metods.updateLineFile("Usuarios", content);

			return new
			{
				mesage = "Usuario modificado",
				result = usuario
			};
		}

		[HttpDelete]
		[Route("EliminarUsuario")]
		public dynamic EliminarUsuario(int id_usuario)
		{
			string[] list = metods.getContentFile("Usuarios");

			if (list.Count() == 0)
			{
				return new
				{
					mesage = "No hay usuarios registrados"
				};
			}

			bool encontrado = false;
			List<string> content = new List<string>();

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) != id_usuario)
				{
					content.Add(list[i]);
				}
				else
				{
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				return new
				{
					mesage = "El usaurio '" + id_usuario + "' no existe en los registros"
				};
			}

			metods.updateLineFile("Usuarios", content);

			return new
			{
				mesage = "El usaurio '" + id_usuario + "' fue eliminado exitosamente"
			};
		}
	}
}
