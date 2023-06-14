using MusicProAPI.Modelos;
using System.IO;

namespace MusicProAPI
{
	public class GlobalMetods
	{
		public dynamic getContentFile(string NomDoc)
		{
			TransaccionResult result = new TransaccionResult();

			try
			{
				if (!Directory.Exists("C:\\txtMusicPro"))
				{
					Directory.CreateDirectory("C:\\txtMusicPro");
				}

				if (!System.IO.File.Exists("C:\\txtMusicPro\\" + NomDoc + ".txt"))
				{
					System.IO.File.Create("C:\\txtMusicPro\\" + NomDoc + ".txt").Close();
				}

				return System.IO.File.ReadAllLines("C:\\txtMusicPro\\" + NomDoc + ".txt");
			}
			catch (Exception)
			{
				result.resultTransaccion = true;
				result.message = "Favor intente nuevamente";

				return result;
				throw;
			}
		}

		public dynamic saveLineFile(string NomDoc, string lineContent)
		{
			TransaccionResult result = new TransaccionResult();

			try
			{
				if (!Directory.Exists("C:\\txtMusicPro"))
				{
					Directory.CreateDirectory("C:\\txtMusicPro");
				}

				if (!System.IO.File.Exists("C:\\txtMusicPro\\" + NomDoc + ".txt"))
				{
					System.IO.File.Create("C:\\txtMusicPro\\" + NomDoc + ".txt").Close();
				}

				StreamWriter sw = System.IO.File.AppendText("C:\\txtMusicPro\\" + NomDoc + ".txt");
				sw.WriteLine(lineContent);
				sw.Close();

				return true;
			}
			catch (Exception)
			{
				result.resultTransaccion = true;
				result.message = "Favor intente nuevamente";

				return result;
				throw;
			}
		}

		public dynamic updateLineFile(string NomDoc, List<string> content)
		{
			TransaccionResult result = new TransaccionResult();

			try
			{
				if (!Directory.Exists("C:\\txtMusicPro"))
				{
					Directory.CreateDirectory("C:\\txtMusicPro");
				}

				if (!System.IO.File.Exists("C:\\txtMusicPro\\" + NomDoc + ".txt"))
				{
					System.IO.File.Create("C:\\txtMusicPro\\" + NomDoc + ".txt").Close();
				}

				System.IO.File.WriteAllLines("C:\\txtMusicPro\\" + NomDoc + ".txt", content);

				return true;
			}
			catch (Exception)
			{
				result.resultTransaccion = true;
				result.message = "Favor intente nuevamente";

				return result;
				throw;
			}
		}
	}
}
