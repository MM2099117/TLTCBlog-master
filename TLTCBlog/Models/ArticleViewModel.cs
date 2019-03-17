    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TLTCBlog.Models
{
    public class ArticleViewModel
    {
        /// <summary>
        /// Post ID property
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Post Title property
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        /// <summary>
        /// Post Content Property
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Post Creator property
        /// </summary>
        public string CreatorID { get; set; }

        /// <summary>
        /// Category ID property
        /// </summary>
        public int? CategoryID { get; set; }

        /// <summary>
        /// navigational property for Categories
        /// </summary>
        public ICollection<Category> Categories { get; set; }

    }
}