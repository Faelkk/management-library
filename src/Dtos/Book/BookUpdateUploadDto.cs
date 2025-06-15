public class BookUpdateRequest
{
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int? PublishYear { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public IFormFile? ImageFile { get; set; }
}
