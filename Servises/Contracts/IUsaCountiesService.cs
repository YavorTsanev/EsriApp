namespace Services.Contracts
{
    using Dtos;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsaCountiesService
    {
        Task<ICollection<UsaStateResponseModel>> GetAllAsync();

        Task<UsaStateResponseModel> GetByStateNameAsync(string stateName);
    }
}
