
using System.ComponentModel;

namespace PclToPdf.DataItem
{
	public class SortByRecord
	{
		public string Field { get; set; }
		public ListSortDirection SortMode { get; set; }

		public override string ToString() {
			return Field + " - " + SortMode.ToString();
		}
	}
}
