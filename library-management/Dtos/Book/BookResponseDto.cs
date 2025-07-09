namespace LibraryManagement.Dto
{
    public class BookResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int PublishYear { get; set; }

        public string Description { get; set; }

        public List<GenreResponseDto> Genres { get; set; }

        public int Quantity { get; set; }

        public bool Available { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<LoanResponseDto> Loans { get; set; }
    }
}
