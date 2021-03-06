﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Utility.ErmsSqlServerClient
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="EMRS")]
	public partial class ErmsDbClassDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public ErmsDbClassDataContext() : 
				base(global::Utility.ErmsSqlServerClient.Properties.Settings.Default.EMRSConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public ErmsDbClassDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ErmsDbClassDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ErmsDbClassDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public ErmsDbClassDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<V_PDF_FROM_PCL> V_PDF_FROM_PCLs
		{
			get
			{
				return this.GetTable<V_PDF_FROM_PCL>();
			}
		}
		
		public System.Data.Linq.Table<PDF_FROM_PCL> PDF_FROM_PCLs
		{
			get
			{
				return this.GetTable<PDF_FROM_PCL>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.V_PDF_FROM_PCL")]
	public partial class V_PDF_FROM_PCL
	{
		
		private string _folder_PCL;
		
		private string _PCLFilename;
		
		private System.Guid _UID;
		
		private string _tipo_contr_ide;
		
		private System.Nullable<int> _conv_ide;
		
		private System.Nullable<long> _scheda_ide;
		
		private System.Nullable<int> _progr_scheda_ide;
		
		private System.Nullable<short> _cod_comp;
		
		private System.Nullable<System.Guid> _UIDFascicoloInformativo;
		
		private System.Nullable<System.DateTime> _DATECREATED;
		
		private bool _ISOFFLINE;
		
		private System.Guid _CLASSID;
		
		private string _NAME;
		
		private string _DESCRIPTION;
		
		private System.Guid _StreamID;
		
		private string _Extension;
		
		private string _VirtualFolder;
		
		private string _Filename;
		
		private System.Guid _ReferenceID;
		
		private System.Nullable<int> _StartPage;
		
		private System.Nullable<int> _EndPage;
		
		private System.Nullable<long> _Num_Doc;
		
		private string _TLE_IDENTIFICATIVO;
		
		private string _Recipient;
		
		private string _FullDescription;
		
		private System.Nullable<int> _CodComp;
		
		private System.Nullable<int> _TipoCtr;
		
		private System.Nullable<int> _CodRamo;
		
		private System.Nullable<long> _NumCtr;
		
		public V_PDF_FROM_PCL()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_folder_PCL", DbType="VarChar(6000) NOT NULL", CanBeNull=false)]
		public string folder_PCL
		{
			get
			{
				return this._folder_PCL;
			}
			set
			{
				if ((this._folder_PCL != value))
				{
					this._folder_PCL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PCLFilename", DbType="VarChar(1024) NOT NULL", CanBeNull=false)]
		public string PCLFilename
		{
			get
			{
				return this._PCLFilename;
			}
			set
			{
				if ((this._PCLFilename != value))
				{
					this._PCLFilename = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				if ((this._UID != value))
				{
					this._UID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_tipo_contr_ide", DbType="Char(3)")]
		public string tipo_contr_ide
		{
			get
			{
				return this._tipo_contr_ide;
			}
			set
			{
				if ((this._tipo_contr_ide != value))
				{
					this._tipo_contr_ide = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_conv_ide", DbType="Int")]
		public System.Nullable<int> conv_ide
		{
			get
			{
				return this._conv_ide;
			}
			set
			{
				if ((this._conv_ide != value))
				{
					this._conv_ide = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_scheda_ide", DbType="BigInt")]
		public System.Nullable<long> scheda_ide
		{
			get
			{
				return this._scheda_ide;
			}
			set
			{
				if ((this._scheda_ide != value))
				{
					this._scheda_ide = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_progr_scheda_ide", DbType="Int")]
		public System.Nullable<int> progr_scheda_ide
		{
			get
			{
				return this._progr_scheda_ide;
			}
			set
			{
				if ((this._progr_scheda_ide != value))
				{
					this._progr_scheda_ide = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_cod_comp", DbType="SmallInt")]
		public System.Nullable<short> cod_comp
		{
			get
			{
				return this._cod_comp;
			}
			set
			{
				if ((this._cod_comp != value))
				{
					this._cod_comp = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UIDFascicoloInformativo", DbType="UniqueIdentifier")]
		public System.Nullable<System.Guid> UIDFascicoloInformativo
		{
			get
			{
				return this._UIDFascicoloInformativo;
			}
			set
			{
				if ((this._UIDFascicoloInformativo != value))
				{
					this._UIDFascicoloInformativo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DATECREATED", DbType="DateTime")]
		public System.Nullable<System.DateTime> DATECREATED
		{
			get
			{
				return this._DATECREATED;
			}
			set
			{
				if ((this._DATECREATED != value))
				{
					this._DATECREATED = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ISOFFLINE", DbType="Bit NOT NULL")]
		public bool ISOFFLINE
		{
			get
			{
				return this._ISOFFLINE;
			}
			set
			{
				if ((this._ISOFFLINE != value))
				{
					this._ISOFFLINE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CLASSID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid CLASSID
		{
			get
			{
				return this._CLASSID;
			}
			set
			{
				if ((this._CLASSID != value))
				{
					this._CLASSID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NAME", DbType="VarChar(50)")]
		public string NAME
		{
			get
			{
				return this._NAME;
			}
			set
			{
				if ((this._NAME != value))
				{
					this._NAME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DESCRIPTION", DbType="VarChar(250)")]
		public string DESCRIPTION
		{
			get
			{
				return this._DESCRIPTION;
			}
			set
			{
				if ((this._DESCRIPTION != value))
				{
					this._DESCRIPTION = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StreamID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid StreamID
		{
			get
			{
				return this._StreamID;
			}
			set
			{
				if ((this._StreamID != value))
				{
					this._StreamID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Extension", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string Extension
		{
			get
			{
				return this._Extension;
			}
			set
			{
				if ((this._Extension != value))
				{
					this._Extension = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VirtualFolder", DbType="VarChar(6000) NOT NULL", CanBeNull=false)]
		public string VirtualFolder
		{
			get
			{
				return this._VirtualFolder;
			}
			set
			{
				if ((this._VirtualFolder != value))
				{
					this._VirtualFolder = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Filename", DbType="VarChar(1024) NOT NULL", CanBeNull=false)]
		public string Filename
		{
			get
			{
				return this._Filename;
			}
			set
			{
				if ((this._Filename != value))
				{
					this._Filename = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ReferenceID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid ReferenceID
		{
			get
			{
				return this._ReferenceID;
			}
			set
			{
				if ((this._ReferenceID != value))
				{
					this._ReferenceID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StartPage", DbType="Int")]
		public System.Nullable<int> StartPage
		{
			get
			{
				return this._StartPage;
			}
			set
			{
				if ((this._StartPage != value))
				{
					this._StartPage = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EndPage", DbType="Int")]
		public System.Nullable<int> EndPage
		{
			get
			{
				return this._EndPage;
			}
			set
			{
				if ((this._EndPage != value))
				{
					this._EndPage = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Num_Doc", DbType="BigInt")]
		public System.Nullable<long> Num_Doc
		{
			get
			{
				return this._Num_Doc;
			}
			set
			{
				if ((this._Num_Doc != value))
				{
					this._Num_Doc = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TLE_IDENTIFICATIVO", DbType="VarChar(322)")]
		public string TLE_IDENTIFICATIVO
		{
			get
			{
				return this._TLE_IDENTIFICATIVO;
			}
			set
			{
				if ((this._TLE_IDENTIFICATIVO != value))
				{
					this._TLE_IDENTIFICATIVO = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Recipient", DbType="VarChar(100)")]
		public string Recipient
		{
			get
			{
				return this._Recipient;
			}
			set
			{
				if ((this._Recipient != value))
				{
					this._Recipient = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FullDescription", DbType="VarChar(4096)")]
		public string FullDescription
		{
			get
			{
				return this._FullDescription;
			}
			set
			{
				if ((this._FullDescription != value))
				{
					this._FullDescription = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CodComp", DbType="Int")]
		public System.Nullable<int> CodComp
		{
			get
			{
				return this._CodComp;
			}
			set
			{
				if ((this._CodComp != value))
				{
					this._CodComp = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TipoCtr", DbType="Int")]
		public System.Nullable<int> TipoCtr
		{
			get
			{
				return this._TipoCtr;
			}
			set
			{
				if ((this._TipoCtr != value))
				{
					this._TipoCtr = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CodRamo", DbType="Int")]
		public System.Nullable<int> CodRamo
		{
			get
			{
				return this._CodRamo;
			}
			set
			{
				if ((this._CodRamo != value))
				{
					this._CodRamo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NumCtr", DbType="BigInt")]
		public System.Nullable<long> NumCtr
		{
			get
			{
				return this._NumCtr;
			}
			set
			{
				if ((this._NumCtr != value))
				{
					this._NumCtr = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="CMOD.PDF_FROM_PCL")]
	public partial class PDF_FROM_PCL
	{
		
		private string _volume;
		
		private string _filename;
		
		private string _esito;
		
		private string _PCL_Filename;
		
		private string _Extension;
		
		private System.Guid _UID;
		
		private System.Nullable<int> _StartPage;
		
		private System.Nullable<int> _EndPage;
		
		public PDF_FROM_PCL()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_volume", DbType="VarChar(7) NOT NULL", CanBeNull=false)]
		public string volume
		{
			get
			{
				return this._volume;
			}
			set
			{
				if ((this._volume != value))
				{
					this._volume = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_filename", DbType="VarChar(50)")]
		public string filename
		{
			get
			{
				return this._filename;
			}
			set
			{
				if ((this._filename != value))
				{
					this._filename = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_esito", DbType="VarChar(6) NOT NULL", CanBeNull=false)]
		public string esito
		{
			get
			{
				return this._esito;
			}
			set
			{
				if ((this._esito != value))
				{
					this._esito = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PCL_Filename", DbType="VarChar(1024) NOT NULL", CanBeNull=false)]
		public string PCL_Filename
		{
			get
			{
				return this._PCL_Filename;
			}
			set
			{
				if ((this._PCL_Filename != value))
				{
					this._PCL_Filename = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Extension", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string Extension
		{
			get
			{
				return this._Extension;
			}
			set
			{
				if ((this._Extension != value))
				{
					this._Extension = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				if ((this._UID != value))
				{
					this._UID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StartPage", DbType="Int")]
		public System.Nullable<int> StartPage
		{
			get
			{
				return this._StartPage;
			}
			set
			{
				if ((this._StartPage != value))
				{
					this._StartPage = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EndPage", DbType="Int")]
		public System.Nullable<int> EndPage
		{
			get
			{
				return this._EndPage;
			}
			set
			{
				if ((this._EndPage != value))
				{
					this._EndPage = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
