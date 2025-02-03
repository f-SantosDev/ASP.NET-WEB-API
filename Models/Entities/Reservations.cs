using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Revisao_ASP.NET_Web_API.Models.Entities
{
    public class Reservations
    {
        [Key] // Primary Key of the Reservas table
        public int ReservationId { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan? ReturnTime { get; set; }

        // Primary Key of the Clientes table
        public int ClientId { get; set; }

        [JsonIgnore]
        // navigation property
        public Clients? Client { get; set; } // Many (reservations) to 1 (Client) relationship
    }
}
