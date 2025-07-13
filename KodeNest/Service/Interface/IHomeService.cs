using Entity.Entity.Home;

namespace KodeNest.Service.Interface;
public interface IHomeService
{
    Task<List<HomeResponse>> GetAllAsync();
    Task<HomeResponse> GetByIdAsync(int id);
    Task<int> CreateAsync(HomeRequest request);
    Task<bool> UpdateAsync(int id, HomeRequest request);
    Task<bool> DeleteAsync(int id);
}
