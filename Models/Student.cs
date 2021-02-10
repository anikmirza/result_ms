using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace result_ms.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public string Name { get; set; }
        public int Roll { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime DateOfEntry { get; set; }
    }
}
