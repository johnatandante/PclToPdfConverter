using System;
using System.IO;
using PclToPdf.Interfaces;
using Utility.Log;

namespace PclToPdf.Model
{
	public class PclToPdfFileWriter : IAnythingAppenedNotifyer
	{
		string SourcePclDir { get; set; }

		string OutputPdfDir { get; set; }
		
		SwiftCommandLineTool tool = new SwiftCommandLineTool();
		
		public event EventHandler AnythigHappened;

		public PclToPdfFileWriter(string outputPdfDir, string sourcePclDir = null) {
			OutputPdfDir = outputPdfDir;
			SourcePclDir = sourcePclDir ?? OutputPdfDir;

			tool.AnythigHappened += OnAnythigHappened;

		}

		private void OnAnythigHappened(object sender, EventArgs e) {
			if (!(e is AnythingHappenedEventArgs))
				return;

			AnythingHappenedEventArgs args = e as AnythingHappenedEventArgs;
			if (AnythigHappened != null)
				AnythigHappened(sender, args);

		}

		public void Process(IPclToPdfFileInfo item) {
			string filePclDir = Path.Combine(SourcePclDir, item.VirtualFolder);
			string filePdfDir = Path.Combine(OutputPdfDir, item.VirtualFolder);

			string outputfile = tool.ExtractPage(Path.Combine(filePclDir, item.SourceName),
													filePdfDir,
													Path.Combine(filePdfDir, item.PdfFileName.ToLowerInvariant().Replace(".pdf", ".pcl")),
													item.FromIndex,
													item.Pages);

			if (!string.IsNullOrEmpty(outputfile)) {
				// item.PdfFileName
				item.Done = !string.IsNullOrEmpty(tool.ConvertPclToPdf(Path.Combine(filePdfDir, outputfile)));
			}

		}

		[Obsolete]
		internal bool CopyItemTo(IPclToPdfFileInfo item, string outputDir) {
			string outputFolder = Path.Combine(outputDir, item.VirtualFolder);
			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);

			string inputFile = Path.Combine(Path.Combine(OutputPdfDir, item.VirtualFolder), item.PdfFileName);
			string outputFile = Path.Combine(outputFolder, item.PdfFileName);

			if (File.Exists(outputFile))
				return false;
			else
				return Copy(inputFile, outputFile);

		}

		private static bool Copy(string input, string output) {
			File.Copy(input, output);
			return true;
		}

		internal static void EnsureDir(string virtualFolder, string outputDir) {
			string outputFolder = Path.Combine(outputDir, virtualFolder);
			if (!Directory.Exists(outputFolder))
				Directory.CreateDirectory(outputFolder);
		}

		public static string Export(IPclToPdfFileInfo[] _FileList, string outputPath = "") {
			string filename = "ExportPdfList" + DateTime.Now.Ticks + ".csv";
			
			if (!string.IsNullOrEmpty(outputPath) && !Directory.Exists(outputPath))
				Directory.CreateDirectory(outputPath);

			using (StreamWriter writer = new StreamWriter(Path.Combine(outputPath, filename))) {
				writer.AutoFlush = true;

				// header
				writer.WriteLine(string.Join(",", new string[] { "Volume", "Sorgente", "Filename", "Indice", "Pagine", "Stato" }));
				foreach (IPclToPdfFileInfo item in _FileList)
					writer.WriteLine(string.Join(",", new string[] { item.VirtualFolder, item.SourceName, item.PdfFileName, item.FromIndex.ToString(), item.Pages.ToString(), item.Done ? "Ok" : "Errore" }));

			}
			
			return filename;
		}
		
	}
}
