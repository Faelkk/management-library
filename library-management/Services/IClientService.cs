namespace LibraryManagement.Services;

public interface IClientService
{
    ClientResponseDto GetById(int id);
    IEnumerable<ClientResponseDto> GetAll();
    ClientResponseDto Create(ClientInsertDto clientInsertDto);
    ClientResponseDto Update(int id, ClientUpdateDto clientUpdateDto);
    Task<bool> Delete(int id);
}
