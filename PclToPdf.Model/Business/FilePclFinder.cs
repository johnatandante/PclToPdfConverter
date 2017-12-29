using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PclToPdf.Config;
using PclToPdf.DataItem;
using PclToPdf.Interfaces;
using Utility.ErmsSqlServerClient;

namespace PclToPdf.Model
{
	public class FilePclFinder
	{
		public enum Volume {
			Volume1,
			Volume2,
			Volume3,
			Volume4,
			Volume5,
			Volume6,
			Volume7,
			Volume8,
		}

		const string pclFilter = "*.pcl";

		string CurrentDir { get; set; }
		
		public FilePclFinder(string currentDir = "") {
			CurrentDir = string.IsNullOrEmpty(currentDir) ?
				Directory.GetCurrentDirectory() : currentDir;
			
		}

		[Obsolete]
		public List<IFilePclInfo> GetAllFileAfpFromCurrentDirectory(Volume volume) {

			List<IFilePclInfo> list = new List<IFilePclInfo>();
			string workingDir = Path.Combine(CurrentDir, volume.ToString());
			if (!Directory.Exists(workingDir))
				return list;

			string[] filePathArray = Directory.GetFiles(workingDir, pclFilter, SearchOption.AllDirectories);

			foreach (string filename in filePathArray) {
				FileInfo f = new FileInfo(filename);
				if (f.Name.StartsWith("."))
					continue;

				IFilePclInfo item = new FilePclInfo() { Volume = f.Directory.Name, Filename = f.Name };
				list.Add(item);

			}

			return list;
		}

		public IEnumerable<IFilePclInfo> ReadFromConfig(ConfigManager config) {
			List<IFilePclInfo> list = new List<IFilePclInfo>();
			foreach (IFilePclInfo item in config.GetAllPcls()) {
				list.Add(new FilePclInfo(item));

			}

			return list;
		}

		public static string ExportFromConfig(ConfigManager config, string outputPath = "") {
			string filename = "ExportPclList" + DateTime.Now.Ticks + ".csv";

			if (!string.IsNullOrEmpty(outputPath) && !Directory.Exists(outputPath))
				Directory.CreateDirectory(outputPath);

			using (StreamWriter writer = new StreamWriter(Path.Combine(outputPath, filename))) {
				writer.AutoFlush = true;

				// header
				writer.WriteLine(string.Join(",", new string[] {"Volume", "Filename", "Pdf_Trovati", "Stato" }));
				foreach (IFilePclInfo item in config.GetAllPcls()) 
					writer.WriteLine(string.Join(",", new string[] {item.Volume, item.Filename, item.Quanti.ToString(), item.Done ?  "Completo"  : "Incompleto" }));
				
			}

			return filename;
		}

		public List<IFilePclInfo> GetAllFileAfpFromDb(Volume volume) {
			List<IFilePclInfo> list = new List<IFilePclInfo>();

			ErmsDbClassDataContext context = new ErmsDbClassDataContext("Data Source=h2014eul0499a.azgroup.itad.corpnet;Initial Catalog=EMRS;Integrated Security=True");
			IQueryable<string> results = (from item in context.PDF_FROM_PCLs
						   where item.volume == volume.ToString()
						   select item.PCL_Filename).Distinct();

			foreach (string fileName in results) {
				IFilePclInfo item = new FilePclInfo() {
					Volume = volume.ToString(),
					Filename = fileName					
				};

				list.Add(item);

			}

			return list;
		
		
		}
	}
}
