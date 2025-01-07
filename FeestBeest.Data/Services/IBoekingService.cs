﻿using FeestBeest.Data.Dto;

namespace FeestBeest.Data.Services
{
    
    public interface IBoekingService
    {
        Task<List<BeestjeDto>> GetBeschikbareBeestjesAsync(DateTime datum);
        
        Task<List<Beestje>> GetBeschikbareBeestjesMappedAsync(DateTime selectedDate);

        Task<BoekingDto> MaakBoekingAsync(BoekingDto boekingDto);
        Task<BoekingDto> GetBoekingByIdAsync(int id);
        Task<List<BoekingDto>> GetAlleBoekingenAsync();
        Task VerwijderBoekingAsync(int id);
    }
}

