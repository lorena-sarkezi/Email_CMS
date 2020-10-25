using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CMS.Data.Database
{
    [Table("Threads")]
    public class ConvoThread
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string ThreadTitle { get; set; }
        [Required]
        public DateTime LatestMessageTimestamp { get; set; }
        [Required]
        public DateTime TimestampCreated { get; set; }
    }
}
