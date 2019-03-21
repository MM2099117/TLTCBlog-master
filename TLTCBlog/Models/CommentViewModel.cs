using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace TLTCBlog.Models
{
    public class CommentViewModel
    {
        /// <summary>
        /// property for the comment text
        /// </summary>
        public string Text { get; set; }

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
        /// <summary>
        /// navigational properties for Article
        /// </summary>
        public int articleID;

        public virtual BlogArticle Article { get; set; }

        }
    }