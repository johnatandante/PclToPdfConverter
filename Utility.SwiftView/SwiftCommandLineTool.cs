using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Utility.Log
{
	/// <summary>
	/// Summary description for SWTools.
	/// </summary>
	public class SwiftCommandLineTool
	{
		static string ExtractorEXE = "spext.exe";
		static string ConverterEXE = "sview.exe";
		static string SwiftViewLicenseKey = "0001hamkgieqeqg";

		public event EventHandler AnythigHappened;

		#region Costruttori/Distruttori
		public SwiftCommandLineTool() {
			
		}
		#endregion Costruttori/Distruttori

		#region Metodi Pubblici		
		public string ExtractPage(string pcl_infile, string outputDir, string outputPclPortionFileName, long startPage, long numofpagetoextract) {
			FileInfo f = new FileInfo(pcl_infile);
			if (!f.Exists)
				return string.Empty;

			if (!Directory.Exists(outputDir))
				Directory.CreateDirectory(outputDir);

			string outfile = Path.Combine(outputDir, outputPclPortionFileName);
			if (!ExtractPage(ExtractorEXE, SwiftViewLicenseKey, pcl_infile, outfile, startPage, numofpagetoextract))
				return string.Empty;

			return outfile;
			
		}

		public string ConvertAndExtractPage(string file, long startPage, long numofpagetoextract, string watermark, bool EBCDIC, short res) {
			string infile = file;
			string tempfile = file.ToLowerInvariant().Replace(".pcl", "_from_" + startPage + "_for_" + numofpagetoextract + ".pcl");
			string outfile = file.ToLowerInvariant().Replace(".pcl", ".pdf");
			if (ExtractPage(ExtractorEXE, SwiftViewLicenseKey, infile, tempfile, startPage, numofpagetoextract)) {
				if (ConvertToPdf(ConverterEXE, tempfile, outfile, watermark, EBCDIC, res))
					return outfile;
				File.Delete(tempfile);
			}

			return string.Empty;
		}

		public string ConvertPclToPdf(string pclFile) {
			string infile = pclFile;
			string outfile = pclFile.ToLowerInvariant().Replace(".pcl", ".pdf");
			if (!ConvertToPdf(ConverterEXE, infile, outfile, string.Empty, true, (short)600))
				return string.Empty;

			File.Delete(pclFile);

			return outfile;

		}

		#endregion Metodi Pubblici

		#region Implementazioni Protette

		protected bool ExtractPage(string ExtractorEXE, string SwiftViewLicenseKey, string infile, string outfile, long startPage, long numofpagetoextract) {
			if (ExtractorEXE == null || SwiftViewLicenseKey == null) {
				string msg = String.Format("Invalid SwiftView cfg : [exe] = {0}, [license] = {1}", ExtractorEXE, SwiftViewLicenseKey);
				Notify(msg);
				throw new NotSupportedException(msg);
			}
			string PclCommand = EstractPCL_Command(SwiftViewLicenseKey, infile, outfile, startPage, numofpagetoextract);
			Notify("Pclcommand: " + PclCommand);

			Process a = System.Diagnostics.Process.Start(ExtractorEXE, PclCommand);
			a.StartInfo.CreateNoWindow = true;
			a.WaitForExit();
			
			if (File.Exists(outfile))
				return true;			

			Notify(string.Format("Error extracting with {3} from file {0} StartPage : {1} Num of Pages to Extract : {2}", infile, startPage, numofpagetoextract, ExtractorEXE));

			return false;
		}

		protected bool ConvertToPdf(string ConverterEXE, string pclfile, string pdffile, string watermark, bool EBCDIC, short res) {
			if (ConverterEXE == null) {
				string msg = "Invalid SwiftView cfg : [convexe] = " + ConverterEXE;
				Notify(msg);
				throw new NotSupportedException(msg);
			}

			Process a = System.Diagnostics.Process.Start(ConverterEXE, ConvertToPdf_Command(pclfile, pdffile, watermark, EBCDIC, res));
			a.StartInfo.CreateNoWindow = true;			
			a.WaitForExit();
			if (File.Exists(pdffile))
				return true;
			Notify(string.Format("Error converting file {0} to pdf", pclfile));
			return false;
		}
		
		protected string ConvertToPdf_Command(string pclfile, string pdffile, string watermark, bool EBCDIC, short res) {
			StringBuilder sb = new StringBuilder();
			sb.Append(" -v1 -p -c\"set quotechar 0x5e ");
			sb.Append(" | ldoc ^" + pclfile + "^");
			#region EBCDIC
			if (EBCDIC) {
				sb.Append(" | set select charset EBCDIC ");
			}
			#endregion EBCDIC
			#region Watermark
			if (watermark != null && watermark.Length > 0) {
				sb.Append(" | markup attributes fgcolor rgb:c0/c0/c0 ");
				sb.Append(" | markup attributes drawmode transparent ");
				sb.Append(" | markup text font ^face cour size 42^ ");
				#region watermark2print
				string watermark2print = watermark.Trim() + " ";
				int lenghtOfstring2print = 80;
				int index_ini = 0;
				string string2print= "";
				//quanti carateri in tutto nella pagina
				int number_of_line_per_page = 10;
				int total_char_of_the_page = lenghtOfstring2print * number_of_line_per_page;
				int number_of_watermark_s_iteration = total_char_of_the_page / watermark2print.Length + 1;
				int i;
				//fill the string
				for (i = 1; i <= number_of_watermark_s_iteration; i++) {
					string2print += watermark2print;
				}
				for (i = 0; i < number_of_line_per_page; i++) {
					index_ini = i * lenghtOfstring2print;
					sb.Append(" | onpage all markup text rxloc 0.2 ryloc " + i + " string ^" + string2print.Substring(index_ini, lenghtOfstring2print) + "^");
				}
				#endregion watermarl2print
			}
			#endregion watermark
			#region resolution
			if (res != 0) {
				sb.Append(" | set printres " + res);
			}
			#endregion resolution
			sb.Append(" | save PDF All ^" + pdffile + "^ onefile\"");

			return sb.ToString();
		}

		protected string EstractPCL_Command(string SwiftViewLicenseKey, string infile, string outfile, long startPage, long numPages) {
			return string.Format(@"-f{0} -l{4} -p{2} -n{3} -o{1} -c", infile, outfile, startPage, numPages, SwiftViewLicenseKey);
		}
		#endregion Implementazioni Protette

		protected void Notify(string message) {
			if (AnythigHappened != null)
				AnythigHappened(this, new AnythingHappenedEventArgs(message));
		}

	}

}

