using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TLTCBlog.Models;

namespace TLTCBlog.Controllers.Admin
{
    public class CategoryController : Controller
    {
        /// <summary>
        /// GET: Category list
        /// </summary>
        /// <returns>list of categories</returns>
        [HttpGet]
        public ActionResult List()
        {
            using (var DB = new TLTCBlogDbContext())
            {
                var categories = DB.Categories.ToList();

                return View(categories);
            }
        }

        /// Redirect to Category List
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        /// <summary>
        /// GET: Categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// POST: Create new Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>view with list of categories</returns>
        [HttpPost]
        public ActionResult Create(Category category)
        {
            ///checks if model state is valid
            if (ModelState.IsValid)
            {
                using (var DB = new TLTCBlogDbContext())
                {
                   
                    ///ADD: Article to Database
                    DB.Categories.Add(category);
                    DB.SaveChanges();

                    ///Redirects to the Index Page and subsequently the List of Articles
                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }

        /// <summary>
        /// GET: Category/Edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns>view with list of categories</returns>
        public ActionResult Edit(int? id)
        {
           if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           using(var DB = new TLTCBlogDbContext())
            {
                var category = DB.Categories.FirstOrDefault(c => c.ID == id);

                if(category == null)
                {
                    return HttpNotFound();
                }

                return View(category);
            }
        }

        /// <summary>
        /// POST: Edit/Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>View with edited categories</returns>
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if(ModelState.IsValid)
            {
                using (var DB = new TLTCBlogDbContext())
                {
                    DB.Entry(category).State = EntityState.Modified;
                    DB.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            return View(category);
        }


        /// <summary>
        /// GET: Category/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View with categories and delete options</returns>
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var DB = new TLTCBlogDbContext())
            {
                var category = DB.Categories.FirstOrDefault(c => c.ID == id);
                
                if(category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }

        }
        
        /// <summary>
        /// POST: Category/Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Updated category list</returns>
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            using (var DB = new TLTCBlogDbContext())
            {
                var category = DB.Categories.FirstOrDefault(C => C.ID == id);

                var categoryArticles = category.Articles.ToList();

                foreach(var article in categoryArticles)
                {
                    DB.BlogArticles.Remove(article);
                }

                DB.Categories.Remove(category);
                DB.SaveChanges();

                return RedirectToAction("Index");
            }
        }


    }

 }
