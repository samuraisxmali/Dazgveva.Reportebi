using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Dapper;
using Dazgveva.Reportebi.Models;

namespace Dazgveva.Reportebi.Queries
{
    public static class QueriesExtensions
    {
        public static ICollection<Kontrakti> MomeciKontraktebi(this SqlConnection conn, string sadzieboTexti, int sadazgveosId)
        {
            var boloDazgCkhrili = conn.Query<string>("SELECT TOP 1 TABLE_NAME FROM INSURANCEW.INFORMATION_SCHEMA.TABLES where TABLE_NAME like 'DAZGVEVA_201[0-9][0-9][0-9]' ORDER BY TABLE_NAME DESC").First();
            var periodi = boloDazgCkhrili.Substring(9, 6);
            string sql = "SELECT  d.ID ID," +
                         "d.Base_Description Base_Description," +

                         "d.FID FID," +

                         "d.Unnom Unnom," +
                         "d.PID PID," +
                         "d.FIRST_NAME FIRST_NAME," +
                         "d.LAST_NAME LAST_NAME," +
                         "d.BIRTH_DATE BIRTH_DATE," +

                         "d.RAI RAI," +
                         "d.CITY CITY," +
                         "d.ADDRESS_FULL ADDRESS_FULL," +

                         "m.RAI as aRAI, " +
                         "m.CITY as aCITY, " +
                         "m.ADDRESS_FULL as aADDRESS_FULL, " +
                         "mk.DRO as GanakhlebisTarigi," +


                         "d.[dagv-tar] dagv_tar," +
                         "d.STATE_" + periodi + " STATE," +
                         "d.ADD_DATE_" + periodi + "_TMP ADD_DATE," +
                         "d.CONTINUE_DATE_" + periodi + "_TMP CONTINUE_DATE," +
                         "d.STOP_DATE_" + periodi + "_TMP STOP_DATE," +
                         "d.Company_" + periodi + " Company," +
                         "d.End_Date End_Date," +
                         "d.POLISIS_NOMERI POLISIS_NOMERI," +

                         "p.KontraktisNomeri as GAUKMEBULI, " +
                         "p.Pirovneba as VIN_GAAUQMA, " +
                         "s.Ganmarteba " +

                         "FROM INSURANCEW.dbo.[" + boloDazgCkhrili + "] (nolock) d " +
                         "left join INSURANCEW.dbo.StatusebisGanmarteba s ON d.STATE_" + periodi + " = s.Statusi " +
                         "left join INSURANCEW.dbo.aMisamartebi m ON d.ID = m.ID " +
                         "left join (select DazgvevisID,Max(DRO) as DRO from INSURANCEW.dbo.aMisamartisKorektirebisIstoria group by DazgvevisID) mk on d.ID = mk.DazgvevisID " +
                         "left join INSURANCEW.dbo.KontraktisGauqmeba p on d.ID = p.KontraktisNomeri " +
                         "WHERE d.Company_ID_" + periodi + " = " + sadazgveosId + " AND ";

            var whereNacili = WhereNacili(sadzieboTexti, "PID", "FID", "BIRTH_DATE", "FIRST_NAME", "LAST_NAME");
            if (whereNacili == null)
                return new List<Kontrakti>();
            var momeciKontraktebi = conn.Query<Kontrakti>(sql + whereNacili.Item1, whereNacili.Item2, commandTimeout: 999).ToList();
            return momeciKontraktebi;
        }
        static Tuple<string, object> WhereNacili(string q, string pid, string fid, string tarigi, string sakheli, string gvari)
        {
            var sadzieboTekstebi = q.Split(' ', ';', ',')
                                    .Select(x => x.Trim())
                                    .Where(x => x.Length > 0)
                                    .ToList();

            var pidebi = sadzieboTekstebi.Where(x => x.Length == 11 && x.All(Char.IsNumber))
                                         .ToList();

            var tarigebi = sadzieboTekstebi.Select(x =>
            {
                DateTime dt;
                return new { ParsedDate = DateTime.TryParse(x, CultureInfo.CreateSpecificCulture("ka-GE"), DateTimeStyles.None, out dt) ? dt : default(DateTime?), OrgString = x };
            })
                                           .Where(x => x.ParsedDate.HasValue)
                                           .ToList();


            var savaraudoFidebi = sadzieboTekstebi.Where(x => x.Any(Char.IsNumber))
                                                  .Except(pidebi)
                                                  .Except(tarigebi.Select(x => x.OrgString))
                                                  .ToList();

            var sakheliAnGvarebi = sadzieboTekstebi.Except(pidebi)
                                                   .Except(tarigebi.Select(x => x.OrgString))
                                                   .Except(savaraudoFidebi)
                                                   .ToList();

            var sakheliDaGvarebi = (from s in sakheliAnGvarebi
                                    from g in sakheliAnGvarebi
                                    where s != g
                                    select new { s, g })
                                   .ToList();

            var sakheliDaDabDarigebi = (from sg in sakheliDaGvarebi
                                        from t in tarigebi.Select(x => x.ParsedDate)
                                        select new { sg.s, sg.g, t })
                                       .ToList();

            if (pidebi.FirstOrDefault() != null)
                return Tuple.Create<string, object>(pid + "=@Pid", new { Pid = pidebi.FirstOrDefault() });
            else if (savaraudoFidebi.FirstOrDefault() != null)
                return Tuple.Create<string, object>(fid + "=@Fid", new { Fid = savaraudoFidebi.FirstOrDefault() });
            else if (sakheliDaDabDarigebi.Count() > 1)
                return Tuple.Create<string, object>(string.Join(" OR ", sakheliDaDabDarigebi.Select((x, i) => string.Format("( " + sakheli + "=@s{0} AND " + gvari + "=@g{0} AND " + tarigi + "=@d{0})", i))),
                                                           sakheliDaDabDarigebi.Aggregate(Tuple.Create(new DynamicParameters(), 0), (s, v) =>
                                                           {
                                                               s.Item1.Add("s" + s.Item2, v.s);
                                                               s.Item1.Add("g" + s.Item2, v.g);
                                                               s.Item1.Add("d" + s.Item2, v.t);
                                                               return Tuple.Create(s.Item1, s.Item2 + 1);
                                                           }).Item1);
            else if (sakheliDaGvarebi.Count() > 1)
                return Tuple.Create<string, object>(string.Join(" OR ", sakheliDaGvarebi.Select((x, i) => string.Format("(" + sakheli + "=@s{0} AND " + gvari + "=@g{0})", i))),
                                                           sakheliDaGvarebi.Aggregate(Tuple.Create(new DynamicParameters(), 0), (s, v) =>
                                                           {
                                                               s.Item1.Add("s" + s.Item2, v.s);
                                                               s.Item1.Add("g" + s.Item2, v.g);
                                                               return Tuple.Create(s.Item1, s.Item2 + 1);
                                                           }).Item1);
            else
                return null;
        }


        public static IEnumerable<KontraktisPeriodi> MomeKontraktisPeriodebi(this SqlConnection conn, int kontraktisId)
        {
            var boloDazgCkhrili = conn.Query<string>("SELECT TOP 1 TABLE_NAME FROM INSURANCEW.INFORMATION_SCHEMA.TABLES where TABLE_NAME like 'DAZGVEVA_201[0-9][0-9][0-9]' ORDER BY TABLE_NAME DESC").First();
            var cols = conn.Query<string>(string.Format(@"SELECT COLUMN_NAME FROM INSURANCEW.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", boloDazgCkhrili)).ToList();
            var periodebi = (
                from state in MomePerioduliVelebi(cols, "STATE")
                from continueDate in MomePerioduliVelebi(cols, "CONTINUE_DATE").Where(x => x.Key == state.Key).DefaultIfEmpty()
                from dasabechdi in MomePerioduliVelebi(cols, "DASABECHDI").Where(x => x.Key == state.Key).DefaultIfEmpty()
                from company in MomePerioduliVelebi(cols, "Company").Where(x => x.Key == state.Key).DefaultIfEmpty()
                from stopDate in MomePerioduliVelebi(cols, "STOP_DATE").Where(x => x.Key == state.Key).DefaultIfEmpty()
                from addDate in MomePerioduliVelebi(cols, "ADD_DATE").Where(x => x.Key == state.Key).DefaultIfEmpty()
                select new
                {
                    Periodi = state.Key,
                    State = state.Value,
                    ContinueDate = continueDate.Value,
                    Dasabechdi = dasabechdi.Value,
                    Company = company.Value,
                    StopDate = stopDate.Value,
                    AddDate = addDate.Value
                }
            );
            return (
                from p in periodebi
                from c in
                    conn.Query("select top 4 * from INSURANCEW..[" + boloDazgCkhrili + "] where ID = 589566")
                        .Cast<IDictionary<string, object>>()
                select new KontraktisPeriodi
                {
                    ID = (int)c["ID"],
                    Periodi = p.Periodi,
                    Dasabechdi = p.Dasabechdi == null ? null : (int?)c[p.Dasabechdi],
                    State = (int?)c[p.State],
                    CONTINUE_DATE = (DateTime?)c[p.ContinueDate],
                    Company = (string)c[p.Company],
                    STOP_DATE = (DateTime?)c[p.StopDate],
                    ADD_DATE = (DateTime?)c[p.AddDate],
                }
            );
        }


        static IDictionary<int, string> MomePerioduliVelebi(IEnumerable<string> cols, string sackisiDasaxeleba)
        {
            var regexContDate = new Regex(@"^(" + sackisiDasaxeleba + @")(_(\d+)(_TMP)?)?$", RegexOptions.IgnoreCase);

            var velebi = cols
                .Select(x => regexContDate.Match(x))
                .Where(x => x.Success)
                .Select(x => new { x.Captures[0].Value, G = x.Groups[3].Value }).ToList();


            var cinaPeriodi = DateTime.Parse(velebi.Max(v => v.G).Insert(4, "-") + "-01")
                                    .AddMonths(-1)
                                    .ToString("yyyyMM");

            return velebi.Where(v => v.G.Length == 2).Select(v => new { v.Value, G = "2010" + v.G })
                    .Concat(velebi.Where(v => v.G.Length == 0).Select(v => new { v.Value, G = cinaPeriodi }))
                    .Concat(velebi.Where(v => v.G.Length == 6))
                    .Select(v => new { v.Value, G = int.Parse(v.G) })
                    .OrderBy(v => v.G)
                    .ToDictionary(v => v.G, v => v.Value);
        }
    }
}