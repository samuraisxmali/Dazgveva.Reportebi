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
        static Dictionary<int,string> pirvelckaroebi = new Dictionary<int, string>()
            {
                {1,"select * from [Pirvelckaro_01_UMCEOEBI] (nolock)"},
{2,"select * from [Pirvelckaro_02_DEVNILEBI] (nolock)"},
{3,"select * from [Pirvelckaro_03_BAVSHVEBI] (nolock)"},
{4,"select * from [Pirvelckaro_04_REINTEGRACIA] (nolock)"},
{5,"select * from [Pirvelckaro_05_KULTURA] (nolock)"},
{6,"select * from [Pirvelckaro_06_XANDAZMULEBI] (nolock)"},
{7,"select * from [Pirvelckaro_07_SKOLA_PANSIONEBI] (nolock)"},
{8,"select * from [Pirvelckaro_08_TEACHERS] (nolock)"},
{9,"select * from [Pirvelckaro_09_UFROSI_AGMZRDELEBI] (nolock)"},
{10,"select * from [Pirvelckaro_10_APKHAZETIS_OJAKHEBI] (nolock)"},
{100,"select * from [Pirvelckaro_100_DevniltaMisamartebi_201210] (nolock)"},
{11,"select * from [Pirvelckaro_11_SATEMO] (nolock)"},
{12,"select * from [Pirvelckaro_12_MCIRE_SAOJAXO] (nolock)"},
{13,"select * from [Pirvelckaro_13_TEACHERS_AFX] (nolock)"},
{14,"select * from [Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI] (nolock)"},
{21,"select * from [Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA] (nolock)"},
{22,"select * from [Pirvelckaro_22_STUDENTEBI] (nolock)"},
{23,@"select [RecId],[RecDate],[Base_Type],[SourceDataId],[FIRST],[LAST],[GENDER],[PRIVATE_NUMBER],[BIRTH_DATE],[SUB_TYPE],[Region],[Raion],[City],[Village],[RP_Address],[FACT_ADDRESS],[MotherLast],[MotherFirst],[MotherPrivateNumber],[MotherBirthDate],[FatherLast],[FatherFirst],[FatherPrivateNumber],[FatherBirthDate],[DeathRegistrationDate],[DeathDate],[CONDITION_DESCRIPTION],[APPD_DATE],[APPD_STATUS_DESCRIPTION],[ForegnCountry],[Void_Address],[Without_Address],[Gakrechilia],[Unnom],[Rai] from [Pirvelckaro_23_BAVSHVEBI(165)] (nolock)
union all 
select [RecId],[RecDate],[Base_Type],[SourceDataId],[FIRST],[LAST],[GENDER],[PRIVATE_NUMBER],[BIRTH_DATE],[SUB_TYPE],[Region],[Raion],[City],[Village],[RP_Address],[FACT_ADDRESS],[MotherLast],[MotherFirst],[MotherPrivateNumber],[MotherBirthDate],[FatherLast],[FatherFirst],[FatherPrivateNumber],[FatherBirthDate],[DeathRegistrationDate],[DeathDate],[CONDITION_DESCRIPTION],[APPD_DATE],[APPD_STATUS_DESCRIPTION],[ForegnCountry],[Void_Address],[Without_Address],[Gakrechilia],[Unnom],[Rai] from [Pirvelckaro_27_AXALSHOBILEBI(165)] (nolock)"},
{24,"select * from [Pirvelckaro_24_INVALIDI_BAVSHVEBI] (nolock)"},
{25,"select * from [Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI] (nolock)"},
{26,"select * from [Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI] (nolock)"},
            };
        static int LastUnnom(this SqlConnection conn, int unnom)
        {
            var newUnnom = conn.Query<int>("select NewUnnom from UketesiReestri..UnnomisConventori where OldUnnom=@unnom", new { unnom = unnom })
                .ToList();
            return newUnnom.Count > 0 ? newUnnom[0] : unnom;
        }
        public static IList<IDictionary<string, object>> PirvelckarosChanacerebi(this SqlConnection conn, int basetype, int unnom)
        {
            var dublebi = conn.Query(
                "select * from UketesiReestri..UnnomisConventori where OldUnnom=@unnom or NewUnnom=@unnom",
                new { unnom = unnom }).ToList();

            var unnomebi = dublebi.Select(x => (int)x.OldUnnom).Concat(dublebi.Select(x => (int)x.NewUnnom)).Concat(new[]{unnom}).Distinct();
            return conn.Query(@"SELECT *
FROM (" + pirvelckaroebi[basetype] + @")  g 
where Unnom in (" + string.Join(",", unnomebi) + @") order by RecDate")
                                              .Cast<IDictionary<string, object>>()
                                              .ToList();
        }

        public static dynamic BoloKargiChanaceri(this SqlConnection conn, int unnom)
        {
            return conn.Query(@"SELECT *
FROM [UketesiReestri].[dbo].vUnnomBoloKargiChanaceri (nolock) 
WHERE Unnom = @unnom", new { unnom = conn.LastUnnom(unnom) }).FirstOrDefault();

        }

        public static IList<dynamic> PirovnebisRekvizitebisIstoria(this SqlConnection conn, int unnom)
        {
            return conn.Query(@"SELECT *
FROM [UketesiReestri].[dbo].UnnomShesadarebeliReestri (nolock) 
WHERE Unnom = @unnom
ORDER BY Tarigi", new { unnom = conn.LastUnnom(unnom) }).ToList();

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