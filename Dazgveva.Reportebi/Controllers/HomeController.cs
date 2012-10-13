﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dazgveva.Reportebi.Models;
using System.Net;
using System.Text;
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

    public class HomeController : Controller
    {
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

        public ActionResult Dzebna(string q = "")
        {
            var whereNacili = WhereNacili(q, "PID", "FID", "BIRTH_DATE", "FIRST_NAME", "LAST_NAME");

            var status = new Dictionary<string, string>();

            status.Add("-32", "გაუქმებული ხარვეზის გამო");
            status.Add("-27", "ხარვეზიანი პოლისი");
            status.Add("-26", "გადაზღვეულია კომპანია ვესტში");
            status.Add("-16", "დუბლირების გამო გაუქმებული კონტრაქტი");
            status.Add("-15", "გაუქმებული ხარვეზის გამო");
            status.Add("-5", "გარდაცვალება");
            status.Add("33", "დაზღვეულია");
            status.Add("44", "დაზღვეულია");
            status.Add("-31", "გაუქმებულია ხარვეზის გამო");
            status.Add("-30", "გაუქმებულია ხარვეზის გამო");
            status.Add("-25", "პოლისი გაუქმებულია ჩაუბარებლობის გამო");
            status.Add("-20", "გაუქმებული ხარვეზის გამო");
            status.Add("-6", "გაუქმებული ხარვეზის გამო");
            status.Add("0", "დასრულებული კონტრაქტი");
            status.Add("21", "დაზღვეულია");
            status.Add("22", "დაზღვეულია");

            ViewBag.status = status;
            ViewBag.query = q;

            if (q == "") ViewBag.carieliq = true;

            if (whereNacili != null)
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["INSURANCEWConnectionString"].ConnectionString))
                {
                    conn.Open();
                    var kontraktebi = conn.Query(@"SELECT d.*, m.RAI as aRAI, m.CITY as aCITY, m.ADDRESS_FULL as aADDRESS_FULL FROM INSURANCEW.dbo.DAZGVEVA_201210 (nolock) d left join INSURANCEW.dbo.aMisamartebi m ON d.ID = m.ID Where " + whereNacili.Item1, whereNacili.Item2)
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
                            STATE = d.STATE_201210,                    
                            ADD_DATE = d.ADD_DATE_201210_TMP,          
                            CONTINUE_DATE = d.CONTINUE_DATE_201210_TMP,
                            STOP_DATE = d.STOP_DATE_201210_TMP,        
                            Company = d.Company_201210,                

                            End_Date = d.End_Date,
                            POLISIS_NOMERI = d.POLISIS_NOMERI
                        })
                        .ToList();

                    if(kontraktebi.Count() == 0)
                        ViewBag.kontraqtebiarmoidzebna = true;

                    return View("Dzebna", kontraktebi.OrderBy(x => x.End_Date).OrderBy(x => x.FIRST_NAME).ToList());
                }

            ViewBag.kontraqtebiarmoidzebna = true;
            return View("Dzebna", new List<Kontrakti>());
        }

        public ActionResult SourceData(string q = "")
        {

            var whereNacili = WhereNacili(q, "PID", "FID", "Birth_Date", "First_Name", "Last_Name");
            if (whereNacili != null)
                using (var conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["PirvelckaroebiConnectionString1"].ConnectionString))
                {
                    conn.Open();
                    var sd = conn.Query<SourceData>(@"SELECT * FROM Pirvelckaroebi.dbo.Source_Data (nolock) sd WHERE " + whereNacili.Item1, whereNacili.Item2)
                                 .OrderBy(x => x.PID)
                                 .OrderBy(x => x.Periodi)
                                 .ToList();

                    return View("SourceData", sd);
                }
            else
                return View("SourceData", new List<SourceData>());
        }

        public PartialViewResult Periodebi(int id)
        {
            using (var dc = new InsuranceWDataContext())
            {
                var kontraktebi = dc.DAZGVEVA_201210s.Where(x => x.ID == id).ToList();
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
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201202, Dasabechdi = d.DASABECHDI_201202, State = d.STATE_201202, CONTINUE_DATE = d.CONTINUE_DATE_201202, Company = d.Company_201202, STOP_DATE = d.STOP_DATE_201202, ADD_DATE = d.ADD_DATE_201202}))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201203, Dasabechdi = d.DASABECHDI_201203, State = d.STATE_201203, CONTINUE_DATE = d.CONTINUE_DATE_201203, Company = d.Company_201203, STOP_DATE = d.STOP_DATE_201203, ADD_DATE = d.ADD_DATE_201203}))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201204, Dasabechdi = d.DASABECHDI_201204, State = d.STATE_201204, CONTINUE_DATE = d.CONTINUE_DATE_201204, Company = d.Company_201204, STOP_DATE = d.STOP_DATE_201204, ADD_DATE = d.ADD_DATE_201204 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201205, Dasabechdi = d.DASABECHDI_201205, State = d.STATE_201205, CONTINUE_DATE = d.CONTINUE_DATE_201205, Company = d.Company_201205, STOP_DATE = d.STOP_DATE_201205, ADD_DATE = d.ADD_DATE_201205 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201206, Dasabechdi = d.DASABECHDI_201206, State = d.STATE_201206, CONTINUE_DATE = d.CONTINUE_DATE_201206, Company = d.Company_201206, STOP_DATE = d.STOP_DATE_201206, ADD_DATE = d.ADD_DATE_201206 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201207, Dasabechdi = d.DASABECHDI_201207, State = d.STATE_201207, CONTINUE_DATE = d.CONTINUE_DATE_201207, Company = d.Company_201207, STOP_DATE = d.STOP_DATE_201207, ADD_DATE = d.ADD_DATE_201207 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201208, Dasabechdi = d.DASABECHDI_201208, State = d.STATE_201208, CONTINUE_DATE = d.CONTINUE_DATE_201208, Company = d.Company_201208, STOP_DATE = d.STOP_DATE_201208, ADD_DATE = d.ADD_DATE_201208 }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201209, Dasabechdi = d.DASABECHDI_201209, State = d.STATE, CONTINUE_DATE = d.CONTINUE_DATE, Company = d.Company, STOP_DATE = d.STOP_DATE, ADD_DATE = d.ADD_DATE }))
                        .Concat(kontraktebi.Select(d => new KontraktisPeriodi { ID = d.ID, Periodi = 201210, Dasabechdi = d.DASABECHDI_201210, State = d.STATE_201210, CONTINUE_DATE = d.CONTINUE_DATE_201210_TMP, Company = d.Company_201210, STOP_DATE = d.STOP_DATE_201210_TMP, ADD_DATE = d.ADD_DATE_201210_TMP }))

                        .GroupBy(p => new { p.ID, p.Dasabechdi, p.State,p.CONTINUE_DATE,p.Company,p.STOP_DATE,p.ADD_DATE })
                        .Select(g => g.First(x => x.Periodi == g.Min(x_ => x_.Periodi)))
                        .OrderBy(x => x.Periodi)
                        .ToList();

                return PartialView(periodebi);
            }
        }

        public PartialViewResult Gadarickhvebi(int id)
        {
            using (var dc = new InsuranceWDataContext())
            {
                var gadarickhvebi = dc.GADARICXVA_FULLs.Where(x => x.ID == id).OrderBy(x => x.OP_DATE)
                                    .AsEnumerable()
                                    .Select(g => new Gadarickhva { ID = g.ID, OP_DATE = g.OP_DATE, TRANSFER_DATE = g.TRANSFER_DATE, Company_ID = g.Company_ID, TRANSFER = g.TRANSFER })
                                    .ToList();
                return PartialView(gadarickhvebi);
            }
        }

        public PartialViewResult FamiliDatas(string id = "")
        {
            using (var dc = new InsuranceWDataContext())
            using (var dc2 = new Pirvelckaroebi2DataContext())
            {
                var ojakhuriPirvelckaro = new[] {1, 2, 10}.ToList();

                var sds = (   from sd in dc2.Source_Datas
                              from u in dc2.Pirvelckaro_01_UMCEOEBIs.Where(x=>x.SourceDataId==sd.ID).DefaultIfEmpty()
                              where (ojakhuriPirvelckaro.Contains(sd.Base_Type) && sd.FID == id) || (!ojakhuriPirvelckaro.Contains(sd.Base_Type) && sd.PID == id)
                              where 1 <= sd.Base_Type && sd.Base_Type <= 30
                              select new {
                                              sd.Base_Type, 
                                              Periodi = sd.MapDate.Year * 100 + sd.MapDate.Month, 
                                              FID = sd.FID, 
                                              PID = sd.PID, 
                                              FIRST_NAME = sd.FIRST_NAME,
                                              LAST_NAME = sd.LAST_NAME, 
                                              BIRTH_DATE = sd.BIRTH_DATE, 
                                              FAMILY_SCORE = u == null ? (int?)null : u.FAMILY_SCORE,
                                              SCORE_DATE = u == null ? (DateTime?)null : u.SCORE_DATE, 
                                              PIROBA = sd.Piroba 
                                          }
                          ).ToList();

                var fds = dc.FAMILY_DATA_201101s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201012, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA })
                  .Concat(dc.FAMILY_DATA_201102s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201101, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201103s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201102, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201104s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201103, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201105s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201104, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201106s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201105, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201107s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201106, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201108s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201107, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201109s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201108, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201110s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201109, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201111s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201110, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201112s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201111, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201201s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201112, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201202s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201201, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Concat(dc.FAMILY_DATA_201203s.Where(x => x.ACTION_TYPE == 100 || x.ACTION_TYPE == null).Where(x => x.FID == id).Select(f => new { Periodi = 201202, f.FID, f.PID, f.FIRST_NAME, f.LAST_NAME, f.BIRTH_DATE, f.FAMILY_SCORE, SCORE_DATE = f.VISIT_DATE, f.PIROBA }))
                  .Where(x => x.FAMILY_SCORE.HasValue && x.SCORE_DATE.HasValue)
                  .AsEnumerable()
                  .Select(x => new {Base_Type = 1, x.Periodi, x.FID, x.PID, x.FIRST_NAME, x.LAST_NAME, x.BIRTH_DATE, FAMILY_SCORE = x.FAMILY_SCORE, SCORE_DATE = x.SCORE_DATE, x.PIROBA })
                  .Concat(sds.AsEnumerable().Select(x => new {x.Base_Type, x.Periodi, x.FID, x.PID, x.FIRST_NAME, x.LAST_NAME, x.BIRTH_DATE, x.FAMILY_SCORE, x.SCORE_DATE, x.PIROBA }))


                  .GroupBy(x => new { x.FID, x.SCORE_DATE, x.FAMILY_SCORE, x.Periodi })
                  .Select(g => new FamiliData
                                   {
                                       FID = g.Key.FID,
                                       SCORE_DATE = g.Key.SCORE_DATE,
                                       FAMILY_SCORE = g.Key.FAMILY_SCORE,
                                       Periodi = g.Key.Periodi,
                                       ShemadgenlobaHash = string.Join(";", g.Select(x => string.Format("{0}{1}{2}{3}", x.PID, x.FIRST_NAME, x.LAST_NAME, x.BIRTH_DATE)).OrderBy(x => x)).GetHashCode(),
                                       Cevrebi = g.Select(x => new FamiliDataCevri { PID = x.PID, FIRST_NAME = x.FIRST_NAME, LAST_NAME = x.LAST_NAME, BIRTH_DATE = x.BIRTH_DATE, PIROBA = x.PIROBA }).OrderBy(x => x.FIRST_NAME).ToList()
                                   })
                  .OrderBy(x => x.Periodi)
                  .ToList();

                Func<int, int, bool> arisMomdevnoPeriodi = (p1, p2) =>
                                                               {
                                                                   var p1y = (p1 / 100);
                                                                   var p1m = p1 - p1y * 100;

                                                                   var p2y = (p2 / 100);
                                                                   var p2m = p2 - p2y * 100;

                                                                   var monthPlus1 = p1m + 1;
                                                                   var ny = monthPlus1 == 13 ? p1y + 1 : p1y;
                                                                   var nm = monthPlus1 == 13 ? 1 : monthPlus1;
                                                                   return ny == p2y && nm == p2m;
                                                               };

                Func<FamiliData, FamiliData, bool> arisErtiDaIgiveShemadgenloba = (p1, p2) => p1.FID == p2.FID &&
                                                                                              p1.FAMILY_SCORE == p2.FAMILY_SCORE &&
                                                                                              p1.SCORE_DATE == p2.SCORE_DATE &&
                                                                                              p1.ShemadgenlobaHash == p2.ShemadgenlobaHash;

                var fds2 = fds.Aggregate(new List<List<FamiliData>>(), (per, fd) =>
                                                                {
                                                                    var familiDatas =
                                                                        (from x in per
                                                                         let fdl = x.Last()
                                                                         where arisMomdevnoPeriodi(fdl.Periodi, fd.Periodi)
                                                                         where arisErtiDaIgiveShemadgenloba(fdl, fd)
                                                                         select x
                                                                            ).FirstOrDefault();

                                                                    if (familiDatas != null)
                                                                        familiDatas.Add(fd);
                                                                    else
                                                                        per.Add(new List<FamiliData>() { fd });
                                                                    return per;
                                                                });

                var familiDataPeriodis = fds2
                    .Select(x => new FamiliDataPeriodi(x.Min(x_ => x_.Periodi), x.Max(x_ => x_.Periodi), x.First())).ToList();
                return PartialView(familiDataPeriodis);
            }
        }

        public PartialViewResult DeklaraciebisIstoria(string id ="")
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

        public string Reestri(string pid = "")
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            return client.DownloadString(@"http://172.17.8.125/PirovnebisZebna/Person/FragmentiPid?PiradiNomeri=" + pid);
        }

        public FileResult Amonaceri(string pid = "")
        {
            WebClient client = new WebClient();

            byte[] data = client.DownloadData(@"http://172.17.8.125/CRA_Rest/SSA/AmonaceriUmceotaBazidan?pid=" + pid);

            return File(data, "application/pdf", pid + ".pdf");
        }

        [OutputCache(Duration = 600)]
        public ActionResult PrvelckaroebisMocvdisTarigebi()
        {
            using(var con = new SqlConnection(@"Data Source=triton;Initial Catalog=Pirvelckaroebi;User ID=sa;Password=ssa$20"))
            {
                con.Open();
                var list = con.Query(@"select	p.ProgramisId, p.ProgramisDasakheleba, mapDates.MaxMapDate
from	SocialuriDazgveva.dbo.Programebi (nolock) p
join	(select Base_Type, MAX(MapDate) MaxMapDate
		from Pirvelckaroebi.dbo.Source_Data (nolock)
		group by Base_Type) mapDates on mapDates.Base_Type = p.ProgramisId
ORDER BY p.ProgramisId",commandTimeout:120).ToList();
                return PartialView(list);
            }
        }
    }
}
