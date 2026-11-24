using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InstructorInformationSystem.Data;
using InstructorInformationSystem.Models;

namespace InstructorInformationSystem.Controllers;

[Authorize]
public class SubjectController : Controller
{
    // GET: /Subject
    public IActionResult Index()
    {
        return View(SubjectRepository.GetAll());
    }

    // GET: /Subject/Details/5
    public IActionResult Details(int id)
    {
        var subject = SubjectRepository.GetById(id);
        if (subject is null) return NotFound();

        return View(subject);
    }

    // GET: /Subject/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Subject/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Subject model)
    {
        if (!ModelState.IsValid)
            return View(model);

        SubjectRepository.Add(model);
        TempData["Message"] = $"Subject {model.SubjectCode} was added.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Subject/Edit/5
    public IActionResult Edit(int id)
    {
        var subject = SubjectRepository.GetById(id);
        if (subject is null) return NotFound();

        return View(subject);
    }

    // POST: /Subject/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Subject model)
    {
        if (!ModelState.IsValid)
            return View(model);

        SubjectRepository.Update(model);
        TempData["Message"] = $"Subject {model.SubjectCode} was updated.";
        return RedirectToAction(nameof(Index));
    }

    // GET: /Subject/Delete/5
    public IActionResult Delete(int id)
    {
        var subject = SubjectRepository.GetById(id);
        if (subject is null) return NotFound();

        return View(subject);
    }

    // POST: /Subject/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var subject = SubjectRepository.GetById(id);
        SubjectRepository.Delete(id);

        if (subject is not null)
            TempData["Message"] = $"Subject {subject.SubjectCode} was deleted.";

        return RedirectToAction(nameof(Index));
    }
}