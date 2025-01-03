using AppReserve.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppReserve.Domain.Interfaces.Repositories
{
    public interface ISpaceRepository
    {
        Task<Space> GetSpaceByIdAsync(int spaceId);
    }
}
