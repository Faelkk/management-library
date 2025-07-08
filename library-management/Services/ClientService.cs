using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        this.clientRepository = clientRepository;
    }

    public IEnumerable<ClientResponseDto> GetAll()
    {
        var clients = clientRepository.GetAll();

        return clients;
    }

    public ClientResponseDto GetById(int id)
    {
        var client = clientRepository.GetById(id);

        if (client == null)
            throw new Exception("client not found");

        return client;
    }

    public ClientResponseDto Create(ClientInsertDto clientInsertDto)
    {
        var newClient = clientRepository.Create(clientInsertDto);

        return newClient;
    }

    public ClientResponseDto Update(int id, ClientUpdateDto clientUpdateDto)
    {
        if (clientRepository.ExistsWithEmail(clientUpdateDto.Email, id))
            throw new Exception("Email already in use by another client");

        if (clientRepository.ExistsWithPhoneNumber(clientUpdateDto.PhoneNumber, id))
            throw new Exception("Phone number already in use by another client");

        return clientRepository.Update(id, clientUpdateDto);
    }

    public async Task<bool> Delete(int id)
    {
        return await clientRepository.Remove(id);
    }
}
