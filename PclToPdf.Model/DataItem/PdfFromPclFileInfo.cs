using PclToPdf.Interfaces;
using PclToPdf.Interfaces;

namespace PclToPdf.DataItem
{
	public class PdfFromPclFileInfo : IPclToPdfFileInfo
	{
		#region IPclToPdfFileInfo Members

		public bool Done { get; set; }

		public long FromIndex { get; set; }

		public long ToIndex { get; set; }

		public string VirtualFolder {
			get {
				return Parent.Volume;
			}
		}

		public long Pages {
			get { return (ToIndex - FromIndex) + 1; }
		}

		public string SourceName {
			get { return Parent.Filename; }
		}

		public string PdfFileName { get; set; }

		#endregion

		public readonly IFilePclInfo Parent = null;

		public PdfFromPclFileInfo(IFilePclInfo parent) {
			Parent = parent;

		}

	}
}
