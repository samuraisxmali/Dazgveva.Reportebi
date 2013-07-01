using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Dapper;
using Dazgveva.Reportebi.Models;

namespace MvcApplication2.Models
{
    public class Damkoreqtirebeli
    {
        public class ReestrisPasuxi
        {
            public PI PersonInformacia { get; set; }
            public class PI
            {
                public string PrivateNumber { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
                public DateTime? BirthDate { get; set; }
                public int? Gender { get; set; }
                public string RegionStr { get; set; }
                public string LivingPlace { get; set; }

            }
        }
    

        //usefull
        public SqlConnection GaxseniKavshiri()
        {
            var conn =
                new SqlConnection(
                    System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"]
                        .ConnectionString);
            conn.Open();
            return conn;
        }
        //usefull
        public List<Kontrakti> CamoigeDasakoreqtirebeliKontraqti(int ID, SqlConnection conn)
        {
            var sql = @"select
                     isnull(d.ID		    ,'')  as ID	
                    ,isnull(d.Base_Description,'')as Base_Description
	                ,isnull(d.Base_type,'')       as Base_type
				    ,isnull(d.Unnom		    ,'')  as Unnom		
				    ,isnull(d.PID		    ,'')  as PID		
				    ,isnull(d.FID		    ,'')  as FID		
				    ,isnull(d.FIRST_NAME    ,'')  as FIRST_NAME
				    ,isnull(d.LAST_NAME	    ,'')  as LAST_NAME	
				    ,isnull(d.BIRTH_DATE    ,'')  as BIRTH_DATE
				    ,isnull(d.REGION_ID	    ,'')  as REGION_ID	
				    ,isnull(d.RAI		    ,'')  as RAI		
				    ,isnull(d.RAI_NAME	    ,'')  as RAI_NAME	
				    ,isnull(d.CITY		    ,'')  as CITY		
				    ,isnull(d.VILLAGE	    ,'')  as VILLAGE	
				    ,isnull(d.ADDRESS_FULL  ,'')  as ADDRESS_FULL
				    FROM INSURANCEW.dbo.DAZGVEVA_201307 d where ID =" + ID;
            var dasakoreqtirebeliKontraqti = conn.Query<Kontrakti>(sql).ToList();
            return dasakoreqtirebeliKontraqti;
        }
        //usefull
        private string URL(string PID)
        {
            var j =
                string.Format(
                    @"http://172.17.8.125/CRA_Rest/PersonInfo/JSONPersonInfoPid?piradiNomeri={0}&ckaro=Cra&userName=zurabbat", PID);
            return j;
        }
        
        public Kontrakti CamoigeReestridan(string PID)
        {
            var url = URL(PID);
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string jsonURL = url;
            var jObj = client.DownloadString(jsonURL);
            var o = Newtonsoft.Json.JsonConvert.DeserializeObject<ReestrisPasuxi>(jObj);
            Kontrakti Model = new Kontrakti();
            Model.PID = o.PersonInformacia.PrivateNumber;
            Model.LAST_NAME = o.PersonInformacia.LastName;
            Model.FIRST_NAME = o.PersonInformacia.FirstName;
            Model.BIRTH_DATE = o.PersonInformacia.BirthDate.Value.AddHours(4);
            Model.RAI_NAME = o.PersonInformacia.RegionStr;
            Model.ADDRESS_FULL = o.PersonInformacia.LivingPlace;
            return Model;
         
        }

        public void DaakoreqtireMisamarti(int ID,string tags, string cityInput, string villageInput, string AddressInput ,SqlConnection con)
        {
           
            var query = string.Format("exec TempO.dbo.Koreqtireba {0},N'{1}',N'{2}',N'{3}',N'{4}' "
             , ID
             , tags
             , cityInput
             , villageInput
             , AddressInput
              );
            con.Execute(query);     
        }

        public void DaakoreqtirePiradiMonacemebi(int ID,string PID, string FirstName, string LastName, string BirthDate, SqlConnection con)
        {
            
             var query = string.Format("exec TempO.dbo.PiradiMonacemebisKoreqtireba '{0}',N'{1}',N'{2}',N'{3}'"
             , ID 
             , FirstName
             , LastName
             , BirthDate
            );
            con.Execute(query);     
        }
        }

    }


 