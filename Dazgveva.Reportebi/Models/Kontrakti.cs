using System;
using System.ComponentModel.DataAnnotations;

namespace Dazgveva.Reportebi.Models
{
    public class Kontrakti
    {
        public int ID { get; set; }
        public string Base_Description { get; set; }
        public int? Unnom { get; set; }
        public string FID { get; set; }
        public string PID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? BIRTH_DATE { get; set; }
        public string RAI { get; set; }
        public string CITY { get; set; }
        public string ADDRESS_FULL { get; set; }
        public string aRAI { get; set; }
        public string aCITY { get; set; }
        public string aADDRESS_FULL { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? GanakhlebisTarigi { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? ADD_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? dagv_tar { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? CONTINUE_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? End_Date { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? STOP_DATE { get; set; }
        public string Company { get; set; }
        public int? STATE { get; set; }
        public string POLISIS_NOMERI { get; set; }
        public int? GAUKMEBULI { get; set; }
        public string VIN_GAAUQMA { get; set; }
        public string Ganmarteba { get; set; }
        public int Base_type { get; set; }
        public string RAI_NAME { get; set; }
        public string VILLAGE { get; set; }
    }
}