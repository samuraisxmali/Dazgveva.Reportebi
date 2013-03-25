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
    

        private SqlConnection GaxseniKavshiri()
        {
            SqlConnection con = new SqlConnection("Data Source=172.17.250.10;Initial Catalog=TempO;Persist Security Info=True;User ID=sa;Password=ssa$20");
            con.Open();
            return con;
        }

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
            Model.BIRTH_DATE = o.PersonInformacia.BirthDate;
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

        public void DaakoreqtirePiradiMonacemebi(int ID,string PID, string FirstName, string LastName, DateTime? BirthDate, SqlConnection con)
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


 