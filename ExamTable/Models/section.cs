//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExamTable.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class section
    {
        public int id { get; set; }
        [Required]
        public Nullable<int> section_number { get; set; }
        public Nullable<int> program_id { get; set; }
        [Required]
        public Nullable<int> course_id { get; set; }
        [Required]
        public Nullable<int> student_enrolled { get; set; }
        [Required]
        public Nullable<int> faculty_id { get; set; }

        [Display(Name = "Not Current Course?")]
        public Nullable<bool> is_deleted { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public string modified_by { get; set; }
        public Nullable<System.DateTime> modified_on { get; set; }
    
        public virtual course course { get; set; }
        public virtual faculty faculty { get; set; }
        public virtual program program { get; set; }
    }
}
