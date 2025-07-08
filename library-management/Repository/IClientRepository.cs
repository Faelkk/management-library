using LibraryManagement.Models;

namespace LibraryManagement.Repository;

public interface IClientRepository
{
    ClientResponseDto GetById(int id);
    IEnumerable<ClientResponseDto> GetAll();
    ClientResponseDto Create(ClientInsertDto clientInsertDto);
    ClientResponseDto Update(int id, ClientUpdateDto clientUpdateDto);
    Task<bool> Remove(int id);

    bool ExistsWithEmail(string email, int excludeClientId);
    bool ExistsWithPhoneNumber(string phoneNumber, int excludeClientId);
}
