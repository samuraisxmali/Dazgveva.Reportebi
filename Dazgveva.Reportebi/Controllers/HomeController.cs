using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Dazgveva.Reportebi.Models;
using Dapper;
using Dazgveva.Reportebi.Queries;

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

        private Tuple<string, object> WhereNacili(string q, string pid, string fid, string tarigi, string sakheli, string gvari, string unnom)
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

            var savaraudoUnnomebi = sadzieboTekstebi.Where(x =>
                                                               {
                                                                   var u = default(int);
                                                                   if(Int32.TryParse(x, out u)) return true;
                                                                   return false;
                                                               })
                                                    .Except(pidebi)
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
            else if (savaraudoFidebi.FirstOrDefault() != null && savaraudoUnnomebi.FirstOrDefault() != null)
                return Tuple.Create<string, object>(fid + "=@Fid OR " + unnom + "=@Unnom", new { Fid = savaraudoFidebi.FirstOrDefault(), Unnom = savaraudoUnnomebi.FirstOrDefault() });
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
            var whereNacili = WhereNacili(q, "PID", "FID", "BIRTH_DATE", "FIRST_NAME", "LAST_NAME", "Unnom");

            ViewBag.query = q;

            if (q == "") ViewBag.carieliq = true;

            var user = GetUser();
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                conn.Open();
                var kontraktebi = conn.MomeciKontraktebi(q, user.SadazgveosID);

                if (kontraktebi.Any())
                {
                    ViewBag.kontraqtebiarmoidzebna = false;
                    return View("Dzebna", kontraktebi.OrderBy(x => x.End_Date).OrderBy(x => x.FIRST_NAME).ToList());
                    
                }
                else
                {
                    ViewBag.Links = GetFileListi(user.Name).GroupBy(x =>
                        string.Format("{0}/{1}/{2}",
                              ((DateTime)x.ShekmnisTarigi).Day,
                              ((DateTime)x.ShekmnisTarigi).Month,
                              ((DateTime)x.ShekmnisTarigi).Year));

                    ViewBag.kontraqtebiarmoidzebna = true;
                    return View("Dzebna", new List<Kontrakti>());
                    
                }

            }
        }

        public PartialViewResult Periodebi(int id)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                conn.Open();
                var periodebi = conn.MomeKontraktisPeriodebi(id)
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