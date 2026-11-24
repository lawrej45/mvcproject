using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InstructorInformationSystem.Data;
using InstructorInformationSystem.Models;

namespace InstructorInformationSystem.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IPasswordHasher<UserAccount> _hasher;

    public AccountController(ApplicationDbContext db, IPasswordHasher<UserAccount> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    // GET: /Account/Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var adminId = model.AdminId.Trim();
        var email = model.Email.Trim().ToLowerInvariant();

        if (await _db.UserAccounts.AnyAsync(u => u.Email.ToLower() == email))
            ModelState.AddModelError(nameof(model.Email), "An account with this email already exists.");

        if (await _db.UserAccounts.AnyAsync(u => u.AdminId == adminId))
            ModelState.AddModelError(nameof(model.AdminId), "This Admin ID is already registered.");

        if (!ModelState.IsValid)
            return View(model);

        var user = new UserAccount
        {
            CreatedAt = DateTime.UtcNow,
            AdminId = adminId,
            FirstName = model.FirstName.Trim(),
            FullName = $"{model.FirstName.Trim()} {model.LastName.Trim()}",
            LastName = model.LastName.Trim(),
            Email = email
        };
        user.PasswordHash = _hasher.HashPassword(user, model.Password);

        _db.UserAccounts.Add(user);
        await _db.SaveChangesAsync();

        TempData["Message"] = "Registration successful. Please log in.";
        return RedirectToAction(nameof(Login));
    }

    // Live duplicate checks used by the registration form (jQuery remote validation).
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IsAdminIdAvailable(string adminId)
    {
        var id = (adminId ?? string.Empty).Trim();
        return Json(!await _db.UserAccounts.AnyAsync(u => u.AdminId == id));
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IsEmailAvailable(string email)
    {
        var value = (email ?? string.Empty).Trim().ToLowerInvariant();
        return Json(!await _db.UserAccounts.AnyAsync(u => u.Email.ToLower() == value));
    }

    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction(nameof(Dashboard));

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var input = model.EmailOrId.Trim();
        var emailLower = input.ToLowerInvariant();

        var user = await _db.UserAccounts
            .FirstOrDefaultAsync(u => u.Email.ToLower() == emailLower || u.AdminId == input);

        if (user is null ||
            _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Invalid Email/Admin ID or password.");
            return View(model);
        }

        await SignInUserAsync(user);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Dashboard));
    }

    // GET: /Account/Dashboard
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        var user = await _db.UserAccounts.FindAsync(userId);

        if (user is null)
            return RedirectToAction(nameof(Login));

        ViewBag.InstructorCount = await _db.Instructors.CountAsync();
        ViewBag.SubjectCount = SubjectRepository.GetAll().Count;

        return View(user);
    }

    // POST: /Account/Logout
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Message"] = "You have been logged out.";
        return RedirectToAction(nameof(Login));
    }

    private async Task SignInUserAsync(UserAccount user)
    {
        var nameParts = new[] { user.FirstName.Trim(), user.LastName.Trim() };
        var displayName = string.Join(" ", Array.FindAll(nameParts, part => !part.Contains('@')));
        if (string.IsNullOrWhiteSpace(displayName))
            displayName = "User";

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, displayName),
            new(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            });
    }
}