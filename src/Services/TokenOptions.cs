namespace LibraryManagement.Services;

public class TokenOptions
{
    public string Token = "Token";

    public string Secret { get; set; }
    public int ExpiresDay { get; set; }
}