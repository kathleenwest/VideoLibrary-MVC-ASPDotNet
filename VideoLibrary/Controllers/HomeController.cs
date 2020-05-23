using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VideoLibraryBusinessLayer;
using System.Diagnostics;

namespace VideoLibrary.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            // Load the Video Collection from Session
            Collection<Video> myCollection = Session["VideoCollection"] as Collection<Video>;

            // Check to See if the Video Collection is New or Empty
            if (myCollection == null)
            {
                myCollection = VideoFactory.GetCollection();
                //Add the Factory to the Session
                Session["VideoCollection"] = myCollection;
            }

            ViewData["Directors"] = GetVideoDirectors();

            return View(myCollection);
        }

        private SelectListItem[] GetVideoDirectors()
        {

            List<SelectListItem> list = new List<SelectListItem>();

            // Load the Video Collection from Session
            Collection<Video> myCollection = Session["VideoCollection"] as Collection<Video>;

            // Check to See if the Video Collection is New or Empty
            if (myCollection == null)
            {
                myCollection = VideoFactory.GetCollection();
                //Add the Factory to the Session
                Session["VideoCollection"] = myCollection;
            }

            foreach (Video video in myCollection)
            {
                list.Add(new SelectListItem()
                {
                    Text = video.Director,
                    Value = video.Director,
                    Selected = false
                });
            }

            return list.ToArray();
        }

        [HttpPost]
        public ActionResult Results(FormCollection frm)
        {
            // Take Results from the Form
            string title = frm["TitleSearch"];
            string year = frm["YearSearch"];
            string director = frm["DirectorSearch"];

            //TODO
            // Validate User Input Entry - Year

            // Load the Video Collection from Session
            Collection<Video> myCollection = Session["VideoCollection"] as Collection<Video>;

            // Check to See if the Video Collection is New or Empty
            if (myCollection == null)
            {
                myCollection = VideoFactory.GetCollection();
                //Add the Factory to the Session
                Session["VideoCollection"] = myCollection;
            }

            // Create Ienumerable Collection for LINQ Query
            IEnumerable<Video> mySearchResults = myCollection;

            // Filter Results on Title (If Exists)
            if (!String.IsNullOrWhiteSpace(title))
            {
                mySearchResults = mySearchResults.Where(v => v.Title.ToLower().Contains(title.ToLower()));
            }

            // Filter Results on Year (If Exists)
            if (!String.IsNullOrWhiteSpace(year))
            {
                int yearValid;
                if(int.TryParse(year, out yearValid))
                {
                    mySearchResults = mySearchResults.Where(v => v.Year == yearValid);
                }               
            }

            // Filter Results on Director (If Exists)
            if (!String.IsNullOrWhiteSpace(director))
            {
                mySearchResults = mySearchResults.Where(v => v.Director == director);
            }

            // Convert the Search Results to a Collection
            Collection<Video> mySearchCollection = new Collection<Video>(mySearchResults.ToList<Video>());

            // Return the View
            return View(mySearchCollection);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            // Load the Video Collection from Session
            Collection<Video> myCollection = Session["VideoCollection"] as Collection<Video>;

            // Check to See if the Video Collection is New or Empty
            if (myCollection == null)
            {
                myCollection = VideoFactory.GetCollection();
                //Add the Factory to the Session
                Session["VideoCollection"] = myCollection;
            }

            // Create Ienumerable Collection for LINQ Query
            IEnumerable<Video> mySearchResults = myCollection;

            // Find the Video Model with the Matching ID
            mySearchResults = mySearchResults.Where(v => v.VideoId == id);

            // Convert the Search Results to a Collection
            Collection<Video> mySearchCollection = new Collection<Video>(mySearchResults.ToList<Video>());

            // Return the View
            return View(mySearchCollection);
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}