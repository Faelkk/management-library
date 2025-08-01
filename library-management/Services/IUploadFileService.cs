namespace LibraryManagement.Services;

public interface IUploadFileService
{
    Task<string> UploadFileAsync(IFormFile file);

    Task DeleteFileAsync(string fileName);
}
