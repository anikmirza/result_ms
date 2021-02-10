using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace result_ms.Models
{
    [Table("Class")]
    public class Class
    {
        [Key]
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
        public DateTime DateOfEntry { get; set; }
    }
}
