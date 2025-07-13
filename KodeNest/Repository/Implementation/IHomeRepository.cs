using System.Collections.Generic;
using System.Threading.Tasks;
using KodeNest.Entity;

namespace KodeNest.Repository.Implementation;
public interface IHomeRepository
{
    Task<List<HomeResponse>> GetAllAsync();
    Task<HomeResponse> GetByIdAsync(int id);
    Task<int> CreateAsync(HomeRequest request);
    Task<bool> UpdateAsync(int id, HomeRequest request);
    Task<bool> DeleteAsync(int id);
}
