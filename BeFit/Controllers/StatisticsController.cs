using BeFitApp.Data;
using BeFitApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeFitApp.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _context;
        public StatisticsController(AppDbContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var timeUpdate = DateTime.Now.AddDays(-30); // Last 4 weeks
            var stats = await _context.Exercises
                .Include(e => e.ExerciseSessionNav)
                .Include(e => e.ExerciseTypeNav)
                .Where(e => e.ExerciseSessionNav.CreatedById == userId && e.ExerciseSessionNav.StartDateTime >= timeUpdate)
                .GroupBy(e => e.ExerciseTypeNav.ExerciseName)
                .Select(g => new UserViewStatistics
                {
                    ExerciseTypeName = g.Key,
                    TimesPerformed = g.Count(),
                    TotalRepetitions = g.Sum(e => e.Sets * e.Repetitions),
                    AverageWeight = g.Average(e => e.Weight ?? 0),
                    MaxWeight = g.Max(e => e.Weight ?? 0)
                })
                .ToListAsync();
            return View(stats);
        }
    }
}
