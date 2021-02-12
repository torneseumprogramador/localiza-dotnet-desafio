using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    [Table("address")]
    public class Address
    {
        [Key]
        [Column]
		[JsonIgnore]
        public virtual int Id { get; set; }

        [Column]
        [Required]
        public string ZipCode { get; set; }

        [Column]
        [Required]
        public string Street { get; set; }

        [Column]
        [Required]
        public int Number { get; set; }

        [Column]
        public string Complement { get; set; }

        [Column]
        [Required]
        public string City { get; set; }

        [Column]
        [Required]
        public string State { get; set; }
    }
}
