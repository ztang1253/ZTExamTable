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
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public course()
        {
            this.course_exam = new HashSet<course_exam>();
            this.sections = new HashSet<section>();
        }

        [Display(Name = "Course ID")]
        public int id { get; set; }

        [Display(Name = "Course Code")]
        [Required]
        public string code { get; set; }

        [Display(Name = "Course Title")]
        [Required]
        public string title { get; set; }

        [Display(Name = "Program Level")]
        [Required]
        public Nullable<int> hours { get; set; }

        [Display(Name = "Required Room Type 1")]
        public Nullable<int> required_room1_type_id { get; set; }

        [Display(Name = "Required Room Type 2")]
        public Nullable<int> required_room2_type_id { get; set; }

        [Display(Name = "Current Semester?")]
        public Nullable<bool> is_deleted { get; set; }

        [Display(Name = "Created By")]
        public string created_by { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Created On")]
        public Nullable<System.DateTime> created_on { get; set; }

        [Display(Name = "Modified By")]
        public string modified_by { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Modified On")]
        public Nullable<System.DateTime> modified_on { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<course_exam> course_exam { get; set; }

        public virtual room_type room_type { get; set; }

        public virtual room_type room_type1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<section> sections { get; set; }

        public string courseDropdown
        {
            get
            {
                return code + " - " + title;
            }
        }

        [Display(Name = "Class Length")]
        [Required]
        public Nullable<double> class_length { get; set; }














        [Display(Name = "Has Final Exam?")]
        [Required]
        public string have_final_exam { get; set; }

        [Display(Name = "Exam Length")]
        public Nullable<double> exam_length { get; set; }

        [Display(Name = "Required Room Type")]
        public Nullable<int> required_room_type_id { get; set; }

        [Display(Name = "Note")]
        public string final_exam_note { get; set; }




        [Display(Name = "Section")]
        public Nullable<int> section_number { get; set; }

        [Display(Name = "Program")]
        public Nullable<int> program_id { get; set; }

        [Display(Name = "Faculty")]
        [Required]
        public Nullable<int> faculty_id { get; set; }

        [Display(Name = "Weekday")]
        public Nullable<int> class_weekday { get; set; }

        [Display(Name = "Start Time")]
        public Nullable<int> class_start_time { get; set; }

        [Display(Name = "Room")]
        public Nullable<int> room_id { get; set; }














    }
}
