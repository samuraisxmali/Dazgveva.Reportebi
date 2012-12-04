using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using Dazgveva.Reportebi.Models;
using Dapper;

namespace Dazgveva.Reportebi.Controllers
{
    public class FamiliData
    {
        public string FID { get; set; }
        public DateTime? SCORE_DATE { get; set; }
        public int? FAMILY_SCORE { get; set; }
        public int Periodi { get; set; }
        public int ShemadgenlobaHash { get; set; }
        public List<FamiliDataCevri> Cevrebi { get; set; }
    }

    public class DeklarirebisIstoria
    {
        public int FID_VERSION { get; set; }
        public List<DeklaraciebisIstoriaList> istoria { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SadazgveosID { get; set; }
    }

    public class Links
    {
        public string FailisHash { get; set; }
        public string Shinaarsi { get; set; }
        public string DownloadLink { get; set; }
        public DateTime? ShekmnisTarigi { get; set; }
        public string Kompania { get; set; }
        public string FailisSaxeli { get; set; }
    }

    [Authorize]
    public class HomeController : Controller
    {
        private User GetUser()
        {
            if (Request.IsAuthenticated == true)
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
                {
                    conn.Open();
                    return
                        conn.Query<User>(@"SELECT * FROM INSURANCEW.dbo.SadazgveoebisAccountebi WHERE Name = @name",
                                         new { name = HttpContext.User.Identity.Name }).FirstOrDefault();
                }
            else
                return null;
        }

        private Tuple<string, object> WhereNacili(string q, string pid, string fid, string tarigi, string sakheli, string gvari)
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

        public ActionResult Index()
        {
            return RedirectToAction("Dzebna");
        }

        private static IEnumerable<string> DirSearch(string sDir)
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach (string f in Directory.GetFiles(d, "*.accdb"))
                    yield return f;
                foreach (var f in DirSearch(d))
                    yield return f;
            }
        }

        private static List<Links> GetFileListi(string user)
        {
            var data = DeserializeJson(@"http://172.17.7.40/sadazgveoebi_rest/api/List?name=" + user);
            return data;
        }

        public ActionResult Dzebna(string q = "")
        {
            var whereNacili = WhereNacili(q, "PID", "FID", "BIRTH_DATE", "FIRST_NAME", "LAST_NAME");

            ViewBag.query = q;

            if (q == "") ViewBag.carieliq = true;

            var user = GetUser();

            if (whereNacili != null)
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
                {
                    conn.Open();
                    string sql = @"" +
                        "SELECT d.*, " +
                        "m.RAI as aRAI, m.CITY as aCITY, m.ADDRESS_FULL as aADDRESS_FULL, " +
                        "p.KontraktisNomeri as GAUKMEBULI, p.Pirovneba as VIN_GAAUQMA, " +
                        "s.Ganmarteba " +
                        "FROM INSURANCEW.dbo.DAZGVEVA_201212 (nolock) d " +
                        "left join INSURANCEW.dbo.StatusebisGanmarteba s ON d.STATE_201212 = s.Statusi " +
                        "left join INSURANCEW.dbo.aMisamartebi m ON d.ID = m.ID " +
                        "left join INSURANCEW.dbo.KontraktisGauqmeba p on d.ID = p.KontraktisNomeri " +
                        "WHERE d.Company_ID_201212 = " + user.SadazgveosID + " AND ";

                    var kontraktebi = conn.Query(sql + whereNacili.Item1, whereNacili.Item2)
                        .ToList()
                        .Select(d => new Kontrakti
                        {
                            ID = d.ID,
                            Base_Description = d.Base_Description,
                            Unnom = d.Unnom,
                            FID = d.FID,
                            PID = d.PID,
                            FIRST_NAME = d.FIRST_NAME,
                            LAST_NAME = d.LAST_NAME,
                            BIRTH_DATE = d.BIRTH_DATE,
                            RAI = d.RAI,
                            CITY = d.CITY,
                            ADDRESS_FULL = d.ADDRESS_FULL,
                            aRAI = d.aRAI,
                            aCITY = d.aCITY,
                            aADDRESS_FULL = d.aADDRESS_FULL,
                            //dagv_tar = (DateTime?)((IDictionary<string, object>)d)["dagv-tar"],
                            dagv_tar = d.dagv__tar,
                            STATE = d.STATE_201212,
                            ADD_DATE = d.ADD_DATE_201212_TMP,
                            CONTINUE_DATE = d.CONTINUE_DATE_201212_TMP,
                            STOP_DATE = d.STOP_DATE_201212_TMP,
                            Company = d.Company_201212,

                            End_Date = d.End_Date,
                            POLISIS_NOMERI = d.POLISIS_NOMERI,
                            GAUKMEBULI = d.GAUKMEBULI,
                            VIN_GAAUQMA = d.VIN_GAAUQMA,
                            Ganmarteba = d.Ganmarteba
                        })
                        .ToList();

                    if (kontraktebi.Count() == 0)
                        ViewBag.kontraqtebiarmoidzebna = true;

                    return View("Dzebna", kontraktebi.OrderBy(x => x.End_Date).OrderBy(x => x.FIRST_NAME).ToList());
                }

            ViewBag.Links = GetFileListi(user.Name).GroupBy(
                x =>
                string.Format("{0}/{1}/{2}",
                              ((DateTime) x.ShekmnisTarigi).Day,
                              ((DateTime) x.ShekmnisTarigi).Month,
                              ((DateTime) x.ShekmnisTarigi).Year
                    )
                );

            ViewBag.kontraqtebiarmoidzebna = true;
            return View("Dzebna", new List<Kontrakti>());
        }

        public PartialViewResult Periodebi(int id)
        {
            using (var dc = new InsuranceWDataContext())
            {
                var kontraktebi = dc.DAZGVEVA_201212s.Where(x => x.ID == id).ToList();
                var periodebi = kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201006, Dasabechdi = (int?)null, State = d.STATE_06, CONTINUE_DATE = d.CONTINUE_DATE_06, Company = d.Company_06, STOP_DATE = d.STOP_DATE_06, ADD_DATE = d.ADD_DATE_06 })
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201007, Dasabechdi = (int?)null, State = d.STATE_07, CONTINUE_DATE = d.CONTINUE_DATE_07, Company = d.Company_07, STOP_DATE = d.STOP_DATE_07, ADD_DATE = d.ADD_DATE_07 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201008, Dasabechdi = (int?)null, State = d.STATE_08, CONTINUE_DATE = d.CONTINUE_DATE_08, Company = d.Company_08, STOP_DATE = d.STOP_DATE_08, ADD_DATE = d.ADD_DATE_08 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201009, Dasabechdi = (int?)null, State = d.STATE_09, CONTINUE_DATE = d.CONTINUE_DATE_09, Company = d.Company_09, STOP_DATE = d.STOP_DATE_09, ADD_DATE = d.ADD_DATE_09 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201010, Dasabechdi = (int?)null, State = d.STATE_10, CONTINUE_DATE = d.CONTINUE_DATE_10, Company = d.Company_10, STOP_DATE = d.STOP_DATE_10, ADD_DATE = d.ADD_DATE_10 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201011, Dasabechdi = (int?)null, State = d.STATE_11, CONTINUE_DATE = d.CONTINUE_DATE_11, Company = d.Company_11, STOP_DATE = d.STOP_DATE_11, ADD_DATE = d.ADD_DATE_11 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201012, Dasabechdi = (int?)null, State = d.STATE_12, CONTINUE_DATE = d.CONTINUE_DATE_12, Company = d.Company_12, STOP_DATE = d.STOP_DATE_12, ADD_DATE = d.ADD_DATE_12 }))

                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201101, Dasabechdi = (int?)null, State = d.STATE_201101, CONTINUE_DATE = d.CONTINUE_DATE_201101, Company = d.Company_201101, STOP_DATE = d.STOP_DATE_201101, ADD_DATE = d.ADD_DATE_201101 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201102, Dasabechdi = (int?)null, State = d.STATE_201102, CONTINUE_DATE = d.CONTINUE_DATE_201102, Company = d.Company_201102, STOP_DATE = d.STOP_DATE_201102, ADD_DATE = d.ADD_DATE_201102 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201103, Dasabechdi = (int?)null, State = d.STATE_201103, CONTINUE_DATE = d.CONTINUE_DATE_201103, Company = d.Company_201103, STOP_DATE = d.STOP_DATE_201103, ADD_DATE = d.ADD_DATE_201103 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201104, Dasabechdi = (int?)null, State = d.STATE_201104, CONTINUE_DATE = d.CONTINUE_DATE_201104, Company = d.Company_201104, STOP_DATE = d.STOP_DATE_201104, ADD_DATE = d.ADD_DATE_201104 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201105, Dasabechdi = (int?)null, State = d.STATE_201105, CONTINUE_DATE = d.CONTINUE_DATE_201105, Company = d.Company_201105, STOP_DATE = d.STOP_DATE_201105, ADD_DATE = d.ADD_DATE_201105 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201106, Dasabechdi = (int?)null, State = d.STATE_201106, CONTINUE_DATE = d.CONTINUE_DATE_201106, Company = d.Company_201106, STOP_DATE = d.STOP_DATE_201106, ADD_DATE = d.ADD_DATE_201106 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201107, Dasabechdi = d.DASABECHDI_201107, State = d.STATE_201107, CONTINUE_DATE = d.CONTINUE_DATE_201107, Company = d.Company_201107, STOP_DATE = d.STOP_DATE_201107, ADD_DATE = d.ADD_DATE_201107 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201108, Dasabechdi = d.DASABECHDI_201108, State = d.STATE_201108, CONTINUE_DATE = d.CONTINUE_DATE_201108, Company = d.Company_201108, STOP_DATE = d.STOP_DATE_201108, ADD_DATE = d.ADD_DATE_201108 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201109, Dasabechdi = d.DASABECHDI_201109, State = d.STATE_201109, CONTINUE_DATE = d.CONTINUE_DATE_201109, Company = d.Company_201109, STOP_DATE = d.STOP_DATE_201109, ADD_DATE = d.ADD_DATE_201109 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201110, Dasabechdi = d.DASABECHDI_201110, State = d.STATE_201110, CONTINUE_DATE = d.CONTINUE_DATE_201110, Company = d.Company_201110, STOP_DATE = d.STOP_DATE_201110, ADD_DATE = d.ADD_DATE_201110 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201111, Dasabechdi = d.DASABECHDI_201111, State = d.STATE_201111, CONTINUE_DATE = d.CONTINUE_DATE_201111, Company = d.Company_201111, STOP_DATE = d.STOP_DATE_201111, ADD_DATE = d.ADD_DATE_201111 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201112, Dasabechdi = d.DASABECHDI_201112, State = d.STATE_201112, CONTINUE_DATE = d.CONTINUE_DATE_201112, Company = d.Company_201112, STOP_DATE = d.STOP_DATE_201112, ADD_DATE = d.ADD_DATE_201112 }))

                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201201, Dasabechdi = d.DASABECHDI_201201, State = d.STATE_201201, CONTINUE_DATE = d.CONTINUE_DATE_201201, Company = d.Company_201201, STOP_DATE = d.STOP_DATE_201201, ADD_DATE = d.ADD_DATE_201201 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201202, Dasabechdi = d.DASABECHDI_201202, State = d.STATE_201202, CONTINUE_DATE = d.CONTINUE_DATE_201202, Company = d.Company_201202, STOP_DATE = d.STOP_DATE_201202, ADD_DATE = d.ADD_DATE_201202 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201203, Dasabechdi = d.DASABECHDI_201203, State = d.STATE_201203, CONTINUE_DATE = d.CONTINUE_DATE_201203, Company = d.Company_201203, STOP_DATE = d.STOP_DATE_201203, ADD_DATE = d.ADD_DATE_201203 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201204, Dasabechdi = d.DASABECHDI_201204, State = d.STATE_201204, CONTINUE_DATE = d.CONTINUE_DATE_201204, Company = d.Company_201204, STOP_DATE = d.STOP_DATE_201204, ADD_DATE = d.ADD_DATE_201204 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201205, Dasabechdi = d.DASABECHDI_201205, State = d.STATE_201205, CONTINUE_DATE = d.CONTINUE_DATE_201205, Company = d.Company_201205, STOP_DATE = d.STOP_DATE_201205, ADD_DATE = d.ADD_DATE_201205 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201206, Dasabechdi = d.DASABECHDI_201206, State = d.STATE_201206, CONTINUE_DATE = d.CONTINUE_DATE_201206, Company = d.Company_201206, STOP_DATE = d.STOP_DATE_201206, ADD_DATE = d.ADD_DATE_201206 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201207, Dasabechdi = d.DASABECHDI_201207, State = d.STATE_201207, CONTINUE_DATE = d.CONTINUE_DATE_201207, Company = d.Company_201207, STOP_DATE = d.STOP_DATE_201207, ADD_DATE = d.ADD_DATE_201207 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201208, Dasabechdi = d.DASABECHDI_201208, State = d.STATE_201208, CONTINUE_DATE = d.CONTINUE_DATE_201208, Company = d.Company_201208, STOP_DATE = d.STOP_DATE_201208, ADD_DATE = d.ADD_DATE_201208 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201209, Dasabechdi = d.DASABECHDI_201209, State = d.STATE_201209, CONTINUE_DATE = d.CONTINUE_DATE_201209, Company = d.Company_201209, STOP_DATE = d.STOP_DATE_201209, ADD_DATE = d.ADD_DATE_201209 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201210, Dasabechdi = d.DASABECHDI_201210, State = d.STATE_201210, CONTINUE_DATE = d.CONTINUE_DATE_201210, Company = d.Company_201210, STOP_DATE = d.STOP_DATE_201210, ADD_DATE = d.ADD_DATE_201210 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201211, Dasabechdi = d.DASABECHDI_201211, State = d.STATE, CONTINUE_DATE = d.CONTINUE_DATE, Company = d.Company, STOP_DATE = d.STOP_DATE, ADD_DATE = d.ADD_DATE }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201212, Dasabechdi = d.DASABECHDI_201212, State = d.STATE_201212, CONTINUE_DATE = d.CONTINUE_DATE_201212_TMP, Company = d.Company_201212, STOP_DATE = d.STOP_DATE_201212_TMP, ADD_DATE = d.ADD_DATE_201212_TMP }))

                        .GroupBy(p => new { p.ID, p.Dasabechdi, p.State, p.CONTINUE_DATE, p.Company, p.STOP_DATE, p.ADD_DATE })
                        .Select(g => g.First(x => x.Periodi == g.Min(x_ => x_.Periodi)))
                        .OrderBy(x => x.Periodi)
                        .ToList();

                return PartialView(periodebi);
            }
        }

        public PartialViewResult Gadarickhvebi(int id)
        {
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                conn.Open();
                var gadarickhvebi = conn.Query<Gadarickhva>(
                    @"SELECT ID , OP_DATE , TRANSFER_DATE , Company_ID , [TRANSFER] FROM [INSURANCEW].[dbo].GADARICXVA_FULL where ID=@Id
                    UNION
                    SELECT ID , OP_DATE , TRANSFER_DATE , Company_ID , [TRANSFER]+isnull(DANAMATI,0) [TRANSFER] FROM [INSURANCEW].[dbo].GADARICXVA_FULL_165 where ID=@Id"
                    , new { Id = id }).ToList();
                return PartialView(gadarickhvebi);
            }
        }

        public PartialViewResult PolisisChabarebisIstoria(string polisisNomeri = "")
        {
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                conn.Open();
                return PartialView(conn.Query(@"select PaketisNomeri,PolisisNomeri,VizitisTarigi,case when Chambarebeli='AdgilidanGacema' or Chambarebeli='Socagenti' or Chambarebeli='Fosta' or Chambarebeli='Banki' then Chambarebeli else N'გაიცა ადგილიდან ('+Chambarebeli+')' end Chambarebeli, Statusi from SocialuriDazgveva.dbo.PolisisChabarebisIstoria where PolisisNomeri = @pol order by VizitisTarigi", new { pol = polisisNomeri }).ToList());
            }
        }

        [OutputCache(Duration = 600)]
        public ActionResult PrvelckaroebisMocvdisTarigebi()
        {
            using (var con = new SqlConnection(@"Data Source=triton;Initial Catalog=Pirvelckaroebi;User ID=sa;Password=ssa$20"))
            {
                con.Open();
                var list = con.Query(@"select	p.ProgramisId, p.ProgramisDasakheleba, mapDates.MaxMapDate
from	SocialuriDazgveva.dbo.Programebi (nolock) p
join	(select Base_Type, MAX(MapDate) MaxMapDate
		from Pirvelckaroebi.dbo.Source_Data (nolock)
		group by Base_Type) mapDates on mapDates.Base_Type = p.ProgramisId
ORDER BY p.ProgramisId", commandTimeout: 120).ToList();
                return PartialView(list);
            }
        }

        public FileContentResult Gadmowera(string hash)
        {
            var user = GetUser();

            var dict = GetFileListi(user.Name);

            var result = dict.Where(x => x.FailisHash == hash).ToList().First();

            WebClient client = new WebClient();
            byte[] data = client.DownloadData(result.DownloadLink);

            return File(data, "application/msaccess", result.FailisSaxeli);
        }

        private static List<Links> DeserializeJson(string requestUrl)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string result = client.DownloadString(requestUrl);

                var js = new JavaScriptSerializer();

                return js.Deserialize<List<Links>>(result);
            }
        }
    }
}