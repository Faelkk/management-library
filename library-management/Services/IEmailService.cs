using LibraryManagement.Dto;

namespace LibraryManagement.Services;

public interface IEmailService
{
    void Send(Message message);
}