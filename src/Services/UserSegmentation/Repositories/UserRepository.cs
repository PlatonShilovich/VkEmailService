using Microsoft.EntityFrameworkCore;
using UserSegmentation.Data;
using UserSegmentation.Entities;

namespace UserSegmentation.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserSegmentationDbContext _context;

    public UserRepository(UserSegmentationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync(int page, int pageSize)
        => await _context.Users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    public async Task<User?> GetByIdAsync(Guid id)
        => await _context.Users.FindAsync(id);

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<User>> FilterAsync(SegmentCriteria criteria, int page, int pageSize)
    {
        var query = _context.Users.AsQueryable();
        if (criteria.AgeRange.HasValue)
        {
            query = query.Where(u => u.Age >= criteria.AgeRange.Value.Min && u.Age <= criteria.AgeRange.Value.Max);
        }
        if (criteria.Genders != null && criteria.Genders.Any())
        {
            query = query.Where(u => criteria.Genders.Contains(u.Gender));
        }
        if (criteria.Locations != null && criteria.Locations.Any())
        {
            query = query.Where(u => criteria.Locations.Contains(u.Location));
        }
        if (criteria.ActivityPeriod.HasValue)
        {
            query = query.Where(u => u.LastActivity >= criteria.ActivityPeriod.Value.From && u.LastActivity <= criteria.ActivityPeriod.Value.To);
        }
        return await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<int> CountAsync()
        => await _context.Users.CountAsync();
} 