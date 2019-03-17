using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TLTCBlog.Controllers;

namespace TLTCBlog.Models
{
    public class EditUserViewModel
    {
        /// <summary>
        /// new instance of ApplicationUser
        /// </summary>
        public ApplicationUser User { get; set; }

        /// <summary>
        /// string property for password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Creates a password confirmation
        /// </summary>
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password does not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// navigational property for Roles class
        /// </summary>
        public IList<Role> Roles { get; set; }


    }
}