using System;
using System.ComponentModel.DataAnnotations;

namespace Dazgveva.Reportebi.Models
{
    public class FamiliDataCevri
    {
        public string PID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyy}")]
        public DateTime? BIRTH_DATE { get; set; }

        public int? PIROBA { get; set; }
    }
}