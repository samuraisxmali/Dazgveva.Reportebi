﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dazgveva.Reportebi.Models
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Pirvelckaroebi")]
	public partial class Pirvelckaroebi2DataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public Pirvelckaroebi2DataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["PirvelckaroebiConnectionString1"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public Pirvelckaroebi2DataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Pirvelckaroebi2DataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Pirvelckaroebi2DataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public Pirvelckaroebi2DataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<VDeklaraciebisIstoria> VDeklaraciebisIstorias
		{
			get
			{
				return this.GetTable<VDeklaraciebisIstoria>();
			}
		}
		
		public System.Data.Linq.Table<vForm_3> vForm_3s
		{
			get
			{
				return this.GetTable<vForm_3>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.VDeklaraciebisIstoria")]
	public partial class VDeklaraciebisIstoria
	{
		
		private string _FID;
		
		private string _PID;
		
		private string _LAST_NAME;
		
		private string _FIRST_NAME;
		
		private System.DateTime _BIRTH_DATE;
		
		private string _FULL_ADDRESS;
		
		private int _FID_VERSION;
		
		private string _HOME_PHONE;
		
		private string _MOB_PHONE;
		
		private System.Nullable<System.DateTime> _CALC_DATE;
		
		private System.Nullable<System.DateTime> _LEGAL_SCORE_DATE;
		
		private System.Nullable<System.DateTime> _RESTORE_DOC_DATE;
		
		private bool _ON_CONTROL;
		
		private System.Nullable<int> _ACTION_TYPE;
		
		public VDeklaraciebisIstoria()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FID", DbType="Char(12) NOT NULL", CanBeNull=false)]
		public string FID
		{
			get
			{
				return this._FID;
			}
			set
			{
				if ((this._FID != value))
				{
					this._FID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PID", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
		public string PID
		{
			get
			{
				return this._PID;
			}
			set
			{
				if ((this._PID != value))
				{
					this._PID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LAST_NAME", DbType="NVarChar(1000)")]
		public string LAST_NAME
		{
			get
			{
				return this._LAST_NAME;
			}
			set
			{
				if ((this._LAST_NAME != value))
				{
					this._LAST_NAME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FIRST_NAME", DbType="NVarChar(1000)")]
		public string FIRST_NAME
		{
			get
			{
				return this._FIRST_NAME;
			}
			set
			{
				if ((this._FIRST_NAME != value))
				{
					this._FIRST_NAME = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BIRTH_DATE", DbType="SmallDateTime NOT NULL")]
		public System.DateTime BIRTH_DATE
		{
			get
			{
				return this._BIRTH_DATE;
			}
			set
			{
				if ((this._BIRTH_DATE != value))
				{
					this._BIRTH_DATE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FULL_ADDRESS", DbType="NVarChar(1000)")]
		public string FULL_ADDRESS
		{
			get
			{
				return this._FULL_ADDRESS;
			}
			set
			{
				if ((this._FULL_ADDRESS != value))
				{
					this._FULL_ADDRESS = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FID_VERSION", DbType="Int NOT NULL")]
		public int FID_VERSION
		{
			get
			{
				return this._FID_VERSION;
			}
			set
			{
				if ((this._FID_VERSION != value))
				{
					this._FID_VERSION = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HOME_PHONE", DbType="VarChar(20)")]
		public string HOME_PHONE
		{
			get
			{
				return this._HOME_PHONE;
			}
			set
			{
				if ((this._HOME_PHONE != value))
				{
					this._HOME_PHONE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MOB_PHONE", DbType="VarChar(20)")]
		public string MOB_PHONE
		{
			get
			{
				return this._MOB_PHONE;
			}
			set
			{
				if ((this._MOB_PHONE != value))
				{
					this._MOB_PHONE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CALC_DATE", DbType="SmallDateTime")]
		public System.Nullable<System.DateTime> CALC_DATE
		{
			get
			{
				return this._CALC_DATE;
			}
			set
			{
				if ((this._CALC_DATE != value))
				{
					this._CALC_DATE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LEGAL_SCORE_DATE", DbType="DateTime")]
		public System.Nullable<System.DateTime> LEGAL_SCORE_DATE
		{
			get
			{
				return this._LEGAL_SCORE_DATE;
			}
			set
			{
				if ((this._LEGAL_SCORE_DATE != value))
				{
					this._LEGAL_SCORE_DATE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RESTORE_DOC_DATE", DbType="SmallDateTime")]
		public System.Nullable<System.DateTime> RESTORE_DOC_DATE
		{
			get
			{
				return this._RESTORE_DOC_DATE;
			}
			set
			{
				if ((this._RESTORE_DOC_DATE != value))
				{
					this._RESTORE_DOC_DATE = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ON_CONTROL", DbType="Bit NOT NULL")]
		public bool ON_CONTROL
		{
			get
			{
				return this._ON_CONTROL;
			}
			set
			{
				if ((this._ON_CONTROL != value))
				{
					this._ON_CONTROL = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ACTION_TYPE", DbType="Int")]
		public System.Nullable<int> ACTION_TYPE
		{
			get
			{
				return this._ACTION_TYPE;
			}
			set
			{
				if ((this._ACTION_TYPE != value))
				{
					this._ACTION_TYPE = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.vForm_3")]
	public partial class vForm_3
	{
		
		private string _FID;
		
		private System.DateTime _DT;
		
		private string _Mizezi;
		
		public vForm_3()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FID", DbType="Char(12) NOT NULL", CanBeNull=false)]
		public string FID
		{
			get
			{
				return this._FID;
			}
			set
			{
				if ((this._FID != value))
				{
					this._FID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DT", DbType="SmallDateTime NOT NULL")]
		public System.DateTime DT
		{
			get
			{
				return this._DT;
			}
			set
			{
				if ((this._DT != value))
				{
					this._DT = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Mizezi", DbType="NVarChar(1000)")]
		public string Mizezi
		{
			get
			{
				return this._Mizezi;
			}
			set
			{
				if ((this._Mizezi != value))
				{
					this._Mizezi = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
