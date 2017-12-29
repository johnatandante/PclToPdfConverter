using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PclToPdf.DataItem;
using PclToPdf.Interfaces;

namespace PclToPdf.Model
{
	public class FilePclToPdfReader
	{

		enum Header : short
		{
			virtualfolder = 0,
			pcl,
			uid,
			startpage,
			endpage,
		}

		const char separator = ';';

		Dictionary<Header, int> Indexes = new Dictionary<Header, int>();
		
		public event EventHandler ReadLine;

		public long TotalBytes { get; private set; }
		public long BytesRead { get; private set; }

		public List<PclToPdfFileInfo> ReadFile(string filePath) {
			List<PclToPdfFileInfo> list = new List<PclToPdfFileInfo>();
			if (!File.Exists(filePath))
				return list;

			TotalBytes = (new FileInfo(filePath)).Length;			
			using(StreamReader reader = new StreamReader(filePath)) {
				BytesRead = 0;
				string buffer;

				if (!reader.EndOfStream) {
					buffer = reader.ReadLine();					
					ProcessHeader(buffer);
				}

				while (!reader.EndOfStream) {
					buffer = reader.ReadLine();
					Notify(buffer.Length);
					list.Add(ProcessRow(buffer));
				}

			}

			Notify(TotalBytes);

			return list;
		}

		private void Notify(long l) {
			BytesRead += l;

			if (ReadLine != null)
				ReadLine(this, new EventArgs());
		}

		private PclToPdfFileInfo ProcessRow(string row) {
			PclToPdfFileInfo fileitem  = new PclToPdfFileInfo();
			string[] items = row.Split(separator);

			foreach (Header key in Indexes.Keys) {
				switch (key) {
					case Header.virtualfolder:
						fileitem.VirtualFolder = items[(short)key];
						break;
					case Header.pcl:
						fileitem.PclFileName = items[(short)key];
						break;
					case Header.uid:
						fileitem.PdfFileName = items[(short)key] + ".pdf";
						fileitem.TempPclFileName = items[(short)key] + ".pcl";
						break;
					case Header.startpage:
						fileitem.FromIndex = int.Parse(items[(short)key]);
						break;
					case Header.endpage:
						fileitem.ToIndex = int.Parse(items[(short)key]);
						break;
				}
			}

			fileitem.EnsurePclFile(false);
			
			return fileitem;
		}

		private void ProcessHeader(string row) {
			Indexes.Clear();
			string[] enumHeaders = Enum.GetNames(typeof(Header));

			foreach(string item in row.Split(separator)){
				if (!enumHeaders.Any(i => i == item.ToLowerInvariant()))
					continue;

				Header itemEnum = (Header) Enum.Parse(typeof(Header), item.ToLowerInvariant());
				Indexes.Add(itemEnum, (short)itemEnum);
				
			}

		}

		public List<FileInfo> GetFileInfoList(string direcotry, string filter = "*.csv") {
			List<FileInfo> list = new List<FileInfo>();
			if (!Directory.Exists(direcotry))
				return list;

			foreach (string filename in Directory.GetFiles(direcotry, filter)) {
				// scarto i file "nascosti"
				if (filename.StartsWith("."))
					continue;

				list.Add(new FileInfo(filename));
			}

			return list;

		}

		public static bool CheckAreDone(IEnumerable<IPclToPdfFileInfo> list, string workingDir) {
			bool check = true;
			foreach(IPclToPdfFileInfo item in list){
				string fileWorkingDir = Path.Combine(workingDir, item.VirtualFolder);
				item.Done = File.Exists(Path.Combine(fileWorkingDir, item.PdfFileName));
				check = check && item.Done;
			}

			return check;
		}

		internal static bool Exists(IPclToPdfFileInfo item, string workingDir) {
			string fileWorkingDir = Path.Combine(workingDir, item.VirtualFolder);
			return File.Exists(Path.Combine(fileWorkingDir, item.PdfFileName));

		}

		public static void Open(IPclToPdfFileInfo item, string workingDir) {
			string fileWorkingDir = Path.Combine(workingDir, item.VirtualFolder);
			System.Diagnostics.Process.Start(Path.Combine(fileWorkingDir, item.PdfFileName));

		}
	}
}
