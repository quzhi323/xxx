using OrthoEvidence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace OrthoEvidence.Controllers
{
    public class AceReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var viewModel = new AceReportViewModel
            {
                AceReports = db.AceReports
            };
            return View(viewModel);
        }

        public ActionResult Show(string id, String sharestamp)
        {

            var aceReport = db.AceReports.FirstOrDefault(x => x.Name.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            if (aceReport == null) return RedirectToAction("Index");
            bool? userAuthenticated = User?.Identity?.IsAuthenticated;
            if (userAuthenticated == null || userAuthenticated == false)
            {
                if (sharestamp != null) {
                    Guid sharestampId = new Guid(sharestamp);
                    var shareReport = from r in db.ShareReports
                                      where r.Id == sharestampId && r.Email == id
                                      select r;

                    if (shareReport == null)
                    {
                        return RedirectToAction("Index");
                    }
                    else return View(aceReport);
                }
                return RedirectToAction("Login", "Account");

            }
            
            return View(aceReport);



        }




        [HttpPost]
        public ActionResult AjaxTest(string email,string report)

        {
            string test = "test";


            //string alreadyUrl = CheckSharedReport(email, report);

            //if (alreadyUrl != null) {
              //  return PartialView((object)alreadyUrl);
            //}

            if (CheckShareTime(email) is true)
            {
                test = "generating";
                Guid stamp = GenerateShareStamp();
                ShareReport share = NewShare(stamp,email,report);
                WriteShareRecord(share);
                string url = "http://localhost:52013/AceReport/Show/"+report+"?sharestamp="+stamp.ToString();
                return PartialView((object)url);

            }
            else {
                test = "limited";
            }

            return PartialView((object)test);
        }

        public bool CheckShareTime(string email)
        {
            var time = (from r in db.ShareReports
                      where r.Email == email
                      select r
                    ).Count();
            if (time < 3) return true;
            else return false;
        }

        public string CheckSharedReport(string email, string reportname) {
            string url = null;

            var shareReportId = from r in db.ShareReports
                              where r.ReportName == reportname && r.Email == email
                              select r;

            if (shareReportId == null)
            {
                return url;
            }
            else {
                url = "http://localhost:52013/AceReport/Show/"+reportname+"?sharestamp=shabi";
            }
            return url;

        }

        public Guid GenerateShareStamp() {
            Guid id = System.Guid.NewGuid();
            return id;
        }

        public ShareReport NewShare(Guid stamp, string email, string reportname) {
            ShareReport share = new ShareReport();
            share.Id = stamp;
            share.Email = email;
            share.ReportName = reportname;
            return share;
        }

        public void WriteShareRecord(ShareReport share) {
            db.ShareReports.Add(share); 
            db.SaveChanges();
        }

     


    }
}