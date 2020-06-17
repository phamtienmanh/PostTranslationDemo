using System.Collections.Generic;
using WebApplication1.DataAccess.Entities;

namespace WebApplication1.DataAccess.Repositories
{
    public interface IAppDbRepository
    {
        bool Save();
    }
}
