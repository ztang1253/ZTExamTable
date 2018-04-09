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

    public partial class room
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public Nullable<int> capacity { get; set; }
        [Required]
        public Nullable<int> room_type_id { get; set; }

        [Display(Name = "Not Available?")]
        public Nullable<bool> is_deleted { get; set; }
        public string created_by { get; set; }
        public Nullable<System.DateTime> created_on { get; set; }
        public string modified_by { get; set; }
        public Nullable<System.DateTime> modified_on { get; set; }
    
        public virtual room_type room_type { get; set; }
    }
}
