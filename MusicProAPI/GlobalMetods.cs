using MusicProAPI.Modelos;
using System.IO;

namespace MusicProAPI
{
	public class GlobalMetods
	{
		public string[] getContentFile(string NomDoc)
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

		public void saveLineFile(string NomDoc, string lineContent)
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
		}

		public void updateLineFile(string NomDoc, List<string> content)
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
		}
	}
}
