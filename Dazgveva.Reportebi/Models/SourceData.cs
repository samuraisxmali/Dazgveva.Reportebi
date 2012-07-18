using System;
using System.ComponentModel.DataAnnotations;

namespace Dazgveva.Reportebi.Models
{
    public class SourceData
    {
        public int ID { get; set; }
        public string Pirvelckaro { get; set; } 
        public int? Base_Type { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? Periodi { get; set; }
        public string FID { get; set; }
        public string PID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? BIRTH_DATE { get; set; }
        public int? Sex { get; set; } 
        public string Region_Name { get; set; }
        public string Rai_Name { get; set; } 
        public string City { get; set; }  
        public string Village { get; set; }
        public string Street { get; set; }  
        public string Full_Address { get; set; }
        public string Dacesebuleba { get; set; }  
        public string Dac_Region_Name { get; set; }
        public string Dac_Rai_Name { get; set; }
        public string Dac_City { get; set; }
        public string Dac_Village { get; set; }
        public string Dac_Full_Address { get; set; }
        public string Region_ID { get; set; }
        public string Rai { get; set; }
        public int? ITEM_NO { get; set; }
        public int? Unnom { get; set; }
        public int? DEC_REC_ID { get; set; }
        public string Xarvezi { get; set; }
        public int? J_ID { get; set; }
        public int? Piroba { get; set; }

    }
}