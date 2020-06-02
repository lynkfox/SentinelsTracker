using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    [Table("AccessChanges", Schema = "logging")]
    public class AccessChange
    {
        public int ID { get; set; }

        //Fkey - one to many (this is the many side)
        public int UserId { get; set; }
        public User User { get; set; }

        //Fkey one to many (this is the many side)
        [Display(Name="Access Level Changed To: ")]
        public int AccessLevelId { get; set; }
        public AccessLevel AccessLevel { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ChangedOn { get; set; }
        public string Reason { get; set; }
    }
}
