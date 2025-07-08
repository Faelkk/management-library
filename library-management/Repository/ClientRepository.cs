using LibraryManagement.Contexts;
using LibraryManagement.Models;

namespace LibraryManagement.Repository;

public class ClientRepository : IClientRepository
{
    private readonly IDatabaseContext databaseContext;

    public ClientRepository(IDatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    public IEnumerable<ClientResponseDto> GetAll()
    {
        var clients = databaseContext
            .clients.Select(c => new ClientResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
            })
            .ToList();

        return clients;
    }

    public ClientResponseDto GetById(int id)
    {
        var client = databaseContext
            .clients.Where(c => c.Id == id)
            .Select(c => new ClientResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
            })
            .FirstOrDefault();

        if (client == null)
        {
            throw new Exception("Client not found");
        }

        return client;
    }

    public ClientResponseDto Create(ClientInsertDto clientInsertDto)
    {
        var client = new Models.Client
        {
            Name = clientInsertDto.Name,
            Email = clientInsertDto.Email,
            PhoneNumber = clientInsertDto.PhoneNumber,
        };

        databaseContext.clients.Add(client);
        databaseContext.SaveChanges();

        return new ClientResponseDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
        };
    }

    public ClientResponseDto Update(int id, ClientUpdateDto clientUpdateDto)
    {
        var client = databaseContext.clients.FirstOrDefault(c => c.Id == id);

        if (client == null)
            throw new Exception("Client not found");

        if (clientUpdateDto.Name != null)
            client.Name = clientUpdateDto.Name;

        if (clientUpdateDto.Email != null)
            client.Email = clientUpdateDto.Email;

        if (clientUpdateDto.PhoneNumber != null)
            client.PhoneNumber = clientUpdateDto.PhoneNumber;

        databaseContext.SaveChanges();

        return new ClientResponseDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
        };
    }

    public async Task<bool> Remove(int id)
    {
        var client = databaseContext.clients.FirstOrDefault(c => c.Id == id);

        if (client == null)
        {
            throw new Exception("client not found");
        }

        databaseContext.clients.Remove(client);

        await databaseContext.SaveChangesAsync();
        return true;
    }

    public bool ExistsWithEmail(string email, int excludeClientId)
    {
        return databaseContext.clients.Any(c => c.Email == email && c.Id != excludeClientId);
    }

    public bool ExistsWithPhoneNumber(string phoneNumber, int excludeClientId)
    {
        return databaseContext.clients.Any(c =>
            c.PhoneNumber == phoneNumber && c.Id != excludeClientId
        );
    }
}
