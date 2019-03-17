using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TLTCBlog.Models
{
    public class Category
    {
        /// <summary>
        /// Constructor to set a new hashSet of the blog articles
        /// </summary>
        public Category()
        {
            this.Articles = new HashSet<BlogArticle>();
        }

        /// <summary>
        /// Primary Key
        /// Property for holding category ID
        /// </summary>
        [Key]
        public int? ID { get; set; }

        /// <summary>
        /// Property for category name
        /// </summary>
        [Required]
        [Index(IsUnique = true)]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// navigational property 
        /// </summary>
        public virtual ICollection<BlogArticle> Articles { get; set; }
        

    }
}