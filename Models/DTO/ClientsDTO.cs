namespace Revisao_ASP.NET_Web_API.Models.DTO
{
    public class ClientsDTO
    {
        public int ClientId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // relationship between DTOs to retrive data from client and reservations that is being relationated
        public List<ReservationsDTO> Reservations { get; set; } = new List<ReservationsDTO>(); // new List<ReservationsDTO>(); - avoid break because null reference
    }
}
