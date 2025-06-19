using LibraryManagement.Dto;
using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.Services;
using Moq;
using Moq.Language.Flow;

public class GenreServiceTest
{
    private readonly Mock<IGenreRepository> genreRepositoryMock;
    private readonly GenreService genreService;
    public GenreServiceTest()
    {
        genreRepositoryMock = new Mock<IGenreRepository>();
        genreService = new GenreService(genreRepositoryMock.Object);
    }


    [Fact]
    public void GetAll_ReturnsGenreResponseDtos()
    {
        var genres = new List<GenreResponseDto>
        {
            new GenreResponseDto{ Id = 1, Name = "Fiction" },
            new GenreResponseDto{ Id = 2, Name = "Non-Fiction" }
        };

        genreRepositoryMock.Setup(r => r.GetAll()).Returns(genres);


        var result = genreService.GetAll();


        Assert.Collection(result, g => { Assert.Equal(1, g.Id); Assert.Equal("Fiction", g.Name); },
                          g => { Assert.Equal(2, g.Id); Assert.Equal("Non-Fiction", g.Name); });

    }


    [Fact]
    public void GetById_ExistingGenre_ReturnGenreResponseDto()
    {
        var genreResponseDto = new GenreResponseDto { Id = 1, Name = "Fiction" };
        genreRepositoryMock.Setup(r => r.GetById(1)).Returns(genreResponseDto);

        var result = genreService.GetById(1);

        Assert.Equal(1, result.Id);
        Assert.Equal("Fiction", result.Name);
    }

    [Fact]
    public void Create_ValidGenreInsertDto_ReturnGenreResponseDto()
    {
        var genreResponseDto = new GenreResponseDto { Id = 1, Name = "Fiction" };

        genreRepositoryMock.Setup(r => r.Create(It.IsAny<GenreInsertDto>())).Returns(genreResponseDto);

        var result = genreService.Create(new GenreInsertDto { Name = "Fiction" });

        Assert.Equal(1, result.Id);
        Assert.Equal("Fiction", result.Name);
    }

    [Fact]
    public void Update_ExistingGenre_ReturnGenreResponseDto()
    {
        var genreUpdateDto = new GenreUpdateDto { Name = "Updated Fiction" };

        genreRepositoryMock.Setup(r => r.Update(1, genreUpdateDto))
            .Returns(new GenreResponseDto { Id = 1, Name = "Updated Fiction" });

        var result = genreService.Update(1, genreUpdateDto);

        Assert.Equal(1, result.Id);
        Assert.Equal("Updated Fiction", result.Name);
    }

    [Fact]

    public void Delete_ExistingGenre_ReturnsTrue()
    {
        genreRepositoryMock.Setup(r => r.Delete(1)).Returns(Task.FromResult(true));
        var result = genreService.Delete(1).Result;


        Assert.True(result);
    }
}