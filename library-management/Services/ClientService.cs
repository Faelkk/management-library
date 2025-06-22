
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
        var updatedClient = clientRepository.Update(id, clientUpdateDto);

        if (updatedClient == null)
            throw new Exception("Book not found for update");

        return updatedClient;
    }
    public async Task<bool> Delete(int id)
    {
        return await clientRepository.Remove(id);
    }

}