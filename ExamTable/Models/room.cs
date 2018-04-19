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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public room()
        {
            this.sections = new HashSet<section>();
        }
        [Display(Name = "ID")]
        public int id { get; set; }

        [Display(Name = "Room Name")]
        [Required]
        public string name { get; set; }

        [Display(Name = "Room Capacity")]
        [Required]
        public Nullable<int> capacity { get; set; }

        [Display(Name = "Room Type")]
        [Required]
        public Nullable<int> room_type_id { get; set; }

        [Display(Name = "Room Available?")]
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

        public virtual room_type room_type { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<section> sections { get; set; }
    }
}
