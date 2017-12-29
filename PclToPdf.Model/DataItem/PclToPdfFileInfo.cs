using System.IO;
using PclToPdf.Interfaces;

namespace PclToPdf.DataItem
{

	public class PclToPdfFileInfo : IPclToPdfFileInfo
	{

		public string SourceName { get; set; }

		public FileInfo FilePcl { get; set; }
		
		public long FromIndex { get; set; }

		public long ToIndex { get; set; }

		public long Pages {
			get {
				return ToIndex - FromIndex;
			}
		}

		public string PdfFileName { get; set; }

		public bool Done { get; set; }

		public string VirtualFolder { get; set; }

		public string PclFileName { get; set; }

		public string TempPclFileName { get; set; }

		public PclToPdfFileInfo() {
			ToIndex = 0;
			FromIndex = 0;
		}

		internal void EnsurePclFile(bool checkFile = false) {
			FilePcl = new System.IO.FileInfo(Path.Combine("./" + VirtualFolder, PclFileName));
			if (checkFile && FilePcl.Exists)
				SourceName = FilePcl.Name;
			else
				SourceName = PclFileName;

		}

		public bool Exists() {
			return !string.IsNullOrEmpty(PdfFileName) &&
				File.Exists(Path.Combine("./" + VirtualFolder, PdfFileName));
		}

	}
}

