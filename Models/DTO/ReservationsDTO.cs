namespace Revisao_ASP.NET_Web_API.Models.DTO
{
    public class ReservationsDTO
    {
        public int ReservationId { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan? ReturnTime { get; set; }

        // relationship between DTOs to retrive data from client and reservations that is being relationated
        public ClientsDTO? Client { get; set; }
    }
}
