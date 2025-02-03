using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Revisao_ASP.NET_Web_API.Models.DTO;
using Revisao_ASP.NET_Web_API.Models.Entities;

namespace Revisao_ASP.NET_Web_API.Controllers
{
    [Authorize]
    [ApiController] // define this controller as a API RESTful
    [Route("api/[controller]/[action]")]
    public class ClientsController : ControllerBase // ControllerBase is used to create API Controller and don't suport return Views
    {
        // sets DI's
        private readonly IRepository _repository;

        public ClientsController(IRepository repository)
        {
            _repository = repository;
        }

        //***********************************************************************************************//
        //                                            Endpoints                                          //
        //***********************************************************************************************//
        //
        // Read
        [HttpGet]
        [Route("AllClients")]
        public IActionResult GetClients() => Ok(_repository.GetClients); // search all clients in the database using the IRepository model

        // Read - by id
        [HttpGet("{clientId}")]
        public IActionResult GetClientById([FromRoute] int clientId)
        {
            var searchClient = _repository.GetClientById(clientId); // search client by id in the database using the IRepository model

            if (searchClient == null)
            {
                return NotFound();
            }
            return Ok(searchClient);
        }

        // Create
        [Authorize(Policy = "RequireAdminRole")] // applies access policy defined on Program.cs
        [HttpPost]
        public IActionResult AddClient([FromBody] Clients registerClient)
        {
            var newClient = _repository.AddClient(registerClient); // add client in the database using the IRepository model

            return CreatedAtAction(nameof(GetClientById), new // call the Action GetClientById defined in this controller
            {
                clientId = newClient.ClientId // the reciver parameter must match the same name defined in the method - in this case GetClientById
            }, newClient);
        }

        // Update
        [Authorize(Policy = "RequireAdminRole")] // applies access policy defined on Program.cs
        [HttpPut("{clientId}")]
        public IActionResult UpdateClient([FromRoute] int clientId, [FromBody] Clients registerClient)
        {
            if(clientId != registerClient.ClientId)
            {
                return BadRequest();
            }

            var updateClient = _repository.UpdateClient(registerClient); // update client data using the IRepository model

            return Ok(updateClient);
        }

        // Delete
        [Authorize(Policy = "RequireAdminRole")] // applies access policy defined on Program.cs
        [HttpDelete("{clientId}")]
        public IActionResult DeleteClient([FromRoute] int clientId)
        {
            _repository.DeleteClient(clientId); // delete client from database using the IRepository model

            return NoContent();
        }
    }
}
