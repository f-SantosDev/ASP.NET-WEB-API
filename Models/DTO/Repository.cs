using Microsoft.EntityFrameworkCore;
using Revisao_ASP.NET_Web_API.Models.Entities;

namespace Revisao_ASP.NET_Web_API.Models.DTO
{
    public class Repository : IRepository
    {
        // referential element - DI
        private readonly AppDbContext _context;

        // configure DI
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        //*****************************************************************************************************//
        //              sets  the CRUD operations of the Clients table defined in IRepository - DTO            //
        //*****************************************************************************************************//
        //
        // Read
        public IEnumerable<Clients> GetClients => _context.Clients.Include(c => c.Reservations).ToList();

        // CRUD for relationship between DTOs to retrive data from client and reservations that is being relationated
        public IEnumerable<ClientsDTO> ClientsDTOs => _context.Clients.Select(c => new DTO.ClientsDTO
        {
            ClientId = c.ClientId,
            Name = c.Name,
            Surname = c.Surname,
            Email = c.Email,
            PhoneNumber = c.PhoneNumber
        }).ToList();

        // Read - by id
        //public Clients GetClientById(int clientId) => _context.Clients.Find(clientId);
        //
        public Clients GetClientById(int clientId)
        {
            return _context.Clients.Include(c => c.Reservations) // include reservations related to the client
                                   .FirstOrDefault(c => c.ClientId == clientId);
        }

        public ClientsDTO GetClientsDTO(int clientId)
        {
            var client = _context.Clients.FirstOrDefault(c => c.ClientId == clientId);

            return client != null ? new ClientsDTO
            {
                ClientId = clientId,
                Name = client.Name,
                Surname = client.Surname,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            } : null;
        }

        // Create
        public Clients AddClient(Clients registerClient)
        {
            _context.Clients.Add(registerClient); // add the new user in the database

            _context.SaveChanges(); // save the user added in the database

            return registerClient;
        }

        // Update
        public Clients UpdateClient(Clients registerClient)
        {
            _context.Clients.Update(registerClient); // update the user data in the database

            _context.SaveChanges(); // save the user data updated in the database

            return registerClient;
        }

        // Delete
        public void DeleteClient(int clientId)
        {
            var client = _context.Clients.Find(clientId); // search the client in the database

            if (client != null) // check if the client exist in the database
            {
                _context.Clients.Remove(client); // delete the user in the database

                _context.SaveChanges(); // save the user deleted in the database
            }
        }

        //*****************************************************************************************************//
        //              sets  the CRUD operations of the Reservations table defined in IRepository - DTO            //
        //*****************************************************************************************************//
        //
        // Read
        public IEnumerable<Reservations> GetReservations => _context.Reservations.Include(r => r.Client).ToList();

        public IEnumerable<ReservationsDTO> GetReservationsDTO => _context.Reservations.Include(r => r.Client).Select(r => new ReservationsDTO
        {
            ReservationId = r.ReservationId,
            Origin = r.Origin,
            Destination = r.Destination,
            DepartureDate = r.DepartureDate,
            ReturnDate = r.ReturnDate,
            DepartureTime = r.DepartureTime,
            ReturnTime = r.ReturnTime,
            Client = r.Client != null ? new ClientsDTO
            {
                ClientId = r.Client.ClientId,
                Name = r.Client.Name,
                Surname = r.Client.Surname,
                Email = r.Client.Email,
                PhoneNumber = r.Client.PhoneNumber,
            } : null
        }).ToList();

        // Read - by id
        //public Reservations this[int reservationId] => _context.Reservations.Find(reservationId);
        /*public Reservations GetReservationsById(int reservationId)
        {
            return _context.Reservations.Find(reservationId);
        }*/

        public Reservations GetReservationById(int reservationId)
        {
            return _context.Reservations.Include(r => r.Client) // include client related to the reservations
                                         .FirstOrDefault(r => r.ReservationId == reservationId);
        }

        public ReservationsDTO GetReservationDTO(int reservationId)
        {
            var reservation = _context.Reservations.Include(r => r.Client) // include client related to the reservations
                                                   .FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null) 
                return null;

            return new ReservationsDTO
            {
                ReservationId = reservation.ReservationId,
                Origin = reservation.Origin,
                Destination = reservation.Destination,
                DepartureDate = reservation.DepartureDate,
                ReturnDate = reservation.ReturnDate,
                DepartureTime = reservation.DepartureTime,
                ReturnTime = reservation.ReturnTime,
                Client = reservation.Client != null ? new ClientsDTO
                {
                    ClientId = reservation.Client.ClientId,
                    Name = reservation.Client.Name,
                    Surname = reservation.Client.Surname,
                    Email = reservation.Client.Email,
                    PhoneNumber = reservation.Client.PhoneNumber,
                } : null
            };
        }
        

        // Insert
        public Reservations AddReservation(Reservations registerReservation)
        {
            _context.Reservations.Add(registerReservation); // add the new reservation in the database

            _context.SaveChanges(); // save the reservation added in the database

            return registerReservation;
        }

        // Update
        public Reservations UpdateReservation(Reservations registerReservation)
        {
            _context.Reservations.Update(registerReservation); // update the reservation data in the database

            _context.SaveChanges(); // save the reservation data updated in the database

            return registerReservation;
        }

        // Delete
        public void DeleteReservation(int reservationId)
        {
            var reservation = _context.Reservations.Find(reservationId); // search the reservation in the database

            if(reservation != null) // check if the reservation exist in the database
            {
                _context.Reservations.Remove(reservation); // delete the reservation in the database

                _context.SaveChanges(); // save the reservation deleted in the database
            }
        }
    }
}
