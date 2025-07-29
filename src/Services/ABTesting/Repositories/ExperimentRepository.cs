using ABTesting.Entities;
using ABTesting.Data;
using Microsoft.EntityFrameworkCore;

namespace ABTesting.Repositories;

public class ExperimentRepository : IExperimentRepository
{
    private readonly ABTestingDbContext _context;
    public ExperimentRepository(ABTestingDbContext context) => _context = context;

    public async Task<IEnumerable<Experiment>> GetAllAsync() => await _context.Experiments.Include(e => e.Variants).ToListAsync();
    public async Task<Experiment?> GetByIdAsync(Guid id) => await _context.Experiments.Include(e => e.Variants).FirstOrDefaultAsync(e => e.Id == id);
    public async Task<Experiment> AddAsync(Experiment experiment) { _context.Experiments.Add(experiment); await _context.SaveChangesAsync(); return experiment; }
    public async Task UpdateAsync(Experiment experiment) { _context.Experiments.Update(experiment); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(Guid id) { var exp = await _context.Experiments.FindAsync(id); if (exp != null) { _context.Experiments.Remove(exp); await _context.SaveChangesAsync(); } }
    public async Task StartAsync(Guid id) { var exp = await _context.Experiments.FindAsync(id); if (exp != null) { exp.Status = "Running"; exp.StartDate = DateTime.UtcNow; await _context.SaveChangesAsync(); } }
    public async Task StopAsync(Guid id) { var exp = await _context.Experiments.FindAsync(id); if (exp != null) { exp.Status = "Stopped"; exp.EndDate = DateTime.UtcNow; await _context.SaveChangesAsync(); } }
    public async Task<IEnumerable<UserAssignment>> GetAssignmentsAsync(Guid experimentId) => await _context.UserAssignments.Where(a => a.ExperimentId == experimentId).ToListAsync();
    public async Task<UserAssignment> AssignUserAsync(Guid experimentId, Guid userId) { /* распределение по вариантам */ var exp = await _context.Experiments.Include(e => e.Variants).FirstOrDefaultAsync(e => e.Id == experimentId); if (exp == null) throw new Exception("Experiment not found"); var variant = exp.Variants.First(); var assignment = new UserAssignment { Id = Guid.NewGuid(), UserId = userId, ExperimentId = experimentId, VariantId = variant.Id, AssignedAt = DateTime.UtcNow }; _context.UserAssignments.Add(assignment); await _context.SaveChangesAsync(); return assignment; }
    public async Task<object> GetResultsAsync(Guid experimentId) { /* статистика */ return new { }; }
} 