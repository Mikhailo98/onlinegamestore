using System.Threading.Tasks;
using BusinessLogicLayer.Dtos;

namespace BusinessLogicLayer.Interfaces
{
    public interface IPublisherService
    {
        Task CreatePublisher(PublisherDto newPublisher);
    }
}