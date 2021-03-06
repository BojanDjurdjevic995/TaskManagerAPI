//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskManagerAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class tasks
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        public string status { get; set; }

        [Required]
        [StringLength(255)]
        public string priority { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> start_date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> end_date { get; set; }
        public string notes { get; set; }
    }
}
