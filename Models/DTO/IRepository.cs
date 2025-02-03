using Revisao_ASP.NET_Web_API.Models.Entities;

namespace Revisao_ASP.NET_Web_API.Models.DTO
{
    public interface IRepository
    {
        // sets the models for implements the endpoints
        // Clients
        IEnumerable<Clients> GetClients { get; }
        Clients GetClientById(int clientId);
        Clients AddClient(Clients registerClient);
        Clients UpdateClient(Clients registerClient);
        void DeleteClient(int clientId);

        // Reservations
        IEnumerable<Reservations> GetReservations { get; }
        //Reservations this[int reservationId] { get; } // mesmo que GetReservationsById
        Reservations GetReservationById(int reservationId);
        Reservations AddReservation(Reservations registerReservation);
        Reservations UpdateReservation(Reservations registerReservation);
        void DeleteReservation(int reservationId);
    }
}
