using Microsoft.AspNetCore.Http;

namespace Persento.ViewModel.People
{
    public class CreateVM
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Position { get; set; }
        public string Content { get; set; }
        public IFormFile Photo { get; set; }
    }
}
