﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
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
    partial void InsertSource_Data(Source_Data instance);
    partial void UpdateSource_Data(Source_Data instance);
    partial void DeleteSource_Data(Source_Data instance);
    partial void InsertPirvelckaro_01_UMCEOEBI(Pirvelckaro_01_UMCEOEBI instance);
    partial void UpdatePirvelckaro_01_UMCEOEBI(Pirvelckaro_01_UMCEOEBI instance);
    partial void DeletePirvelckaro_01_UMCEOEBI(Pirvelckaro_01_UMCEOEBI instance);
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
		
		public System.Data.Linq.Table<Source_Data> Source_Datas
		{
			get
			{
				return this.GetTable<Source_Data>();
			}
		}
		
		public System.Data.Linq.Table<Pirvelckaro_01_UMCEOEBI> Pirvelckaro_01_UMCEOEBIs
		{
			get
			{
				return this.GetTable<Pirvelckaro_01_UMCEOEBI>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Source_Data")]
	public partial class Source_Data : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _Pirvelckaro;
		
		private int _Base_Type;
		
		private System.Nullable<int> _Source_Rec_Id;
		
		private System.DateTime _Periodi;
		
		private System.DateTime _MapDate;
		
		private string _FID;
		
		private System.Nullable<int> _Unnom;
		
		private int _UnnomisKhariskhi;
		
		private string _PID;
		
		private string _First_Name;
		
		private string _Last_Name;
		
		private System.Nullable<System.DateTime> _Birth_Date;
		
		private System.Nullable<int> _Sex;
		
		private string _IdentPID;
		
		private System.Nullable<int> _J_ID;
		
		private System.Nullable<int> _Piroba;
		
		private string _Rai;
		
		private string _Region_Name;
		
		private string _Rai_Name;
		
		private string _City;
		
		private string _Village;
		
		private string _Street;
		
		private string _Full_Address;
		
		private string _Dacesebuleba;
		
		private string _Dac_Region_Name;
		
		private string _Dac_Rai_Name;
		
		private string _Dac_City;
		
		private string _Dac_Village;
		
		private string _Dac_Full_Address;
		
		private string _CONDITION_DESCRIPTION;
		
		private System.Nullable<long> _CONDITION_ID;
		
		private System.Nullable<bool> _GaukmebuliPid;
		
		private EntitySet<Pirvelckaro_01_UMCEOEBI> _Pirvelckaro_01_UMCEOEBIs;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnPirvelckaroChanging(string value);
    partial void OnPirvelckaroChanged();
    partial void OnBase_TypeChanging(int value);
    partial void OnBase_TypeChanged();
    partial void OnSource_Rec_IdChanging(System.Nullable<int> value);
    partial void OnSource_Rec_IdChanged();
    partial void OnPeriodiChanging(System.DateTime value);
    partial void OnPeriodiChanged();
    partial void OnMapDateChanging(System.DateTime value);
    partial void OnMapDateChanged();
    partial void OnFIDChanging(string value);
    partial void OnFIDChanged();
    partial void OnUnnomChanging(System.Nullable<int> value);
    partial void OnUnnomChanged();
    partial void OnUnnomisKhariskhiChanging(int value);
    partial void OnUnnomisKhariskhiChanged();
    partial void OnPIDChanging(string value);
    partial void OnPIDChanged();
    partial void OnFIRST_NAMEChanging(string value);
    partial void OnFIRST_NAMEChanged();
    partial void OnLAST_NAMEChanging(string value);
    partial void OnLAST_NAMEChanged();
    partial void OnBIRTH_DATEChanging(System.Nullable<System.DateTime> value);
    partial void OnBIRTH_DATEChanged();
    partial void OnSexChanging(System.Nullable<int> value);
    partial void OnSexChanged();
    partial void OnIdentPIDChanging(string value);
    partial void OnIdentPIDChanged();
    partial void OnJ_IDChanging(System.Nullable<int> value);
    partial void OnJ_IDChanged();
    partial void OnPirobaChanging(System.Nullable<int> value);
    partial void OnPirobaChanged();
    partial void OnRaiChanging(string value);
    partial void OnRaiChanged();
    partial void OnRegion_NameChanging(string value);
    partial void OnRegion_NameChanged();
    partial void OnRai_NameChanging(string value);
    partial void OnRai_NameChanged();
    partial void OnCityChanging(string value);
    partial void OnCityChanged();
    partial void OnVillageChanging(string value);
    partial void OnVillageChanged();
    partial void OnStreetChanging(string value);
    partial void OnStreetChanged();
    partial void OnFull_AddressChanging(string value);
    partial void OnFull_AddressChanged();
    partial void OnDacesebulebaChanging(string value);
    partial void OnDacesebulebaChanged();
    partial void OnDac_Region_NameChanging(string value);
    partial void OnDac_Region_NameChanged();
    partial void OnDac_Rai_NameChanging(string value);
    partial void OnDac_Rai_NameChanged();
    partial void OnDac_CityChanging(string value);
    partial void OnDac_CityChanged();
    partial void OnDac_VillageChanging(string value);
    partial void OnDac_VillageChanged();
    partial void OnDac_Full_AddressChanging(string value);
    partial void OnDac_Full_AddressChanged();
    partial void OnCONDITION_DESCRIPTIONChanging(string value);
    partial void OnCONDITION_DESCRIPTIONChanged();
    partial void OnCONDITION_IDChanging(System.Nullable<long> value);
    partial void OnCONDITION_IDChanged();
    partial void OnGaukmebuliPidChanging(System.Nullable<bool> value);
    partial void OnGaukmebuliPidChanged();
    #endregion
		
		public Source_Data()
		{
			this._Pirvelckaro_01_UMCEOEBIs = new EntitySet<Pirvelckaro_01_UMCEOEBI>(new Action<Pirvelckaro_01_UMCEOEBI>(this.attach_Pirvelckaro_01_UMCEOEBIs), new Action<Pirvelckaro_01_UMCEOEBI>(this.detach_Pirvelckaro_01_UMCEOEBIs));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Pirvelckaro", DbType="NVarChar(255) NOT NULL", CanBeNull=false)]
		public string Pirvelckaro
		{
			get
			{
				return this._Pirvelckaro;
			}
			set
			{
				if ((this._Pirvelckaro != value))
				{
					this.OnPirvelckaroChanging(value);
					this.SendPropertyChanging();
					this._Pirvelckaro = value;
					this.SendPropertyChanged("Pirvelckaro");
					this.OnPirvelckaroChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Base_Type", DbType="Int NOT NULL")]
		public int Base_Type
		{
			get
			{
				return this._Base_Type;
			}
			set
			{
				if ((this._Base_Type != value))
				{
					this.OnBase_TypeChanging(value);
					this.SendPropertyChanging();
					this._Base_Type = value;
					this.SendPropertyChanged("Base_Type");
					this.OnBase_TypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Source_Rec_Id", DbType="Int")]
		public System.Nullable<int> Source_Rec_Id
		{
			get
			{
				return this._Source_Rec_Id;
			}
			set
			{
				if ((this._Source_Rec_Id != value))
				{
					this.OnSource_Rec_IdChanging(value);
					this.SendPropertyChanging();
					this._Source_Rec_Id = value;
					this.SendPropertyChanged("Source_Rec_Id");
					this.OnSource_Rec_IdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Periodi", DbType="DateTime NOT NULL")]
		public System.DateTime Periodi
		{
			get
			{
				return this._Periodi;
			}
			set
			{
				if ((this._Periodi != value))
				{
					this.OnPeriodiChanging(value);
					this.SendPropertyChanging();
					this._Periodi = value;
					this.SendPropertyChanged("Periodi");
					this.OnPeriodiChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_MapDate", DbType="DateTime NOT NULL")]
		public System.DateTime MapDate
		{
			get
			{
				return this._MapDate;
			}
			set
			{
				if ((this._MapDate != value))
				{
					this.OnMapDateChanging(value);
					this.SendPropertyChanging();
					this._MapDate = value;
					this.SendPropertyChanged("MapDate");
					this.OnMapDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FID", DbType="NVarChar(50)")]
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
					this.OnFIDChanging(value);
					this.SendPropertyChanging();
					this._FID = value;
					this.SendPropertyChanged("FID");
					this.OnFIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Unnom", DbType="Int")]
		public System.Nullable<int> Unnom
		{
			get
			{
				return this._Unnom;
			}
			set
			{
				if ((this._Unnom != value))
				{
					this.OnUnnomChanging(value);
					this.SendPropertyChanging();
					this._Unnom = value;
					this.SendPropertyChanged("Unnom");
					this.OnUnnomChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UnnomisKhariskhi", DbType="Int NOT NULL")]
		public int UnnomisKhariskhi
		{
			get
			{
				return this._UnnomisKhariskhi;
			}
			set
			{
				if ((this._UnnomisKhariskhi != value))
				{
					this.OnUnnomisKhariskhiChanging(value);
					this.SendPropertyChanging();
					this._UnnomisKhariskhi = value;
					this.SendPropertyChanged("UnnomisKhariskhi");
					this.OnUnnomisKhariskhiChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PID", DbType="NVarChar(50)")]
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
					this.OnPIDChanging(value);
					this.SendPropertyChanging();
					this._PID = value;
					this.SendPropertyChanged("PID");
					this.OnPIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="First_Name", Storage="_First_Name", DbType="NVarChar(255)")]
		public string FIRST_NAME
		{
			get
			{
				return this._First_Name;
			}
			set
			{
				if ((this._First_Name != value))
				{
					this.OnFIRST_NAMEChanging(value);
					this.SendPropertyChanging();
					this._First_Name = value;
					this.SendPropertyChanged("FIRST_NAME");
					this.OnFIRST_NAMEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="Last_Name", Storage="_Last_Name", DbType="NVarChar(255)")]
		public string LAST_NAME
		{
			get
			{
				return this._Last_Name;
			}
			set
			{
				if ((this._Last_Name != value))
				{
					this.OnLAST_NAMEChanging(value);
					this.SendPropertyChanging();
					this._Last_Name = value;
					this.SendPropertyChanged("LAST_NAME");
					this.OnLAST_NAMEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Name="Birth_Date", Storage="_Birth_Date", DbType="DateTime")]
		public System.Nullable<System.DateTime> BIRTH_DATE
		{
			get
			{
				return this._Birth_Date;
			}
			set
			{
				if ((this._Birth_Date != value))
				{
					this.OnBIRTH_DATEChanging(value);
					this.SendPropertyChanging();
					this._Birth_Date = value;
					this.SendPropertyChanged("BIRTH_DATE");
					this.OnBIRTH_DATEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Sex", DbType="Int")]
		public System.Nullable<int> Sex
		{
			get
			{
				return this._Sex;
			}
			set
			{
				if ((this._Sex != value))
				{
					this.OnSexChanging(value);
					this.SendPropertyChanging();
					this._Sex = value;
					this.SendPropertyChanged("Sex");
					this.OnSexChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IdentPID", DbType="NVarChar(50)")]
		public string IdentPID
		{
			get
			{
				return this._IdentPID;
			}
			set
			{
				if ((this._IdentPID != value))
				{
					this.OnIdentPIDChanging(value);
					this.SendPropertyChanging();
					this._IdentPID = value;
					this.SendPropertyChanged("IdentPID");
					this.OnIdentPIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_J_ID", DbType="Int")]
		public System.Nullable<int> J_ID
		{
			get
			{
				return this._J_ID;
			}
			set
			{
				if ((this._J_ID != value))
				{
					this.OnJ_IDChanging(value);
					this.SendPropertyChanging();
					this._J_ID = value;
					this.SendPropertyChanged("J_ID");
					this.OnJ_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Piroba", DbType="Int")]
		public System.Nullable<int> Piroba
		{
			get
			{
				return this._Piroba;
			}
			set
			{
				if ((this._Piroba != value))
				{
					this.OnPirobaChanging(value);
					this.SendPropertyChanging();
					this._Piroba = value;
					this.SendPropertyChanged("Piroba");
					this.OnPirobaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Rai", DbType="NVarChar(255)")]
		public string Rai
		{
			get
			{
				return this._Rai;
			}
			set
			{
				if ((this._Rai != value))
				{
					this.OnRaiChanging(value);
					this.SendPropertyChanging();
					this._Rai = value;
					this.SendPropertyChanged("Rai");
					this.OnRaiChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Region_Name", DbType="NVarChar(255)")]
		public string Region_Name
		{
			get
			{
				return this._Region_Name;
			}
			set
			{
				if ((this._Region_Name != value))
				{
					this.OnRegion_NameChanging(value);
					this.SendPropertyChanging();
					this._Region_Name = value;
					this.SendPropertyChanged("Region_Name");
					this.OnRegion_NameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Rai_Name", DbType="NVarChar(255)")]
		public string Rai_Name
		{
			get
			{
				return this._Rai_Name;
			}
			set
			{
				if ((this._Rai_Name != value))
				{
					this.OnRai_NameChanging(value);
					this.SendPropertyChanging();
					this._Rai_Name = value;
					this.SendPropertyChanged("Rai_Name");
					this.OnRai_NameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_City", DbType="NVarChar(255)")]
		public string City
		{
			get
			{
				return this._City;
			}
			set
			{
				if ((this._City != value))
				{
					this.OnCityChanging(value);
					this.SendPropertyChanging();
					this._City = value;
					this.SendPropertyChanged("City");
					this.OnCityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Village", DbType="NVarChar(255)")]
		public string Village
		{
			get
			{
				return this._Village;
			}
			set
			{
				if ((this._Village != value))
				{
					this.OnVillageChanging(value);
					this.SendPropertyChanging();
					this._Village = value;
					this.SendPropertyChanged("Village");
					this.OnVillageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Street", DbType="NVarChar(255)")]
		public string Street
		{
			get
			{
				return this._Street;
			}
			set
			{
				if ((this._Street != value))
				{
					this.OnStreetChanging(value);
					this.SendPropertyChanging();
					this._Street = value;
					this.SendPropertyChanged("Street");
					this.OnStreetChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Full_Address", DbType="NVarChar(500)")]
		public string Full_Address
		{
			get
			{
				return this._Full_Address;
			}
			set
			{
				if ((this._Full_Address != value))
				{
					this.OnFull_AddressChanging(value);
					this.SendPropertyChanging();
					this._Full_Address = value;
					this.SendPropertyChanged("Full_Address");
					this.OnFull_AddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Dacesebuleba", DbType="NVarChar(255)")]
		public string Dacesebuleba
		{
			get
			{
				return this._Dacesebuleba;
			}
			set
			{
				if ((this._Dacesebuleba != value))
				{
					this.OnDacesebulebaChanging(value);
					this.SendPropertyChanging();
					this._Dacesebuleba = value;
					this.SendPropertyChanged("Dacesebuleba");
					this.OnDacesebulebaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Dac_Region_Name", DbType="NVarChar(255)")]
		public string Dac_Region_Name
		{
			get
			{
				return this._Dac_Region_Name;
			}
			set
			{
				if ((this._Dac_Region_Name != value))
				{
					this.OnDac_Region_NameChanging(value);
					this.SendPropertyChanging();
					this._Dac_Region_Name = value;
					this.SendPropertyChanged("Dac_Region_Name");
					this.OnDac_Region_NameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Dac_Rai_Name", DbType="NVarChar(255)")]
		public string Dac_Rai_Name
		{
			get
			{
				return this._Dac_Rai_Name;
			}
			set
			{
				if ((this._Dac_Rai_Name != value))
				{
					this.OnDac_Rai_NameChanging(value);
					this.SendPropertyChanging();
					this._Dac_Rai_Name = value;
					this.SendPropertyChanged("Dac_Rai_Name");
					this.OnDac_Rai_NameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Dac_City", DbType="NVarChar(255)")]
		public string Dac_City
		{
			get
			{
				return this._Dac_City;
			}
			set
			{
				if ((this._Dac_City != value))
				{
					this.OnDac_CityChanging(value);
					this.SendPropertyChanging();
					this._Dac_City = value;
					this.SendPropertyChanged("Dac_City");
					this.OnDac_CityChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Dac_Village", DbType="NVarChar(255)")]
		public string Dac_Village
		{
			get
			{
				return this._Dac_Village;
			}
			set
			{
				if ((this._Dac_Village != value))
				{
					this.OnDac_VillageChanging(value);
					this.SendPropertyChanging();
					this._Dac_Village = value;
					this.SendPropertyChanged("Dac_Village");
					this.OnDac_VillageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Dac_Full_Address", DbType="NVarChar(500)")]
		public string Dac_Full_Address
		{
			get
			{
				return this._Dac_Full_Address;
			}
			set
			{
				if ((this._Dac_Full_Address != value))
				{
					this.OnDac_Full_AddressChanging(value);
					this.SendPropertyChanging();
					this._Dac_Full_Address = value;
					this.SendPropertyChanged("Dac_Full_Address");
					this.OnDac_Full_AddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CONDITION_DESCRIPTION", DbType="NVarChar(100)")]
		public string CONDITION_DESCRIPTION
		{
			get
			{
				return this._CONDITION_DESCRIPTION;
			}
			set
			{
				if ((this._CONDITION_DESCRIPTION != value))
				{
					this.OnCONDITION_DESCRIPTIONChanging(value);
					this.SendPropertyChanging();
					this._CONDITION_DESCRIPTION = value;
					this.SendPropertyChanged("CONDITION_DESCRIPTION");
					this.OnCONDITION_DESCRIPTIONChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CONDITION_ID", DbType="BigInt")]
		public System.Nullable<long> CONDITION_ID
		{
			get
			{
				return this._CONDITION_ID;
			}
			set
			{
				if ((this._CONDITION_ID != value))
				{
					this.OnCONDITION_IDChanging(value);
					this.SendPropertyChanging();
					this._CONDITION_ID = value;
					this.SendPropertyChanged("CONDITION_ID");
					this.OnCONDITION_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GaukmebuliPid", DbType="Bit")]
		public System.Nullable<bool> GaukmebuliPid
		{
			get
			{
				return this._GaukmebuliPid;
			}
			set
			{
				if ((this._GaukmebuliPid != value))
				{
					this.OnGaukmebuliPidChanging(value);
					this.SendPropertyChanging();
					this._GaukmebuliPid = value;
					this.SendPropertyChanged("GaukmebuliPid");
					this.OnGaukmebuliPidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Source_Data_Pirvelckaro_01_UMCEOEBI", Storage="_Pirvelckaro_01_UMCEOEBIs", ThisKey="ID", OtherKey="SourceDataId")]
		public EntitySet<Pirvelckaro_01_UMCEOEBI> Pirvelckaro_01_UMCEOEBIs
		{
			get
			{
				return this._Pirvelckaro_01_UMCEOEBIs;
			}
			set
			{
				this._Pirvelckaro_01_UMCEOEBIs.Assign(value);
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		
		private void attach_Pirvelckaro_01_UMCEOEBIs(Pirvelckaro_01_UMCEOEBI entity)
		{
			this.SendPropertyChanging();
			entity.Source_Data = this;
		}
		
		private void detach_Pirvelckaro_01_UMCEOEBIs(Pirvelckaro_01_UMCEOEBI entity)
		{
			this.SendPropertyChanging();
			entity.Source_Data = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Pirvelckaro_01_UMCEOEBI")]
	public partial class Pirvelckaro_01_UMCEOEBI : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _RecId;
		
		private System.DateTime _RecDate;
		
		private int _Base_Type;
		
		private System.Nullable<int> _SourceDataId;
		
		private string _FID;
		
		private int _FAMILY_SCORE;
		
		private string _ZIP_CODE;
		
		private string _CITY;
		
		private string _GOVERNMENT;
		
		private string _VILLAGE;
		
		private string _FULL_ADDRESS;
		
		private string _PID;
		
		private System.DateTime _BIRTH_DATE;
		
		private string _FIRST_NAME;
		
		private string _LAST_NAME;
		
		private System.DateTime _SCORE_DATE;
		
		private System.Nullable<System.DateTime> _VISIT_DATE;
		
		private string _RESORE_DOC_NO;
		
		private System.Nullable<System.DateTime> _RESTORE_DOC_DATE;
		
		private System.Nullable<int> _N;
		
		private string _RAI_ID;
		
		private string _COMMENT;
		
		private EntityRef<Source_Data> _Source_Data;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnRecIdChanging(int value);
    partial void OnRecIdChanged();
    partial void OnRecDateChanging(System.DateTime value);
    partial void OnRecDateChanged();
    partial void OnBase_TypeChanging(int value);
    partial void OnBase_TypeChanged();
    partial void OnSourceDataIdChanging(System.Nullable<int> value);
    partial void OnSourceDataIdChanged();
    partial void OnFIDChanging(string value);
    partial void OnFIDChanged();
    partial void OnFAMILY_SCOREChanging(int value);
    partial void OnFAMILY_SCOREChanged();
    partial void OnZIP_CODEChanging(string value);
    partial void OnZIP_CODEChanged();
    partial void OnCITYChanging(string value);
    partial void OnCITYChanged();
    partial void OnGOVERNMENTChanging(string value);
    partial void OnGOVERNMENTChanged();
    partial void OnVILLAGEChanging(string value);
    partial void OnVILLAGEChanged();
    partial void OnFULL_ADDRESSChanging(string value);
    partial void OnFULL_ADDRESSChanged();
    partial void OnPIDChanging(string value);
    partial void OnPIDChanged();
    partial void OnBIRTH_DATEChanging(System.DateTime value);
    partial void OnBIRTH_DATEChanged();
    partial void OnFIRST_NAMEChanging(string value);
    partial void OnFIRST_NAMEChanged();
    partial void OnLAST_NAMEChanging(string value);
    partial void OnLAST_NAMEChanged();
    partial void OnSCORE_DATEChanging(System.DateTime value);
    partial void OnSCORE_DATEChanged();
    partial void OnVISIT_DATEChanging(System.Nullable<System.DateTime> value);
    partial void OnVISIT_DATEChanged();
    partial void OnRESORE_DOC_NOChanging(string value);
    partial void OnRESORE_DOC_NOChanged();
    partial void OnRESTORE_DOC_DATEChanging(System.Nullable<System.DateTime> value);
    partial void OnRESTORE_DOC_DATEChanged();
    partial void OnNChanging(System.Nullable<int> value);
    partial void OnNChanged();
    partial void OnRAI_IDChanging(string value);
    partial void OnRAI_IDChanged();
    partial void OnCOMMENTChanging(string value);
    partial void OnCOMMENTChanged();
    #endregion
		
		public Pirvelckaro_01_UMCEOEBI()
		{
			this._Source_Data = default(EntityRef<Source_Data>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RecId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int RecId
		{
			get
			{
				return this._RecId;
			}
			set
			{
				if ((this._RecId != value))
				{
					this.OnRecIdChanging(value);
					this.SendPropertyChanging();
					this._RecId = value;
					this.SendPropertyChanged("RecId");
					this.OnRecIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RecDate", DbType="DateTime NOT NULL")]
		public System.DateTime RecDate
		{
			get
			{
				return this._RecDate;
			}
			set
			{
				if ((this._RecDate != value))
				{
					this.OnRecDateChanging(value);
					this.SendPropertyChanging();
					this._RecDate = value;
					this.SendPropertyChanged("RecDate");
					this.OnRecDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Base_Type", DbType="Int NOT NULL")]
		public int Base_Type
		{
			get
			{
				return this._Base_Type;
			}
			set
			{
				if ((this._Base_Type != value))
				{
					this.OnBase_TypeChanging(value);
					this.SendPropertyChanging();
					this._Base_Type = value;
					this.SendPropertyChanged("Base_Type");
					this.OnBase_TypeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SourceDataId", DbType="Int")]
		public System.Nullable<int> SourceDataId
		{
			get
			{
				return this._SourceDataId;
			}
			set
			{
				if ((this._SourceDataId != value))
				{
					if (this._Source_Data.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnSourceDataIdChanging(value);
					this.SendPropertyChanging();
					this._SourceDataId = value;
					this.SendPropertyChanged("SourceDataId");
					this.OnSourceDataIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FID", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
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
					this.OnFIDChanging(value);
					this.SendPropertyChanging();
					this._FID = value;
					this.SendPropertyChanged("FID");
					this.OnFIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FAMILY_SCORE", DbType="Int NOT NULL")]
		public int FAMILY_SCORE
		{
			get
			{
				return this._FAMILY_SCORE;
			}
			set
			{
				if ((this._FAMILY_SCORE != value))
				{
					this.OnFAMILY_SCOREChanging(value);
					this.SendPropertyChanging();
					this._FAMILY_SCORE = value;
					this.SendPropertyChanged("FAMILY_SCORE");
					this.OnFAMILY_SCOREChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ZIP_CODE", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string ZIP_CODE
		{
			get
			{
				return this._ZIP_CODE;
			}
			set
			{
				if ((this._ZIP_CODE != value))
				{
					this.OnZIP_CODEChanging(value);
					this.SendPropertyChanging();
					this._ZIP_CODE = value;
					this.SendPropertyChanged("ZIP_CODE");
					this.OnZIP_CODEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CITY", DbType="NVarChar(50)")]
		public string CITY
		{
			get
			{
				return this._CITY;
			}
			set
			{
				if ((this._CITY != value))
				{
					this.OnCITYChanging(value);
					this.SendPropertyChanging();
					this._CITY = value;
					this.SendPropertyChanged("CITY");
					this.OnCITYChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GOVERNMENT", DbType="NVarChar(50)")]
		public string GOVERNMENT
		{
			get
			{
				return this._GOVERNMENT;
			}
			set
			{
				if ((this._GOVERNMENT != value))
				{
					this.OnGOVERNMENTChanging(value);
					this.SendPropertyChanging();
					this._GOVERNMENT = value;
					this.SendPropertyChanged("GOVERNMENT");
					this.OnGOVERNMENTChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VILLAGE", DbType="NVarChar(50)")]
		public string VILLAGE
		{
			get
			{
				return this._VILLAGE;
			}
			set
			{
				if ((this._VILLAGE != value))
				{
					this.OnVILLAGEChanging(value);
					this.SendPropertyChanging();
					this._VILLAGE = value;
					this.SendPropertyChanged("VILLAGE");
					this.OnVILLAGEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FULL_ADDRESS", DbType="NVarChar(255)")]
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
					this.OnFULL_ADDRESSChanging(value);
					this.SendPropertyChanging();
					this._FULL_ADDRESS = value;
					this.SendPropertyChanged("FULL_ADDRESS");
					this.OnFULL_ADDRESSChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PID", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
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
					this.OnPIDChanging(value);
					this.SendPropertyChanging();
					this._PID = value;
					this.SendPropertyChanged("PID");
					this.OnPIDChanged();
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
					this.OnBIRTH_DATEChanging(value);
					this.SendPropertyChanging();
					this._BIRTH_DATE = value;
					this.SendPropertyChanged("BIRTH_DATE");
					this.OnBIRTH_DATEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FIRST_NAME", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
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
					this.OnFIRST_NAMEChanging(value);
					this.SendPropertyChanging();
					this._FIRST_NAME = value;
					this.SendPropertyChanged("FIRST_NAME");
					this.OnFIRST_NAMEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LAST_NAME", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
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
					this.OnLAST_NAMEChanging(value);
					this.SendPropertyChanging();
					this._LAST_NAME = value;
					this.SendPropertyChanged("LAST_NAME");
					this.OnLAST_NAMEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SCORE_DATE", DbType="DateTime NOT NULL")]
		public System.DateTime SCORE_DATE
		{
			get
			{
				return this._SCORE_DATE;
			}
			set
			{
				if ((this._SCORE_DATE != value))
				{
					this.OnSCORE_DATEChanging(value);
					this.SendPropertyChanging();
					this._SCORE_DATE = value;
					this.SendPropertyChanged("SCORE_DATE");
					this.OnSCORE_DATEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_VISIT_DATE", DbType="DateTime")]
		public System.Nullable<System.DateTime> VISIT_DATE
		{
			get
			{
				return this._VISIT_DATE;
			}
			set
			{
				if ((this._VISIT_DATE != value))
				{
					this.OnVISIT_DATEChanging(value);
					this.SendPropertyChanging();
					this._VISIT_DATE = value;
					this.SendPropertyChanged("VISIT_DATE");
					this.OnVISIT_DATEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RESORE_DOC_NO", DbType="NVarChar(50)")]
		public string RESORE_DOC_NO
		{
			get
			{
				return this._RESORE_DOC_NO;
			}
			set
			{
				if ((this._RESORE_DOC_NO != value))
				{
					this.OnRESORE_DOC_NOChanging(value);
					this.SendPropertyChanging();
					this._RESORE_DOC_NO = value;
					this.SendPropertyChanged("RESORE_DOC_NO");
					this.OnRESORE_DOC_NOChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RESTORE_DOC_DATE", DbType="DateTime")]
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
					this.OnRESTORE_DOC_DATEChanging(value);
					this.SendPropertyChanging();
					this._RESTORE_DOC_DATE = value;
					this.SendPropertyChanged("RESTORE_DOC_DATE");
					this.OnRESTORE_DOC_DATEChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_N", DbType="Int")]
		public System.Nullable<int> N
		{
			get
			{
				return this._N;
			}
			set
			{
				if ((this._N != value))
				{
					this.OnNChanging(value);
					this.SendPropertyChanging();
					this._N = value;
					this.SendPropertyChanged("N");
					this.OnNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RAI_ID", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string RAI_ID
		{
			get
			{
				return this._RAI_ID;
			}
			set
			{
				if ((this._RAI_ID != value))
				{
					this.OnRAI_IDChanging(value);
					this.SendPropertyChanging();
					this._RAI_ID = value;
					this.SendPropertyChanged("RAI_ID");
					this.OnRAI_IDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_COMMENT", DbType="NVarChar(100)")]
		public string COMMENT
		{
			get
			{
				return this._COMMENT;
			}
			set
			{
				if ((this._COMMENT != value))
				{
					this.OnCOMMENTChanging(value);
					this.SendPropertyChanging();
					this._COMMENT = value;
					this.SendPropertyChanged("COMMENT");
					this.OnCOMMENTChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Source_Data_Pirvelckaro_01_UMCEOEBI", Storage="_Source_Data", ThisKey="SourceDataId", OtherKey="ID", IsForeignKey=true, DeleteRule="SET NULL")]
		public Source_Data Source_Data
		{
			get
			{
				return this._Source_Data.Entity;
			}
			set
			{
				Source_Data previousValue = this._Source_Data.Entity;
				if (((previousValue != value) 
							|| (this._Source_Data.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Source_Data.Entity = null;
						previousValue.Pirvelckaro_01_UMCEOEBIs.Remove(this);
					}
					this._Source_Data.Entity = value;
					if ((value != null))
					{
						value.Pirvelckaro_01_UMCEOEBIs.Add(this);
						this._SourceDataId = value.ID;
					}
					else
					{
						this._SourceDataId = default(Nullable<int>);
					}
					this.SendPropertyChanged("Source_Data");
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
