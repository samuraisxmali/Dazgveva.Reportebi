using System.ComponentModel.DataAnnotations;

namespace Dazgveva.Reportebi.Models
{
    public class KontraktisPeriodi
    {
        public int ID { get; set; }
        public int Periodi { get; set; }
        public int? Dasabechdi { get; set; }
        public int? State { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public System.DateTime? CONTINUE_DATE { get; set; }

        public string Company { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public System.DateTime? STOP_DATE { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyy}")]
        public System.DateTime? ADD_DATE { get; set; }
    }
}