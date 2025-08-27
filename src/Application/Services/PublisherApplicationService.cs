namespace LimbooCards.Application.Services
{
    using AutoMapper;
    using LimbooCards.Application.DTOs;
    using LimbooCards.Domain.Entities;
    using LimbooCards.Domain.Repositories;

    public class PublisherApplicationService(IPublisherRepository publisherRepository, IMapper mapper)
    {
        private readonly IPublisherRepository publisherRepository = publisherRepository;
        private readonly IMapper mapper = mapper;

        public async Task<PublisherDto> CreatePublisherAsync(CreatePublisherDto dto)
        {
            var publisher = mapper.Map<Publisher>(dto);

            await this.publisherRepository.AddPublisherAsync(publisher);

            return mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto?> GetPublisherByIdAsync(Guid publisherId)
        {
            var publisher = await this.publisherRepository.GetPublisherByIdAsync(publisherId);
            if (publisher == null) return null;

            return mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto?> GetPublisherByNameAsync(string name)
        {
            var publisher = await this.publisherRepository.GetPublisherByNameAsync(name);
            if (publisher == null) return null;

            return mapper.Map<PublisherDto>(publisher);
        }

        public async Task<IEnumerable<PublisherDto>> GetAllPublishersAsync()
        {
            var publishers = await this.publisherRepository.GetAllPublishersAsync();
            return mapper.Map<IEnumerable<PublisherDto>>(publishers);
        }

        public async Task UpdatePublisherAsync(UpdatePublisherDto dto)
        {
            var publisher = mapper.Map<Publisher>(dto);

            await this.publisherRepository.UpdatePublisherAsync(publisher);
        }

        public async Task DeletePublisherAsync(Guid publisherId)
        {
            await this.publisherRepository.DeletePublisherAsync(publisherId);
        }
    }
}