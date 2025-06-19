using LibraryManagement.Dto;

public interface ITokenGenerator
{
    string Generate(UserResponseDto user);
}