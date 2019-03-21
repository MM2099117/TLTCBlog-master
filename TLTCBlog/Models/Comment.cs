using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TLTCBlog.Models
{
    public class Comment
    {
        /// <summary>
        /// property for comment's ID
        /// </summary>
        [Key]
        public int CommentID { get; set; }

        /// <summary>
        /// property for the comment text
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// constructors
        /// </summary>
        public Comment()
        {

        }

        public Comment(string text, string creatorID, int? articleID)
        {
            Text = text;
            CreatorID = creatorID;
            this.articleID = (int)articleID;
        }

        public Comment(Comment comment)
        {

        }

        /// <summary>
        /// Foreign Key
        /// CreatorID = UserID
        /// </summary>
        [InverseProperty("User")]
        public string CreatorID { get; set; }

        /// <summary>
        /// Navigational Property for the user/creator of the article
        /// </summary>
        public virtual ApplicationUser Creator { get; set; }

        /// <summary>
        /// navigational properties for Article
        /// </summary>
        [InverseProperty("Article")]
        public int articleID { get; set; }
        public virtual BlogArticle Article { get; set; }
  
    }
}