using Entity.Entity.Home;
using KodeNest.Repository.Interface;
using KodeNest.Service.Interface;

namespace KodeNest.Service.Implementation;
public class HomeService : IHomeService
{
    private readonly IHomeRepository _repository;

    public HomeService(IHomeRepository repository)
    {
        _repository = repository;
    }

    public Task<List<HomeResponse>> GetAllAsync() => _repository.GetAllAsync();

    public Task<HomeResponse> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    public Task<int> CreateAsync(HomeRequest request) => _repository.CreateAsync(request);

    public Task<bool> UpdateAsync(int id, HomeRequest request) => _repository.UpdateAsync(id, request);

    public Task<bool> DeleteAsync(int id) => _repository.DeleteAsync(id);
}
