using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoreApiTest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = string.Empty;
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Order>? Orders { get; set; }
    }
}
