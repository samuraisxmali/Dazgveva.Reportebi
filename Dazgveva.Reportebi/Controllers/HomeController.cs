using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dazgveva.Reportebi.Models;
using System.Net;
using System.Text;
using Dapper;
using MvcApplication2.Models;
using Newtonsoft.Json;
using Dazgveva.Reportebi.Models.Ganckhadebebi;
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



    [Authorize]
    public class HomeController : Controller
    {
        // @done
        
        // @done
        public ActionResult Index()
        {
            return RedirectToAction("Dzebna");
        }


        // @done
        public ActionResult Dzebna(string q = "")
        {

            ViewBag.query = q;
            if (q == "") ViewBag.carieliq = true;
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                conn.Open();
                var kontraktebi = conn.MomeciKontraktebi(q);

                if (!kontraktebi.Any())
                    ViewBag.kontraqtebiarmoidzebna = true;

                return View("Dzebna", kontraktebi.OrderBy(x => x.End_Date).OrderBy(x => x.FIRST_NAME).ToList());
            }
        }

        private User CurrentUser
        {
            get
            {
                var user = default(User);
                using (var dc = new UsersDataContext())
                    user = dc.Users.First(x => x.USER_NAME == HttpContext.User.Identity.Name);
                return user;
            }
        }

        public ActionResult Ganckhadebebi(string q = "")
        {
            var currentUser = CurrentUser;
            if (currentUser == null || currentUser.ROLE_NAME == "მესამე პირი")
            {
                return RedirectToAction("Dzebna");
            }
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PirvelckaroebiConnectionString1"].ConnectionString))
            {
                conn.Open();
                var rez = conn.Ganckhadebebi(q)
                   .GroupBy(x => x.Unnom)
                   .Select(g =>
                   {
                       dynamic o = new ExpandoObject();
                       o.Pirovneba = conn.BoloKargiChanaceri((int)g.Key);
                       o.PirovnebisRekvizitebisIstoria = conn.PirovnebisRekvizitebisIstoria((int)g.Key);
                       o.ReestrisStatusebisIstoria = conn.ReestrisStatusebisIstoria((string) o.Pirovneba.IdentPID);
                       o.Ganckhadebebi = g.ToList();
                       return o;
                   })
                   .ToList();
                return View("ShemosuliGancxadebebi", rez);
            }
        }

        public ActionResult PirvelckarosChanacerebi(int basetype, int unnom)
        {
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PirvelckaroebiConnectionString1"].ConnectionString))
            {
                conn.Open();
                var rez = conn.PirvelckarosChanacerebi(basetype, unnom).ToList();
                ViewBag.PirvelckarosCkhrili = basetype;
                return View("PirvelckarosChanacerebi", rez);
            }
        }


        // @not needed
        public ActionResult SourceData(string q = "")
        {
           return RedirectToAction("Dzebna");
        }

        // ++++++++++++ fmiyc
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
        // @done
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

        // ++++++++++++ fmiyc ^ 2
        public string FamiliDatas(string id = "")
        {
              return "N/A";
        }

        // @done
        public PartialViewResult DeklaraciebisIstoria(string id = "")
        {
            using (var p = new Pirvelckaroebi2DataContext())
            {
                var form3 = p.vForm_3s.FirstOrDefault(x => x.FID == id);
                ViewBag.form3 = form3;

                var istoria = p.VDeklaraciebisIstorias
                    .Where(x => x.FID == id)
                    .AsEnumerable()
                    .GroupBy(x => x.FID_VERSION)
                    .Select(s => new DeklarirebisIstoria
                    {
                        FID_VERSION = s.Key,
                        istoria = s.Select(g => new DeklaraciebisIstoriaList
                                                    {
                                                        MOB_PHONE = g.MOB_PHONE,
                                                        LAST_NAME = g.LAST_NAME,
                                                        BIRTH_DATE = g.BIRTH_DATE,
                                                        CALC_DATE = g.CALC_DATE,
                                                        FID = g.FID,
                                                        FID_VERSION = g.FID_VERSION,
                                                        FIRST_NAME = g.FIRST_NAME,
                                                        FULL_ADDRESS = g.FULL_ADDRESS,
                                                        HOME_PHONE = g.HOME_PHONE,
                                                        LEGAL_SCORE_DATE = g.LEGAL_SCORE_DATE,
                                                        ON_CONTROL = g.ON_CONTROL,
                                                        PID = g.PID,
                                                        ACTION_TYPE = g.ACTION_TYPE,
                                                        RESTORE_DOC_DATE = g.RESTORE_DOC_DATE
                                                    }).ToList()
                    })
                    .ToList();

                return PartialView(istoria);
            }
        }
        // @done
        public PartialViewResult PolisisChabarebisIstoria(string polisisNomeri = "")
        {
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                conn.Open();

                string sql = "select PiradiNomeri,p.PolisisNomeri,Damrigebeli from ( select PolisisNomeri,'Fosta' as Damrigebeli from SocialuriDazgveva.dbo.GadaecaFostas union all select PolisisNomeri, 'Raioni' as Damrigebeli from SocialuriDazgveva.dbo.GadaecaRaions union all select PolisisNomeri,'Banki' as Damrigebeli from SocialuriDazgveva.dbo.BankzeGadasacemiPaketisPolisebi_all ) pol join (select distinct PiradiNomeri, PolisisNomeri from SocialuriDazgveva.dbo.Polisebi) p on pol.PolisisNomeri = p.PolisisNomeri where p.PolisisNomeri = @polisisNomeri";
                ViewBag.damrigebeli = conn.Query(sql, new { polisisNomeri = polisisNomeri });

                return PartialView(conn.Query(@"
                    select   i.PaketisNomeri
                            ,i.PolisisNomeri
                            ,i.VizitisTarigi
                            ,case when i.Chambarebeli='AdgilidanGacema' or i.Chambarebeli='Socagenti' or i.Chambarebeli='Fosta' or i.Chambarebeli='Banki' then i.Chambarebeli else N'გაიცა ადგილიდან ('+i.Chambarebeli+')' end Chambarebeli
                            , i.Statusi 
                            , c.Dro ChvenzeMocvdisDro
                    from SocialuriDazgveva.dbo.PolisisChabarebisIstoria i
                    join SocialuriDazgveva.dbo.MovlenaChabarebebi c on c.PaketisNomeri=i.PaketisNomeri and c.PolisisNomeri=i.PolisisNomeri
                    where i.PolisisNomeri = @pol 
                    order by i.VizitisTarigi
                ", new { pol = polisisNomeri }).ToList());
            }
        }
        // @done
        public PartialViewResult PirovnebisPeriodebi(string pid = "")
        {
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                conn.Open();
                var sql = @"
                     SELECT * FROM (
                        SELECT [PID] ,[Mnishvneloba] ,[Dan] ,[Mde] ,'Mokalakeoba' Tipi
                        FROM [UketesiReestri].[dbo].[PirovnebisMokalakeobisCvlilebebisPeriodebi]
                        union all
                        SELECT [PID], [Mnishvneloba], [Dan], [Mde], 'Statusi'
                        FROM [UketesiReestri].[dbo].[PirovnebisStatusisCvlilebebisPeriodebi]
                        union all
                        SELECT [PID], [Mnishvneloba], [Dan], [Mde], 'NeitMocmoba'
                        FROM [UketesiReestri].[dbo].[PirovnebisNeitraluriMocmobisCvlilebebisPeriodebi]
                    ) t
                    WHERE t.PID = @pid
                ";

                ViewBag.periodebi = conn.Query(sql, new { pid = pid });

                return PartialView();
            }
        }
        [HttpPost]
        // ++++++++++++ NOT YET
        public RedirectResult Gaukmeba(int kontraktisNomeri, string werilisNomeri, string paroli)
        {

            var passwords = new Dictionary<string, string>();
            passwords.Add("Os7b6cu8JB", "ეთერ კიღურაძე");

            if (passwords.ContainsKey(paroli))
            {
                using (var con = new SqlConnection(@"Data Source=triton;Initial Catalog=Pirvelckaroebi;User ID=sa;Password=ssa$20"))
                {
                    con.Open();
                    con.Execute(@"INSERT INTO INSURANCEW.dbo.KontraktisGauqmeba(KontraktisNomeri, Pirovneba, WerilisNomeri) VALUES(@KontraktisNomeri, @Pirovneba, @WerilisNomeri)",
                        new { KontraktisNomeri = kontraktisNomeri, Pirovneba = passwords[paroli], WerilisNomeri = werilisNomeri });
                }
            }

            return Redirect(Request.UrlReferrer.ToString());
        }
        // @done
        public string Reestri(string pid = "")
        {
            var currentUser = CurrentUser;
            if (currentUser == null || currentUser.ROLE_NAME == "მესამე პირი")
            {
                return "";
            }
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            return client.DownloadString(@"http://172.17.8.125/PirovnebisZebna/Person/FragmentiPid?PiradiNomeri=" + pid);
        }
        // @done
        public FileResult Amonaceri(string pid = "")
        {
            WebClient client = new WebClient();

            byte[] data = client.DownloadData(@"http://172.17.8.125/CRA_Rest/SSA/AmonaceriUmceotaBazidan?pid=" + pid);

            return File(data, "application/pdf", pid + ".pdf");
        }
        // ++++++++++++ NOT YET
        [OutputCache(Duration = 600)]
        public ActionResult PrvelckaroebisMocvdisTarigebi()
        {
            using (var con = new SqlConnection(@"Data Source=triton;Initial Catalog=Pirvelckaroebi;User ID=sa;Password=ssa$20"))
            {
                con.Open();
                var list = con.Query(@"SELECT p.ProgramisId, p.ProgramisDasakheleba, mapDates.MaxMapDate 
FROM (        SELECT max(RecDate) MaxMapDate, 1 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_01_UMCEOEBI]
    UNION ALL SELECT max(RecDate), 2 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_02_DEVNILEBI]
    UNION ALL SELECT max(RecDate), 3 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_03_BAVSHVEBI]
    UNION ALL SELECT max(RecDate), 4 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_04_REINTEGRACIA]
    UNION ALL SELECT max(RecDate), 5 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_05_KULTURA]
    UNION ALL SELECT max(RecDate), 6 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_06_XANDAZMULEBI]
    UNION ALL SELECT max(RecDate), 7 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_07_SKOLA_PANSIONEBI]
    UNION ALL SELECT max(RecDate), 8 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_08_TEACHERS]
    UNION ALL SELECT max(RecDate), 9 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_09_UFROSI_AGMZRDELEBI]
    UNION ALL SELECT max(RecDate), 10 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_10_APKHAZETIS_OJAKHEBI]
    UNION ALL SELECT max(RecDate), 100 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_100_DevniltaMisamartebi_201210]
    UNION ALL SELECT max(RecDate), 11 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_11_SATEMO]
    UNION ALL SELECT max(RecDate), 12 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_12_MCIRE_SAOJAXO]
    UNION ALL SELECT max(RecDate), 13 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_13_TEACHERS_AFX]
    UNION ALL SELECT max(RecDate), 14 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_14_RESURSCENTRIS_TANAMSHROMLEBI]
    UNION ALL SELECT max(RecDate), 21 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_21_SAPENSIO_ASAKIS_MOSAXLEOBA]
    UNION ALL SELECT max(RecDate), 22 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_22_STUDENTEBI]
    UNION ALL SELECT max(RecDate), 23 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_23_BAVSHVEBI(165)]
    UNION ALL SELECT max(RecDate), 24 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_24_INVALIDI_BAVSHVEBI]
    UNION ALL SELECT max(RecDate), 25 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_25_MKVETRAD_GAMOXATULI_INVALIDI_BAVSHVEBI]
    UNION ALL SELECT max(RecDate), 26 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_26_ARASAQARTVELOS_MOQALAQE_PENSIONREBI]
    UNION ALL SELECT max(RecDate), 27 Base_Type FROM Pirvelckaroebi..[Pirvelckaro_27_AXALSHOBILEBI(165)]
    ) mapDates
join SocialuriDazgveva.dbo.Programebi  p on mapDates.Base_Type = p.ProgramisId
ORDER BY p.ProgramisId", commandTimeout: 120).ToList();
                return PartialView(list);
            }
        }
        // განცხადებები
        public PartialViewResult Gancxadebebi(string q = "")
        {
            using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                conn.Open();

                var sql = @"
		            SELECT g.*, s.ProgramisDasakheleba
		            FROM [DazgvevaGanckhadebebi].[dbo].[Ganckhadebebi] g
                    JOIN [SocialuriDazgveva].[dbo].[Programebi] s on g.Base_Type = s.ProgramisId
		            WHERE g.Pid = @pid
                    AND not exists (select null 
                        from [DazgvevaGanckhadebebi].[dbo].[GaukmebuliGanckhadebebi] 
                        where GaukmebuliGanckhId=g.Id
                    )
		            ORDER BY
			            g.DadasturebisTarigi DESC,
			            g.StatusisMopovebisTarigi ASC
                ";

                var result = conn.Query(sql, new { pid = q });

                return PartialView(result);
            }
        }














        //just returns partial view
        public PartialViewResult Koreqtireba(int ID)
        {
            var damkoreqtirebeli = new Damkoreqtirebeli();
            using (var conn = damkoreqtirebeli.GaxseniKavshiri())
            {

                var dasakoreqtirebeliKontraqti = damkoreqtirebeli.CamoigeDasakoreqtirebeliKontraqti(ID, conn);
                return PartialView(dasakoreqtirebeliKontraqti);
            }

        }

        public class RaiC
        {
            public string Rai_Name;
            public string Rai;
        }

        public JsonResult MomeRaionebi(string val)
        {


            using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {

                con.Open();
                var reqResult = con.Query<RaiC>("select Rai_Name,Rai from Pirvelckaroebi.dbo.RRC").ToList();
                var json = JsonConvert.SerializeObject(reqResult);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public void Save(int ID, string tags, string cityInput, string villageInput, string AddressInput)
        {
            Damkoreqtirebeli larisa = new Damkoreqtirebeli();
            using (var con = larisa.GaxseniKavshiri())
            {
                con.Open();

                larisa.DaakoreqtireMisamarti(ID, tags, cityInput, villageInput, AddressInput, con);


            }
        }



        public JsonResult ReestridanDatreva(string PID)
        {

            using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                con.Open();
                Damkoreqtirebeli larisa = new Damkoreqtirebeli();
                var reestrisInfo = larisa.CamoigeReestridan(PID);
                var json = JsonConvert.SerializeObject(reestrisInfo);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ReestridanDatrevaP(string PID, int ID)
        {
            using (var con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
            {
                con.Open();
                Damkoreqtirebeli larisa = new Damkoreqtirebeli();
                var reestrisInfo = larisa.CamoigeReestridan(PID);
                var json = JsonConvert.SerializeObject(reestrisInfo);
                string birthDate = reestrisInfo.BIRTH_DATE.HasValue ? reestrisInfo.BIRTH_DATE.Value.ToString("yyyyMMdd") : null;
                larisa.DaakoreqtirePiradiMonacemebi(ID, reestrisInfo.PID, reestrisInfo.FIRST_NAME, reestrisInfo.LAST_NAME, birthDate, con);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
        }



    }
}
