using FeestBeest.Data.Dto;
namespace FeestBeest.Data.Services;

public interface IBeestjeService
{
    Task<List<BeestjeDto>> GetAllBeestjesAsync();
    Task<BeestjeDto> GetBeestjeByIdAsync(int id);
    Task CreateBeestjeAsync(BeestjeDto viewModel);
    Task UpdateBeestjeAsync(BeestjeDto viewModel);
    Task DeleteBeestjeAsync(int id);
    bool BeestjeExists(int id);
}