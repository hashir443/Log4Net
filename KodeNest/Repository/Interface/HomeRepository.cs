using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KodeNest.Entity;
using KodeNest.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace KodeNest.Repository.Interface;

public class HomeRepository : IHomeRepository
{
    private readonly AppDbContext _context;

    public HomeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<HomeResponse>> GetAllAsync()
    {
        return await _context.Homes
            .Select(h => new HomeResponse
            {
                Id = h.Id,
                Title = h.Title,
                Address = h.Address,
                CreatedDate = h.CreatedDate
            }).ToListAsync();
    }

    public async Task<HomeResponse> GetByIdAsync(int id)
    {
        var h = await _context.Homes.FindAsync(id);
        if (h == null) return null;

        return new HomeResponse
        {
            Id = h.Id,
            Title = h.Title,
            Address = h.Address,
            CreatedDate = h.CreatedDate
        };
    }

    public async Task<int> CreateAsync(HomeRequest request)
    {
        var entity = new Home
        {
            Title = request.Title,
            Address = request.Address,
            CreatedDate = DateTime.UtcNow
        };
        _context.Homes.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, HomeRequest request)
    {
        var entity = await _context.Homes.FindAsync(id);
        if (entity == null) return false;

        entity.Title = request.Title;
        entity.Address = request.Address;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Homes.FindAsync(id);
        if (entity == null) return false;

        _context.Homes.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
