using PclToPdf.Interfaces;

namespace PclToPdf.Config
{
	public class FilePclConfig : IFilePclInfo
	{
		#region IFilePclInfo Members

		public string Volume { get; set; }

		public string Filename { get; set; }

		public bool Done { get; set; }

		public bool ToDo { get; set; }

		public int Quanti { get; set; }

		public string[] PdfFiles { get; set; }

		#endregion
	}
}
