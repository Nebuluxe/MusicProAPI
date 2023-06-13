using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Http;
using MusicProAPI;
using MusicProAPI.Controllers;
using MusicProAPI.Modelos;
using Newtonsoft.Json;
using Xunit;

namespace MusicProTestApi
{
	public class UsuarioTest
	{
		private readonly UsuarioController UsuarioController;

		Usuario usuario = new Usuario();
		public UsuarioTest()
		{
			UsuarioController = new UsuarioController();

			PrepareTestEscenary();
		}

		private void PrepareTestEscenary()
		{
			GlobalMetods metods = new GlobalMetods();

			usuario.Id_Usuario = 999999999;
			usuario.Nombre = "UserNameTest";
			usuario.Apellido = "UserApellidoTes";
			usuario.Correo = "xxxxxx@gmail.com";
			usuario.Password = "123211";

			string[] list = metods.getContentFile("Usuarios");

			bool encontrado = false;

			for (int i = 0; i < list.Count(); i++)
			{
				string[] splitArr = list[i].Split("||");

				if (Convert.ToInt32(splitArr[0]) == usuario.Id_Usuario)
				{
					encontrado = true;
					break;
				}
			}

			if (!encontrado)
			{
				metods.saveLineFile("Usuarios", String.Format("{0}||{1}||{2}||{3}||{4}", usuario.Id_Usuario, usuario.Nombre.Trim().Replace("|", ""), usuario.Apellido.Trim().Replace("|", ""), usuario.Correo.Trim().Replace("|", ""), usuario.Password.Trim().Replace("|", "")));
			}
		}

		[Fact]
		public void GetUsuarios_Test()
		{
			var resut = UsuarioController.GetUsuarios();

			var valid = Assert.IsType<List<Usuario>>(resut);

			Assert.True(valid != null);

			List<Usuario> users = valid;
		}

		[Fact]
		public void GetUsuario_Test()
		{
			int id = usuario.Id_Usuario;

			var resut = UsuarioController.GetUsuario(id);

			var valid = Assert.IsType<Usuario>(resut);

			Assert.True(valid != null);

			Usuario user = valid;

			Assert.Equal(user?.Id_Usuario, id);
		}

		[Fact]
		public void EliminarUsuario_Test()
		{
			int id = usuario.Id_Usuario;

			var resut = UsuarioController.EliminarUsuario(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void CrearUsuario_Test()
		{
			var resut = UsuarioController.CrearUsuario(usuario);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void ModificarUsuario_Test()
		{
			Usuario updtaeusuario = new Usuario();
			updtaeusuario.Id_Usuario = usuario.Id_Usuario;
			updtaeusuario.Nombre = "CambioUserNameTest";
			updtaeusuario.Apellido = "CambioUserApellidoTes";
			updtaeusuario.Correo = "Cambioxxxxxx@gmail.com";
			updtaeusuario.Password = "Cambio123211";

			var resut = UsuarioController.ModificarUsuario(updtaeusuario);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}
	}
}