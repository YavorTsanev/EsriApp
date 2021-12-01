namespace Services
{
    using Contracts;
    using Data;
    using Dtos;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsaStatesService : IUsaCountiesService
    {
        private readonly ApplicationDbContext _db;

        public UsaStatesService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ICollection<UsaStateResponseModel>> GetAllAsync()
        {
            return await _db.UsaStates
                .Select(x => new UsaStateResponseModel
                {
                    Population = x.Population,
                    StateName = x.StateName,
                }).ToListAsync();
        }

        public async Task<UsaStateResponseModel> GetByStateNameAsync(string stateName)
        {
            return await _db.UsaStates.Where(x => x.StateName.ToUpper() == stateName.ToUpper())
                .Select(x => new UsaStateResponseModel
                {
                    StateName = x.StateName,
                    Population = x.Population,
                }).FirstOrDefaultAsync();
        }
    }
}
