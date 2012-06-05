using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dazgveva.Reportebi.Controllers;

namespace Dazgveva.Reportebi.Models
{
    public class FamiliDataPeriodi
    {
        private readonly FamiliData _fd;

        public FamiliDataPeriodi(FamiliData fd)
        {
        }

        public FamiliDataPeriodi(int min, int max, FamiliData fd)
        {
            _fd = fd;
            MinPeriodi = min;
            MaxPeriodi = max;
        }

        public int MinPeriodi { get; private set; }
        public int MaxPeriodi { get; private set; }
        public string FID { get { return _fd.FID; } }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyy}")]
        public System.DateTime? SCORE_DATE { get { return _fd.SCORE_DATE; } }
        public int? FAMILY_SCORE { get { return _fd.FAMILY_SCORE; } }
        public IList<FamiliDataCevri> Cevrebi { get { return _fd.Cevrebi; } }
    }
}