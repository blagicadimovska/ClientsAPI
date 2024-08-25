using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientsAPI.Data;
using ClientsAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientsAPI.Services;

namespace ClientsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientsController(ClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients([FromQuery] string name)
        {
            var clients = await _clientService.GetClientsAsync(name);

            if (clients == null || clients.Count == 0)
            {
                return NotFound("No clients found.");
            }

            return Ok(clients);
        }

        // GET: api/Clients/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);

            if (client == null)
            {
                return NotFound("Client not found.");
            }

            return client;
        }

        // PUT: api/Clients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> EditClient(int id, Client client)
        {
            if (id != client.ClientID)
            {
                return BadRequest("Client ID mismatch.");
            }

            var result = await _clientService.UpdateClientAsync(id, client);

            if (!result)
            {
                return NotFound("Client not found.");
            }

            return NoContent();
        }
    }
}