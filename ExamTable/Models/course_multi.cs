namespace ExamTable.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public partial class course_multi
    {
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
        [Required]
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

        [Display(Name = "Class Length")]
        [Required]
        public Nullable<double> class_length { get; set; }




        [Display(Name = "Has Final Exam?")]
        [Required]
        public string have_final_exam { get; set; }

        [Display(Name = "Exam Length")]
        [Required]
        public Nullable<double> exam_length { get; set; }

        [Display(Name = "Required Room Type")]
        [Required]
        public Nullable<int> required_room_type_id { get; set; }

        [Display(Name = "Note")]
        public string final_exam_note { get; set; }




        [Display(Name = "Section")]
        [Required]
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