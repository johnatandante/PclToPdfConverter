using System;
using System.Collections.Generic;
using PclToPdf.Interfaces;

namespace PclToPdf.Model
{
	class ItemReadEventArgs: EventArgs
	{
		public IEnumerable<IPclToPdfFileInfo> Items { get; set; }

		public ItemReadEventArgs(IEnumerable<IPclToPdfFileInfo> items) {
			
			Items = items;

		}
	}
}
