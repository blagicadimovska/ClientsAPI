using ClientsAPI.Data;
using ClientsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientsAPI.Services
{
    public class ClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetClientsAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return await _context.Clients.Include(c => c.Addresses).ToListAsync();
            }

            return await _context.Clients
                                 .Where(c => EF.Functions.Like(c.Name, $"%{name}%"))
                                 .Include(c => c.Addresses)
                                 .ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _context.Clients.Include(c => c.Addresses)
                                         .FirstOrDefaultAsync(c => c.ClientID == id);
        }

        public async Task<bool> UpdateClientAsync(int id, Client client)
        {
            var existingClient = await GetClientByIdAsync(id);

            if (existingClient == null)
            {
                return false;
            }

            existingClient.Name = client.Name;
            existingClient.BirthDate = client.BirthDate;

            existingClient.Addresses.Clear();
            foreach (var address in client.Addresses)
            {
                existingClient.Addresses.Add(address);
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return false;
                }
                throw;
            }
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientID == id);
        }
    }
}
