using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface  IPlatformTypeService
    {
        Task AddPlatform(PlatformTypeCreate platform);
        Task DeletePlatform(int id);
        Task EditPlatform(int id, PlatformTypeDto editedGame);
        Task<PlatformTypeDto> GetInfo(int id);
    }
}
