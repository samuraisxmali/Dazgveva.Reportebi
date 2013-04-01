using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dazgveva.Reportebi.Models
{
    public interface IChevicavPirovnebisRekvizitebs
    {
        int? Unnom { get; }
        string FID { get; }
        string PID { get; }
        string FIRST_NAME { get; }
        string LAST_NAME { get; }
        DateTime? BIRTH_DATE { get; }
    }

    public partial class DAZGVEVA_201304 : IChevicavPirovnebisRekvizitebs
    {
    }

    public partial class Source_Data : IChevicavPirovnebisRekvizitebs
    {
    }
}