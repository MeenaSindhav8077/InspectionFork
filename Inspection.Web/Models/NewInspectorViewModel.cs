using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inspection.Web.Models
{
    public class NewInspectorViewModel
    {
        public NewInspectorModel NewInspector { get; set; }
        public List<NewInspectorModel> Inspectors { get; set; }
        public string Status { get; set; }

    }
    public class NewInspectorModel
    {
        // public NewInspectorModel NewInspector { get; set; }
        // public List<NewInspectorModel> Inspectors { get; set; }



        [Required(ErrorMessage = " ID is required")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "ID must be exactly 4 digits")]
        public int UserID { get; set; }

        [Required(ErrorMessage = " First Name is required")]
        [StringLength(100, ErrorMessage = " First Name cannot exceed 100 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = " Last Name is required")]
        [StringLength(100, ErrorMessage = "Last Name cannot exceed 100 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = " Password is required")]
        [StringLength(100, ErrorMessage = "Password cannot exceed 100 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters")]
        public string Department { get; set; }


    }
}