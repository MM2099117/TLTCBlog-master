using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLTCBlog.Models;

namespace TLTCBlog.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Redirects to the article list as the index page is obsolete
        /// </summary>
        /// <returns>Article/List</returns>
        public ActionResult Index()
        {
            return RedirectToAction("List", "Article");
        }

        /// <summary>
        /// Displays a list of categories
        /// </summary>
        /// <returns></returns>
        public ActionResult ListCategories()
        {
            using (var DB = new TLTCBlogDbContext())
            {
                var categories = DB.Categories.Include(c => c.Articles).OrderBy(c => c.Name).ToList();

                return View(categories);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "The Local Theatre Company";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Lists articles via selected category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns> list of articles by category </returns>
        public ActionResult ListArticles(int? categoryID)
        {
            using(var DB = new TLTCBlogDbContext())
            {
                var articles = DB.BlogArticles
                    .Where(a => a.CategoryID == categoryID)
                    .Include(a => a.Creator)
                    .ToList();

                return View(articles);
            }
        }
    }
}