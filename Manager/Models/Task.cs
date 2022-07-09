//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Task
    {
        [Required(ErrorMessage =  "Required")]
        public string TicketNo { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Task1 { get; set; }

        [Required(ErrorMessage = "Required")]
        public string TaskStatus { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(1,999, ErrorMessage = "Enter Planned Effort")]
        public decimal PlanedEffort { get; set; }

        [Required(ErrorMessage = "Required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-mm-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime Date { get; set; }

        public string Comment { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
