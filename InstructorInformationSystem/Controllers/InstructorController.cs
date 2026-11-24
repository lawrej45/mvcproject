using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstructorInformationSystem.Data;
using InstructorInformationSystem.Models;

namespace InstructorInformationSystem.Controllers;

[Authorize]
public class InstructorController : Controller
{
    private const string CompletionRequiredMessage = "Complete the required fields below before viewing this instructor's details.";
    private readonly ApplicationDbContext _context;

    public InstructorController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Instructor
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 50;
        page = Math.Max(1, page);

        var instructors = await _context.Instructors
            .AsNoTracking()
            .OrderBy(i => i.FullName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize + 1)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.HasPreviousPage = page > 1;
        ViewBag.HasNextPage = instructors.Count > pageSize;

        return View(instructors.Take(pageSize).ToList());
    }

    // GET: /Instructor/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor is null) return NotFound();

        if (!TryValidateModel(instructor))
            return RedirectToAction(nameof(Edit), new { id, completeProfile = true });

        return View(instructor);
    }

    // GET: /Instructor/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Instructor/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Instructor model)
    {
        await PrepareAsync(model);
        if (!ModelState.IsValid)
            return View(model);

        _context.Instructors.Add(model);
        await _context.SaveChangesAsync();

        TempData["Message"] = $"Instructor {model.FullName} was added.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Instructor/Edit/5
    public async Task<IActionResult> Edit(int id, bool completeProfile = false)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor is null) return NotFound();

        if (completeProfile)
        {
            ViewBag.CompletionNotice = CompletionRequiredMessage;
            TryValidateModel(instructor);
        }

        return View(instructor);
    }

    // POST: /Instructor/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Instructor model, bool completeProfile = false)
    {
        await PrepareAsync(model);
        if (!ModelState.IsValid)
        {
            if (completeProfile)
                ViewBag.CompletionNotice = CompletionRequiredMessage;

            return View(model);
        }

        _context.Instructors.Update(model);
        await _context.SaveChangesAsync();

        TempData["Message"] = $"Instructor {model.FullName} was updated.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Instructor/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor is null) return NotFound();

        return View(instructor);
    }

    // POST: /Instructor/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var instructor = await _context.Instructors.FindAsync(id);
        if (instructor is not null)
        {
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            TempData["Message"] = $"Instructor {instructor.FullName} was deleted.";
        }

        return RedirectToAction(nameof(Index));
    }

    // Trims input, builds FullName, and rejects an Instructor ID that is already used.
    private async Task PrepareAsync(Instructor model)
    {
        model.InstructorId = (model.InstructorId ?? string.Empty).Trim();
        model.FirstName = (model.FirstName ?? string.Empty).Trim();
        model.LastName = (model.LastName ?? string.Empty).Trim();
        model.FullName = $"{model.FirstName} {model.LastName}".Trim();

        if (model.InstructorId.Length > 0 && model.InstructorId != "N/A" &&
            await _context.Instructors.AnyAsync(i => i.InstructorId == model.InstructorId && i.Id != model.Id))
        {
            ModelState.AddModelError(nameof(Instructor.InstructorId), "This Instructor ID already exists.");
        }
    }
}
