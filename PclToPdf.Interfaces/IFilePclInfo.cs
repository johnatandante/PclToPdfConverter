
namespace PclToPdf.Interfaces
{
	public interface IFilePclInfo
    {
		string Volume { get; }

		string Filename { get; }

		bool Done { get; set; }

		bool ToDo { get; set; }

		int Quanti { get; set; }

		string[] PdfFiles { get; set; }

    }
}
