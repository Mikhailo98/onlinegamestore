using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Models.Dtos.PublisherDto;

namespace BusinessLogicLayer.Interfaces
{
    public interface IPublisherService
    {
        Task CreatePublisher(CreatePublisherDto newPublisher);
        Task<List<PublisherDto>> GetAll();
        Task<PublisherDto> GetInfo(int id);
        Task EditPublisher(EditPublisherDto genreDto);
        Task<List<GameDto>> GetGamesOfPublisher(int id);
    }
}