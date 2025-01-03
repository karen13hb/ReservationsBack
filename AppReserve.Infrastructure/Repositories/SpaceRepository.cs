using AppReserve.Domain.Entities;
using AppReserve.Domain.Interfaces.Repositories;
using AppReserve.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Infrastructure.Persistence.Repositories
{
    public class SpaceRepository : ISpaceRepository
    {
        private readonly AppDbContext _dbContext;

        public SpaceRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Space> GetSpaceByIdAsync(int spaceId)
        {
            var space = await _dbContext.Spaces.FindAsync(spaceId);
            return space;
        }



    }

}
