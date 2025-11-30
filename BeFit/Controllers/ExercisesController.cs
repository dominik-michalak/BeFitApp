using BeFitApp.Data;
using BeFitApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BeFitApp.Controllers
{
    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly AppDbContext _context;

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public ExercisesController(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var exercises = _context.Exercises
                .Where(e => e.ExerciseSessionNav.CreatedById == GetUserId())
                .Include(e => e.ExerciseTypeNav)
                .Include(e => e.ExerciseSessionNav)
                .OrderByDescending(e => e.ExerciseSessionNav.StartDateTime);

            return View(await exercises.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.ExerciseTypeNav)
                .Include(e => e.ExerciseSessionNav)
                .FirstOrDefaultAsync(m => m.Id == id && m.ExerciseSessionNav.CreatedById == GetUserId());

            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        public IActionResult Create()
        {
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "ExerciseName");
            ViewData["ExerciseSessionId"] = new SelectList(
                _context.ExerciseSessions
                    .Where(es => es.CreatedById == GetUserId())
                    .OrderByDescending(es => es.StartDateTime)
                    .Select(es => new
                    {
                        es.Id,
                        DisplayText = es.StartDateTime.ToString("yyyy-MM-dd HH:mm")
                    }),
                "Id",
                "DisplayText");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExerciseTypeId,ExerciseSessionId,Sets,Repetitions,Weight,Notes,DifficultyLevel")] Exercise exercise)
        {
            var session = await _context.ExerciseSessions
                .FirstOrDefaultAsync(es => es.Id == exercise.ExerciseSessionId && es.CreatedById == GetUserId());

            if (session == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "ExerciseName", exercise.ExerciseTypeId);
            ViewData["ExerciseSessionId"] = new SelectList(
                _context.ExerciseSessions
                    .Where(es => es.CreatedById == GetUserId())
                    .OrderByDescending(es => es.StartDateTime)
                    .Select(es => new
                    {
                        es.Id,
                        DisplayText = es.StartDateTime.ToString("yyyy-MM-dd HH:mm")
                    }),
                "Id",
                "DisplayText",
                exercise.ExerciseSessionId);
            return View(exercise);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.ExerciseSessionNav)
                .Where(e => e.ExerciseSessionNav.CreatedById == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "ExerciseName", exercise.ExerciseTypeId);
            ViewData["ExerciseSessionId"] = new SelectList(
                _context.ExerciseSessions
                    .Where(es => es.CreatedById == GetUserId())
                    .OrderByDescending(es => es.StartDateTime)
                    .Select(es => new
                    {
                        es.Id,
                        DisplayText = es.StartDateTime.ToString("yyyy-MM-dd HH:mm")
                    }),
                "Id",
                "DisplayText",
                exercise.ExerciseSessionId);
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExerciseTypeId,ExerciseSessionId,Sets,Repetitions,Weight,Notes,DifficultyLevel")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            // Verify ownership
            if (!ExerciseExists(exercise.Id, GetUserId()))
            {
                return NotFound();
            }

            var session = await _context.ExerciseSessions
                .FirstOrDefaultAsync(es => es.Id == exercise.ExerciseSessionId && es.CreatedById == GetUserId());

            if (session == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "ExerciseName", exercise.ExerciseTypeId);
            ViewData["ExerciseSessionId"] = new SelectList(
                _context.ExerciseSessions
                    .Where(es => es.CreatedById == GetUserId())
                    .OrderByDescending(es => es.StartDateTime)
                    .Select(es => new
                    {
                        es.Id,
                        DisplayText = es.StartDateTime.ToString("yyyy-MM-dd HH:mm")
                    }),
                "Id",
                "DisplayText",
                exercise.ExerciseSessionId);
            return View(exercise);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.ExerciseTypeNav)
                .Include(e => e.ExerciseSessionNav)
                .Where(e => e.ExerciseSessionNav.CreatedById == GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ExerciseExists(id, GetUserId()))
            {
                var exercise = await _context.Exercises.FindAsync(id);
                if (exercise != null)
                {
                    _context.Exercises.Remove(exercise);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id, string userId)
        {
            return _context.Exercises.Any(e => e.Id == id && e.ExerciseSessionNav.CreatedById == userId);
        }
    }
}