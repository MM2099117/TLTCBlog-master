using Microsoft.Owin;
using Owin;
using System.Data.Entity;
using TLTCBlog.Migrations;
using TLTCBlog.Models;

[assembly: OwinStartupAttribute(typeof(TLTCBlog.Startup))]
namespace TLTCBlog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TLTCBlogDbContext, Configuration>());
            ConfigureAuth(app);
        }
    }
}
