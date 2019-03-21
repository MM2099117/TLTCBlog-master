using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TLTCBlog.Models;

namespace TLTCBlog.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        /// <summary>
        /// GET: the list of blog articles
        /// </summary>
        /// <returns>List<BlogArticle></returns>
        public ActionResult List()
        {
            using (var DB = new TLTCBlogDbContext())
            {
                /// GET: the articles from the database
                var BlogArticles = DB.BlogArticles.Include(a => a.Creator).ToList();
                return View(BlogArticles);
            }
        }

        /// <summary>
        /// GET: Article Details
        /// </summary>
        /// <param name="ArticleID"></param>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var DB = new TLTCBlogDbContext())
            {
                var article = DB.BlogArticles.Where(a=> a.ArticleID == id).Include(a => a.Creator).Include(a=> a.Comments).First();
                
                
                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }

         
        }

        /// <summary>
        /// GET: Article Create View
        /// </summary>
        /// <returns>Article create View</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            using (var DB = new TLTCBlogDbContext())
            {
                var model = new ArticleViewModel();
                model.Categories = DB.Categories.OrderBy(c => c.Name).ToList();

                return View(model);
            }
        }

        /// <summary>
        /// POST: Create New Article
        /// </summary>
        /// <param name="article"></param>
        /// <returns>new BlogArticle instance</returns>
        [HttpPost]
        [ActionName("Create")]
        [Authorize]
        public ActionResult Create(ArticleViewModel model)
        {
            if(ModelState.IsValid)
            {
                using (var DB = new TLTCBlogDbContext())
                {
                    ///GET: Article Creator ID
                    var CreatorID = DB.Users.Where(u => u.UserName == this.User.Identity.Name).First().Id;

                    ///GET: Article
                    var article = new BlogArticle()
                    {
                        CreatorID = this.User.Identity.GetUserId(),
                        Title = model.Title,
                        Content = model.Content,
                        CategoryID = model.CategoryID,
                        
                    };

                    ///SET: Article Creator details
                    article.CreatorID = CreatorID;

                    ///SET: Article Creator user
                    article.Creator = DB.Users.Where(u => u.UserName == User.Identity.Name).First();

                    ///SET: Article ID
                    article.ArticleID++;

                    ///ADD: Article to Database
                    DB.BlogArticles.Add(article);
                    DB.SaveChanges();

                    ///Redirects to the Index Page and subsequently the List of Articles
                    return RedirectToAction("Index");
                }
            }
            return this.View(model);
        }

        /// <summary>
        /// GET: Article Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Blog Article Delete View</returns>
        [HttpGet]
        public ActionResult Delete(int? id)
        {
             if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
       
            using (var DB = new TLTCBlogDbContext())
            {
                ///Get Article from database
                var article = DB.BlogArticles.Where(a => a.ArticleID == id).Include(a => a.Creator)
                    .Include(a=> a.Category)
                    .First();

                ///if the user is not authorized to edit the article
                if(!IsUserAuthorizedToEdit(article))
                {
                    ///does not allow delete access
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                ///check if article exists
                if (article == null)
                {
                    return HttpNotFound();
                }
                ///Pass article to view
                return View(article);
            }

        }


        /// <summary>
        /// POST: Remove Article from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Updates database, removes article row</returns>
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
              return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            ///Get Article from db
            using (var DB = new TLTCBlogDbContext())
            {
                var article = DB.BlogArticles.Where(a => a.ArticleID == id).Include(a => a.Creator).First();

                ///check if Article exists
                if(article == null)
                {
                    return HttpNotFound();
                }

                DB.BlogArticles.Remove(article);
                DB.SaveChanges();
                return RedirectToAction("Index");

            }


        }

        /// <summary>
        /// GET: Article from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View with Article title, content loaded</returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
           /// checks if there is a valid article request
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            /// loads the DB
            using (var DB = new TLTCBlogDbContext())
            {
                /// Gets the article from the DB
                var article = DB.BlogArticles.Where(a => a.ArticleID == id).First();

                ///if the user is not authorized to edit the article
                if (!IsUserAuthorizedToEdit(article))
                {
                    ///does not allow delete access
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                /// checks if the article exists
                if (article == null)
                {
                    return HttpNotFound();
                }

                /// creates the new edit view model
                var model = new ArticleViewModel();
                model.Id = article.ArticleID;
                model.Title = article.Title;
                model.Content = article.Content;
                model.CategoryID = article.CategoryID;
                model.Categories = DB.Categories.OrderBy(c => c.Name).ToList();
                /// passes the model to the view
                return View(model);
            }
     
        }

        /// <summary>
        /// POST: Edits the details in the article model
        /// passes the new model to the DB and updates article properties
        /// </summary>
        /// <param name="model"></param>
        /// <returns>updated article in List</returns>
        [HttpPost]
        public ActionResult Edit(ArticleViewModel model)
        {
            /// checks if the model state is valid
            if(ModelState.IsValid)
            {
                /// loads the DB
                using (var DB = new TLTCBlogDbContext())
                {
                    /// gets the article from the DB
                    var article = DB.BlogArticles.FirstOrDefault(a => a.ArticleID == model.Id);

                    /// set article properties
                    article.Title = model.Title;
                    article.Content = model.Content;
                    article.CategoryID = model.CategoryID;

                    /// save new article state to DB 
                    DB.Entry(article).State = EntityState.Modified;
                    DB.SaveChanges();
                    /// redirect to the Index page
                    return RedirectToAction("Index");

                }
            }

            /// if the model state is invalid, returns the same view
            return View(model);
        }

        /// <summary>
        /// Checks if the current user is authorized to edit the article
        /// I.e is user the author of the article or an admin
        /// </summary>
        /// <param name="article"></param>
        /// <returns>true/false</returns>
        private bool IsUserAuthorizedToEdit(BlogArticle article)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isCreator = article.IsCreator(this.User.Identity.Name);

            return isAdmin || isCreator;
        }

        


    }
}