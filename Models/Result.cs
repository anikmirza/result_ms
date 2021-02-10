using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace result_ms.Models
{
    [Table("Result")]
    public class Result
    {
        [Key]
        public int ResultId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public double Mark { get; set; }
        public DateTime DateOfEntry { get; set; }
    }
}
