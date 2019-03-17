namespace TLTCBlog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TLTCBlog.Models;

    public sealed class Configuration : DbMigrationsConfiguration<TLTCBlogDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /// <summary>
        /// Seeds the database with users, categories and content
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(TLTCBlogDbContext context)
        {
  

            ///if no categories exist, call the create category method
            if (!context.Roles.Any())
            {
                this.CreateRole(context, "Admin");
                this.CreateRole(context, "User");
                this.CreateRole(context, "Moderator");
                this.CreateRole(context, "Suspended");

            }

            ///if no categories exist, call the create category method
            if (!context.Users.Any())
            {
                this.CreateUser(context, "Admin@admin.com", "Admin", "123456");
                this.SetRoleToUser(context, "admin@admin.com", "Admin");
                this.CreateUser(context, "moderator@TLTC.com", "Mark McNulty", "123456");
                this.SetRoleToUser(context, "moderator@TLTC.com", "Moderator");
                this.CreatePosts(context);

            }

            ///if no categories exist, call the create category method
            if (!context.Categories.Any())
            {
                this.CreateCategories(context);
            }

            ///if no categories exist, call the create category method
            if (!context.BlogArticles.Any())
            {
                this.CreatePosts(context);
            }

        }

        /// <summary>
        /// Creates a new role
        /// </summary>
        /// <param name="context"></param>
        /// <param name="roleName"></param>
       private void CreateRole(TLTCBlogDbContext context, string roleName)
        {
            ///new instance of the RoleManager for the IdentityRoles in EntityFramework
            var roleMngr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            ///sets the result to the newly created role
            var result = roleMngr.Create(new IdentityRole(roleName));

            ///if the result is unsuccessful
            if (!result.Succeeded)
            {
                ///displays the errors seperated by a semicolon
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        /// <summary>
        /// Creates a new user at DB creation
        /// </summary>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="fullName"></param>
        /// <param name="password"></param>
        public void CreateUser(TLTCBlogDbContext context, string email, string fullName, string password)
        {
            ///new instance of the UserManager
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            ///sets the user manager password validator
            UserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 5,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            ///creates the new user object
            var admin = new ApplicationUser
            {
                UserName = email,
                FullName = fullName,
                Email = email,
            };

            ///creates a new user
            var result = UserManager.Create(admin, password);

            ///validates the resul
            if (!result.Succeeded)
            {
                ///displays the errors seperated by a semicolon
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        /// <summary>
        /// Applies a role to the newly created user at DB creation
        /// </summary>
        /// <param name="context"></param>
        /// <param name="email"></param>
        /// <param name="role"></param>
        private void SetRoleToUser(TLTCBlogDbContext context, string email, string role)
        {

            ///new instance of the UserManager
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var User = context.Users.Where(u => u.Email == email).First();

            var result = UserManager.AddToRole(User.Id, role);
            
            ///validates the result
            if (!result.Succeeded)
            {
                ///displays the errors seperated by a semicolon
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        /// <summary>
        /// Creates categories at DB creation for use in assigning to posts later
        /// </summary>
        /// <param name="context"></param>
        public void CreateCategories(TLTCBlogDbContext context)
        {
            context.Categories.Add(new Category()
            {
                ID = 1,
                Name = "Announcement"
            });

            context.Categories.Add(new Category()
            {
                ID = 2,
                Name = "Review"
            });
        }

        /// <summary>
        /// Creates a series of default posts in the DB and views
        /// </summary>
        /// <param name="context"></param>
        public void CreatePosts(TLTCBlogDbContext context)
        {

            context.BlogArticles.Add(new BlogArticle()
            {
                Title = "REBUS: LONG SHADOWS",
                Content = "Playwright Rona Munro has created the first Rebus play based on an original story by Rankin, and as has become her trademark, it is long on dialogue and drama. " +
                "Tightly written and atmospheric throughout, fans of the novels will be pleased that it has just as many twists and turns",
                CreatorID = context.Users.First().Id

            });

           context.BlogArticles.Add(new BlogArticle()
           {
                Title = "THE GHOSTING OF RABBIE BURNS",
                Content = "A modern rom com, The Ghosting of Rabbie Burns also manages to weave in a raft of fascinating facts about the Ploughman Poet," +
                " the narrative perfectly enhanced by the inclusion of the brightest and best songs and poems of Burns. Both Young and Mackenzie do a fine job of showcasing the bard’s work:" +
                " My Love is Like a Red Red Rose; Ae Fond Kiss; Charlie is my Darlin’ ; " +
                "John Anderson, My Jo and of course, Auld Lang Syne (with some audience participation) are just beautiful.",
               CreatorID = context.Users.First().Id
           });

            context.BlogArticles.Add(new BlogArticle()
            {
                Title = "WRITERS JOIN FORCES FOR COMEDY FESTIVAL",
                Content = "Four Glasgow playwrights have teamed up to present a quartet of plays as part of the Glasgow International Comedy Festival next month. " +
                "Short Attention Span Theatre is a method of theatre which delivers exactly what it says on the tin: short dramas for those with short attention spans as well as those who like to experience" +
                " a variety of works in one evening. ",
                CreatorID = context.Users.First().Id
            });

           

            context.BlogArticles.Add(new BlogArticle()
            {
                Title = "NEWS: SAILOR JERRY RETURNS TO SCOTLAND FOR ‘CITY TAKEOVERS’ HONOURING BOLD LOCAL TALENT",
                Content = "Throughout the month ‘flash art’ inspired by the iconic designs of Sailor Jerry’s namesake, legendary tattoo artist Norman Collins, will appear across the three cities. " +
                "Residents are encouraged to #SJFollowTheFlash by visiting participating hotspots, including bars, restaurants, tattoo shops and barbers, as well as city walls and university campuses, to be in with a chance of winning tickets to a once-in-a-lifetime experience…",
                CreatorID = context.Users.First().Id
            });

         
        }

  
    }
}
