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

			if (valid == null)
			{
				//prueba en caso de que no hayan usuarios registrados

				var valid2 = Assert.IsType<TransaccionResult>(resut);

				Assert.True(valid2 != null);
			}
			else
			{
				//prueba en caso de que la respuesta sea la lusta de usuarios

				Assert.True(valid != null);
			}
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

			//prueba en caso de que el id ingresado no exista

			resut = UsuarioController.GetUsuario(518914);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void EliminarUsuario_Test()
		{
			int id = usuario.Id_Usuario;

			var resut = UsuarioController.EliminarUsuario(id);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			resut = UsuarioController.EliminarUsuario(1416849);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void CrearUsuario_Test()
		{
			Usuario testcreateuser = usuario;

			//prueba en casso de que no se ingresen todos los datos de un usaurio

			testcreateuser.Nombre = "";
			testcreateuser.Apellido = "";

			var resut = UsuarioController.CrearUsuario(testcreateuser);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de que el formato del correo sea invalido

			testcreateuser = usuario;

			testcreateuser.Correo = "xxxxgmaicom";

			resut = UsuarioController.CrearUsuario(testcreateuser);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba al crear un usuario

			resut = UsuarioController.CrearUsuario(usuario);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba a la hora de ingresar un usuario con un correo ya existente en los registros

			resut = UsuarioController.CrearUsuario(usuario);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}

		[Fact]
		public void ModificarUsuario_Test()
		{
			Usuario updtaeusuario = new Usuario();
			updtaeusuario.Id_Usuario = usuario.Id_Usuario;
			updtaeusuario.Nombre = "CambioUserNameTest";
			updtaeusuario.Apellido = "CambioUserApellidoTes";

			//prueba en caso de que el usuario ingresaado no exista

			updtaeusuario.Id_Usuario = 168165468;

			var resut = UsuarioController.ModificarUsuario(updtaeusuario);

			var valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso de que el formato del correo sea invalido

			updtaeusuario.Correo = "xxxxgmaicom";

			resut = UsuarioController.ModificarUsuario(updtaeusuario);

			valid = Assert.IsType<TransaccionResult>(resut);

			//prueba en caso de que el correo ingresado no exista en los registros

			resut = UsuarioController.ModificarUsuario(updtaeusuario);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba en caso exitoso de que se modifiquen los datos de un usuario

			updtaeusuario.Id_Usuario = usuario.Id_Usuario;
			updtaeusuario.Correo = "Cambioxxxxxx@gmail.com";
			updtaeusuario.Password = "Cambio123211";

			resut = UsuarioController.ModificarUsuario(updtaeusuario);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);

			//prueba a la hora de ingresar un usuario con un correo ya existente en los registros

			resut = UsuarioController.ModificarUsuario(updtaeusuario);

			valid = Assert.IsType<TransaccionResult>(resut);

			Assert.True(valid != null);
		}
	}
}