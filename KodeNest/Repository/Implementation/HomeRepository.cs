using DataLogger;
using Entity.Entity;
using Entity.Entity.Home;
using Entity.Enums;
using KodeNest.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace KodeNest.Repository.Implementation;

public class HomeRepository : IHomeRepository
{
    private readonly AppDbContext _context;
    private readonly DatLogger _logger;

    public HomeRepository(AppDbContext context, DatLogger logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<HomeResponse>> GetAllAsync()
    {
        try
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
        catch (Exception ex)
        {
            // TODO: Add proper logging here
            _logger.SaveDataLogs(new LogPayload
            {
                MethodName = nameof(GetByIdAsync),
                LogMessage = ex.Message,
                ExtraInformation = JsonConvert.SerializeObject(ex),
                CreatedBy = 1,
                LogType = Entity.Enums.LogType.Critical
            }, LogTarget.Database | LogTarget.File);

            return new List<HomeResponse>();
        }
    }

    public async Task<HomeResponse> GetByIdAsync(int id)
    {
        try
        {
            var h = await _context.Homes.FindAsync(id);
            if (h == null)
            {
                // TODO: Add proper logging here
                _logger.SaveDataLogs(new LogPayload
                {
                    MethodName = nameof(GetByIdAsync),
                    LogMessage = "Failed to find any record from DB",
                    ExtraInformation = $"Failed to find any record from DB, RequestID: {id}",
                    CreatedBy = 1,
                    LogType = Entity.Enums.LogType.Critical
                }, LogTarget.Database | LogTarget.File);

                return null;
            }
            else
            {
                // TODO: Add proper logging here
                _logger.SaveDataLogs(new LogPayload
                {
                    MethodName = nameof(GetByIdAsync),
                    LogMessage = "Successfully got record from database",
                    ExtraInformation = $"Successfully got record from database, RequestID: {id}",
                    CreatedBy = 1,
                    LogType = Entity.Enums.LogType.Critical
                }, LogTarget.Database | LogTarget.File);
            }

            return new HomeResponse
            {
                Id = h.Id,
                Title = h.Title,
                Address = h.Address,
                CreatedDate = h.CreatedDate
            };
        }
        catch (Exception ex)
        {
            // TODO: Add proper logging here
            _logger.SaveDataLogs(new LogPayload
            {
                MethodName = nameof(GetByIdAsync),
                LogMessage = ex.Message,
                ExtraInformation = JsonConvert.SerializeObject(ex),
                CreatedBy = 1,
                LogType = Entity.Enums.LogType.Critical
            }, LogTarget.Database | LogTarget.File);

            return null;
        }
    }

    public async Task<int> CreateAsync(HomeRequest request)
    {
        try
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
        catch (Exception ex)
        {
            // TODO: Add proper logging here
            _logger.SaveDataLogs(new LogPayload
            {
                MethodName = nameof(GetByIdAsync),
                LogMessage = ex.Message,
                ExtraInformation = JsonConvert.SerializeObject(ex),
                CreatedBy = 1,
                LogType = Entity.Enums.LogType.Critical
            }, LogTarget.Database | LogTarget.File);

            return 0;
        }
    }

    public async Task<bool> UpdateAsync(int id, HomeRequest request)
    {
        try
        {
            var entity = await _context.Homes.FindAsync(id);
            if (entity == null) return false;

            entity.Title = request.Title;
            entity.Address = request.Address;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // TODO: Add proper logging here
            _logger.SaveDataLogs(new LogPayload
            {
                MethodName = nameof(GetByIdAsync),
                LogMessage = ex.Message,
                ExtraInformation = JsonConvert.SerializeObject(ex),
                CreatedBy = 1,
                LogType = Entity.Enums.LogType.Critical
            }, LogTarget.Database | LogTarget.File);

            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.Homes.FindAsync(id);
            if (entity == null)
            {
                // TODO: Add proper logging here
                _logger.SaveDataLogs(new LogPayload
                {
                    MethodName = nameof(GetByIdAsync),
                    LogMessage = "Failed to find any record from DB",
                    ExtraInformation = $"Failed to find any record from DB, RequestID: {id}",
                    CreatedBy = 1,
                    LogType = Entity.Enums.LogType.Warning
                }, LogTarget.Database | LogTarget.File);

                return false;
            }
            else
            {
                // TODO: Add proper logging here
                _logger.SaveDataLogs(new LogPayload
                {
                    MethodName = nameof(GetByIdAsync),
                    LogMessage = "Successfully found record for request",
                    ExtraInformation = $"Successfully found record for request, RequestID: {id}",
                    CreatedBy = 1,
                    LogType = Entity.Enums.LogType.Warning
                }, LogTarget.Database | LogTarget.File);
            }

            _context.Homes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            // TODO: Add proper logging here
            _logger.SaveDataLogs(new LogPayload
            {
                MethodName = nameof(GetByIdAsync),
                LogMessage = ex.Message,
                ExtraInformation = JsonConvert.SerializeObject(ex),
                CreatedBy = 1,
                LogType = Entity.Enums.LogType.Critical
            }, LogTarget.Database | LogTarget.File);

            return false;
        }
    }
}
