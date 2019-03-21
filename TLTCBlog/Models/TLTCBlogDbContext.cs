using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TLTCBlog.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.


    public class TLTCBlogDbContext : IdentityDbContext<ApplicationUser>
    {
        public TLTCBlogDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;

        }

        /// <summary>
        /// DBSet collections
        /// </summary>
        public virtual IDbSet<BlogArticle> BlogArticles { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<Comment> Comments { get; set; }

        public static TLTCBlogDbContext Create()
        {
            return new TLTCBlogDbContext();
        }

    }
}