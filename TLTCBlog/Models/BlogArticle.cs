using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TLTCBlog.Controllers;

namespace TLTCBlog.Models
{
    public class BlogArticle
    {

        /// <summary>
        /// constructor
        /// </summary>
        public BlogArticle()
        {
            this.CreatorID = CreatorID;
            this.Title = Title;
            this.Content = Content;
            this.CategoryID = CategoryID;
            this.Comments = new HashSet<Comment>();
        }

        

        /// <summary>
        /// Primary Key
        /// Article ID code, increments with each new post
        /// </summary>
        [Key]
        public int ArticleID { get; set; }

        /// <summary>
        /// Post Title
        /// </summary>
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// Post Content text
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Foreign Key
        /// CreatorID = UserID
        /// </summary>
        [InverseProperty("Creator")]
        public string CreatorID { get; set; }

        /// <summary>
        /// Navigational Property for the user/creator of the article
        /// </summary>
        public virtual ApplicationUser Creator { get; set; }

        /// <summary>
        /// checks if the user and creator of the article are the same
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true/false</returns>
        public bool IsCreator(string name)
        {
            return Creator.UserName.Equals(name);
        }

        /// <summary>
        /// Foreign Key to Category table
        /// </summary>
        [InverseProperty("Category")]
        public int? CategoryID { get; set; }


        /// <summary>
        /// Navigational Property for the Category 
        /// </summary>
        public virtual Category Category { get; set; }



        /// <summary>
        /// Navigational Property for Comments
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        

    }
}