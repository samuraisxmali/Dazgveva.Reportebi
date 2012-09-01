using System;
using System.ComponentModel.DataAnnotations;

namespace Dazgveva.Reportebi.Models
{
    public class DeklaraciebisIstoriaList
    {
        public string FID { get; set; }
        public string PID { get; set; }
        public string LAST_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? BIRTH_DATE { get; set; }
        public string FULL_ADDRESS { get; set; }
        public int FID_VERSION { get; set; }
        public string HOME_PHONE { get; set; }
        public string MOB_PHONE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? CALC_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? LEGAL_SCORE_DATE { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public DateTime? RESTORE_DOC_DATE { get; set; }
        public bool ON_CONTROL { get; set; }
        public int? ACTION_TYPE { get; set; }
    }
}