using BeFitApp.Data;
using BeFitApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeFitApp.Controllers
{
    [Authorize]
    public class ExerciseSessionsController : Controller
    {
        private readonly AppDbContext _context;

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public ExerciseSessionsController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var sessions = _context.ExerciseSessions
                .Where(es => es.CreatedById == GetUserId())
                .OrderByDescending(es => es.StartDateTime);

            return View(await sessions.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseSession = await _context.ExerciseSessions
                .Where(es => es.CreatedById == GetUserId())
                .Include(es => es.Exercises)
                    .ThenInclude(e => e.ExerciseTypeNav)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exerciseSession == null)
            {
                return NotFound();
            }

            return View(exerciseSession);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDateTime,EndDateTime,Notes,IntensityLevel,IsCompleted")] ExerciseSession exerciseSession)
        {
            exerciseSession.CreatedById = GetUserId();

            if (ModelState.IsValid)
            {
                _context.Add(exerciseSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exerciseSession);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseSession = await _context.ExerciseSessions
                .Where(es => es.CreatedById == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exerciseSession == null)
            {
                return NotFound();
            }
            return View(exerciseSession);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDateTime,EndDateTime,Notes,IntensityLevel,IsCompleted")] ExerciseSession exerciseSession)
        {
            if (id != exerciseSession.Id)
            {
                return NotFound();
            }

            if (!ExerciseSessionExists(exerciseSession.Id, GetUserId()))
            {
                return NotFound();
            }
            var existingSession = await _context.ExerciseSessions.FindAsync(id);
            if (existingSession != null)
            {
                exerciseSession.CreatedById = existingSession.CreatedById;
            }

            if (ModelState.IsValid)
            {
                _context.Update(exerciseSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exerciseSession);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseSession = await _context.ExerciseSessions
                .Where(es => es.CreatedById == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exerciseSession == null)
            {
                return NotFound();
            }

            return View(exerciseSession);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ExerciseSessionExists(id, GetUserId()))
            {
                var exerciseSession = await _context.ExerciseSessions.FindAsync(id);
                _context.ExerciseSessions.Remove(exerciseSession);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ExerciseSessionExists(int id, string userId)
        {
            return _context.ExerciseSessions.Any(e => e.Id == id && e.CreatedById == userId);
        }
    }
}