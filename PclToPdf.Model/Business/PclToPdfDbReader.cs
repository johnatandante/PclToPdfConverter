using System;
using System.Collections.Generic;
using System.Linq;
using PclToPdf.DataItem;
using PclToPdf.Interfaces;
using PclToPdf.Interfaces;
using Utility.ErmsSqlServerClient;
using Utility.Log;

namespace PclToPdf.Model
{
	public class PclToPdfDbReader : IAnythingAppenedNotifyer
	{
		
		public event EventHandler AnythigHappened;

		public event EventHandler ItemRead;

		public IEnumerable<IPclToPdfFileInfo> ReadAll(IEnumerable<IFilePclInfo> fileAfpList) {

			ErmsDbClassDataContext context = new ErmsDbClassDataContext("Data Source=h2014eul0499a.azgroup.itad.corpnet;Initial Catalog=EMRS;Integrated Security=True");			

			List<IPclToPdfFileInfo> trovati  = new List<IPclToPdfFileInfo>();

			foreach(IFilePclInfo fileItem in fileAfpList) {

				IQueryable<PDF_FROM_PCL> results = from item in context.PDF_FROM_PCLs
							 where item.volume == fileItem.Volume
								&& item.PCL_Filename == fileItem.Filename
								 select item;
				NotifyAnythigHappened("Trovate " + results.Count() + " stampe per " + fileItem.Volume + " - " + fileItem.Filename);

				foreach (PDF_FROM_PCL item in results) {
					trovati.Add(new PdfFromPclFileInfo(fileItem) {
						PdfFileName = item.UID.ToString() + ".pdf",
						FromIndex = item.StartPage.HasValue ? item.StartPage.Value : -1,
						ToIndex = item.EndPage.HasValue ? item.EndPage.Value : -1,						
					});
					
				}

				NotifyItemRead(trovati);
								
			}

			return trovati;

		}

		private void NotifyItemRead(List<IPclToPdfFileInfo> trovati) {
			if (ItemRead != null)
				ItemRead(this, new ItemReadEventArgs(trovati));
		}

		private void NotifyAnythigHappened(string message) {
			if (AnythigHappened != null)
				AnythigHappened(this, new AnythingHappenedEventArgs(message));
		}

		public void CheckPclFatto(List<IFilePclInfo> toDos, List<IPclToPdfFileInfo> list, string pdfDir) {
			foreach (var group in list.GroupBy(p => p.SourceName,
										(key, value) => new { Name = key, Items = value.ToList() })) {
				IFilePclInfo item = toDos.SingleOrDefault(pcl => pcl.Filename.ToLowerInvariant().Equals(group.Name.ToLowerInvariant()));
				if (item == null)
					continue;

				item.Quanti = group.Items.Count;
				item.Done = FilePclToPdfReader.CheckAreDone(group.Items.Where(i => i.Pages > 0), pdfDir);
				item.PdfFiles = group.Items.Select(i => i.PdfFileName).ToArray();
			}
		}
	}
}
 