using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persento.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Position { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
