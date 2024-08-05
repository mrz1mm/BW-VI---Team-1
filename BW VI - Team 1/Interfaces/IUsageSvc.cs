﻿using BW_VI___Team_1.Models;
using BW_VI___Team_1.Models.DTO;

namespace BW_VI___Team_1.Interfaces
{
    public interface IUsageSvc
    {
        Task<List<Usage>> GetAllUsagesAsync();
        Task<Usage> GetUsageByIdAsync(int id);
        Task<Usage> AddUsageAsync(UsageDTO usage);
        Task<Usage> UpdateUsageAsync(Usage usage);
        Task<bool> DeleteUsageAsync(int id);
    }
}
