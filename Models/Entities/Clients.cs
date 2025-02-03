using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Revisao_ASP.NET_Web_API.Models.Entities
{
    public class Clients
    {
        [Key] // Primary Key of the Clientes table
        public int ClientId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        //[JsonIgnore]
        // navigation property
        public ICollection<Reservations>? Reservations { get; set; } // 1 (Cliente) to many (Reservations) relationship
    }
}
