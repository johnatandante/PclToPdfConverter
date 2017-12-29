
namespace PclToPdf.Interfaces
{
	public interface IPclToPdfFileInfo
	{

		string SourceName { get; }

		bool Done { get; set; }

		long Pages { get; }

		string PdfFileName { get; set; }

		string VirtualFolder { get; }

		long FromIndex { get; }

	}
}
