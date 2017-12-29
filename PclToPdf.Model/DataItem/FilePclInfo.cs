using System.Collections.Generic;
using PclToPdf.Interfaces;

namespace PclToPdf.DataItem
{
	public class FilePclInfo : IFilePclInfo
	{

		public string Volume { get; set; }
		public string Filename { get; set; }

		public bool ToDo { get; set; }
		public bool Done { get; set; }

		public bool NotDone {
			get {
				return !Done; 
			}
		}

		public int Quanti { get; set; }
		
		public string[] PdfFiles { get; set; }

		public FilePclInfo(){
			ToDo = true;
			PdfFiles = new string[] { };
		}

		public FilePclInfo(IFilePclInfo item) {
			Filename = item.Filename;
			Volume = item.Volume;
			Done = item.Done;
			Quanti = item.Quanti;
			ToDo = !item.Done && Quanti > 0;
			PdfFiles = item.PdfFiles == null ? new string[] { } : item.PdfFiles;

		}


	}
}
