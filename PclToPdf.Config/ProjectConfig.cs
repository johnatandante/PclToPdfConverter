using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using PclToPdf.Interfaces;

namespace PclToPdf.Config
{
	[DataContract]
	public class ProjectConfig
	{

		[DataMember]
		public List<FilePclConfig> Pcls = new List<FilePclConfig>();
		
		[DataMember]
		public long LastModified = DateTime.Now.Ticks;

		[DataMember]
		public long Created = DateTime.Now.Ticks;

	}

	//[DataContract]
	//public class FilePclConfigFilePclConfig : IFilePclInfo
	//{
	//	[DataMember]
	//	public string Volume { get; set; }

	//	[DataMember]
	//	public string Filename { get; set; }

	//	[DataMember]
	//	public bool Done { get; set; }

	//	[DataMember]
	//	public int Quanti { get; set; }
		
	//	public bool ToDo { get; set; }

	//	[DataMember]
	//	public string[] PdfFiles { get; set; }
		
	//}

}
