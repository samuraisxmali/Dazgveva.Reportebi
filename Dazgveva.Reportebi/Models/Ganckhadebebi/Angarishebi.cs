using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using Dapper;

namespace Dazgveva.Reportebi.Models.Ganckhadebebi
{
    public static class Angarishebi
    {
        public static IList<dynamic> Ganckhadebebi(this SqlConnection conn, string sadzieboTexti)
        {
            var whereNacili = WhereNacili(sadzieboTexti, "PID", "FID", "Birth_Date", "First_Name", "Last_Name");
            if ((whereNacili) == null)
                return new dynamic[0];
            return conn.Query(@"SELECT g.*,p.ProgramisDasakheleba
FROM [DazgvevaGanckhadebebi].[dbo].[Ganckhadebebi] (nolock) g 
JOIN SocialuriDazgveva.dbo.Programebi (nolock) p on g.Base_Type = p.ProgramisId
where not exists (
    select null from [DazgvevaGanckhadebebi].[dbo].GaukmebuliGanckhadebebi gg 
    where gg.GaukmebuliGanckhId = g.Id
    )
AND g.StatusisMopovebisTarigi <= g.DadasturebisTarigi
AND " + whereNacili.Item1 + @" order by RegistraciisTarigi", whereNacili.Item2).ToList();

        }

        public static IList<IDictionary<string, object>> PirvelckarosChanacerebi(this SqlConnection conn, int basetype, int unnom)
        {
            var pirvelckarosCkhrilebi = conn.Query<string>(
                    "select TABLE_NAME from Pirvelckaroebi.INFORMATION_SCHEMA.TABLES where TABLE_NAME like 'Pirvelckaro_%_%'")
                    .Where(x => x.Split('_').Length > 2)
                    .ToDictionary(x => int.Parse(x.Split('_')[1]));
            var dublebi = conn.Query(
                "select * from UketesiReestri..UnnomisConventori where OldUnnom=@unnom or NewUnnom=@unnom",
                new { unnom = unnom }).ToList();
            var unnomebi = dublebi.Select(x => (int)x.OldUnnom).Concat(dublebi.Select(x => (int)x.NewUnnom)).Concat(new[]{unnom}).Distinct();
            return conn.Query(@"SELECT *
FROM [Pirvelckaroebi].[dbo].[" + pirvelckarosCkhrilebi[basetype] + @"] (nolock) g 
where Unnom in (" + string.Join(",", unnomebi) + @") order by RecId")
                                              .Cast<IDictionary<string, object>>()
                                              .ToList();

        }

        public static dynamic BoloKargiChanaceri(this SqlConnection conn, int unnom)
        {
            return conn.Query(@"SELECT *
FROM [UketesiReestri].[dbo].vUnnomBoloKargiChanaceri (nolock) 
WHERE Unnom = @unnom", new { unnom = unnom }).FirstOrDefault();

        }

        public static IList<dynamic> PirovnebisRekvizitebisIstoria(this SqlConnection conn, int unnom)
        {
            return conn.Query(@"SELECT *
FROM [UketesiReestri].[dbo].UnnomShesadarebeliReestri (nolock) 
WHERE Unnom = @unnom
ORDER BY Tarigi", new { unnom = unnom }).ToList();

        }

        public static IList<dynamic> UnnomisConventori(this SqlConnection conn, int unnom)
        {
            return conn.Query(@"SELECT * 
FROM [UketesiReestri].[dbo].UnnomisConventori (nolock) 
WHERE OldUnnom = @unnom or NewUnnom = @unnom", new { unnom = unnom }).ToList();

        }

        public static IList<dynamic> ReestrisStatusebisIstoria(this SqlConnection conn, string pid)
        {
            return conn.Query(@"SELECT *
FROM (
    SELECT [PID]
          ,'Mokalakeoba' Atributi
          ,[Mnishvneloba]
          ,[Dan]
          ,[Mde]
    FROM [UketesiReestri].[dbo].[PirovnebisMokalakeobisCvlilebebisPeriodebi] (nolock)
    union all
    SELECT [PID]
        ,'NeitraluriMocmoba' Statusi
        ,[Mnishvneloba]
        ,[Dan]
        ,[Mde]
    FROM [UketesiReestri].[dbo].[PirovnebisNeitraluriMocmobisCvlilebebisPeriodebi] (nolock)
    union all
    SELECT [PID]
        ,'Statusi' Statusi
        ,[Mnishvneloba]
        ,[Dan]
        ,[Mde]
    FROM [UketesiReestri].[dbo].[PirovnebisStatusisCvlilebebisPeriodebi] (nolock)
) a
WHERE a.PID = @PID
ORDER BY Atributi,Dan", new { PID = pid }).ToList();
        }

        private static Tuple<string, object> WhereNacili(string q, string pid, string fid, string tarigi, string sakheli, string gvari)
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


    }
}