using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Congratulator.Data;


namespace Congratulator.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserDbContext context;

        public UsersController(UserDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Models.User());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.User newUser, IFormFile Avatar)
        {

            if (newUser.Name != null && newUser.Surname != null)
            {
                if (Avatar != null)
                {

                    var fileName = Avatar.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    newUser.Avatar = fileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Avatar.CopyToAsync(stream);
                    }

                }
                else
                {
                    newUser.Avatar = "empty.png";
                }

                var user = new Models.UserDbModel(newUser);

                context.Add(user);

                await context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await context.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await context.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await context.Users.SingleOrDefaultAsync(user => user.Id == id);

            context.Users.Remove(user);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await context.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Surname,Bdate,Avatar")] Models.UserDbModel user, IFormFile Avatar)
        {
            if (id != user.Id)
                return NotFound();

            if (!context.Users.Any(u => u.Id == id))
                return NotFound();

            if (Avatar != null)
            {
                user.Avatar = Avatar.FileName;
            }
            else
            {
                user.Avatar = "empty.png";
            }

            context.Update(user);

            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.NameSortParm = (sort == "Name" ? "Name_desc" : "Name");
            ViewBag.SurnameSortParm = (sort == "Surname" ? "Surname_desc" : "Surname");
            ViewBag.BDateSortParm = (sort == "BDate" ? "BDate_desc" : "BDate");

            return View(await GetSorted(sort).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Near(string sort)
        {
            ViewBag.NameSortParm = (sort == "Name" ? "Name_desc" : "Name");
            ViewBag.SurnameSortParm = (sort == "Surname" ? "Surname_desc" : "Surname");
            ViewBag.BDateSortParm = (sort == "BDate" ? "BDate_desc" : "BDate");

            return View(await GetNear(sort).ToListAsync());
        }

        private IQueryable<Models.UserDbModel> GetNear(string sort)
        {
            var today = DateTime.Today;
            var week = today.AddDays(7);
            var users = context.Users.Where(u => u.BDate.DayOfYear >= today.DayOfYear && u.BDate.DayOfYear <= week.DayOfYear);

            users = sort switch
            {
                "Name" => users.OrderBy(s => s.Name),
                "Name_desc" => users.OrderByDescending(s => s.Name),
                "Surname" => users.OrderBy(s => s.Surname),
                "Surname_desc" => users.OrderByDescending(s => s.Surname),
                "BDate" => users.OrderBy(s => s.BDate),
                "BDate_desc" => users.OrderByDescending(s => s.BDate),
                _ => users.OrderBy(s => s.Name),
            };

            return users;
        }

        private IQueryable<Models.UserDbModel> GetSorted(string sort)
        {
            var users = from user in this.context.Users select user;

            users = sort switch
            {
                "Name" => users.OrderBy(s => s.Name),
                "Name_desc" => users.OrderByDescending(s => s.Name),
                "Surname" => users.OrderBy(s => s.Surname),
                "Surname_desc" => users.OrderByDescending(s => s.Surname),
                "BDate" => users.OrderBy(s => s.BDate),
                "BDate_desc" => users.OrderByDescending(s => s.BDate),
                _ => users.OrderBy(s => s.Name),
            };

            return users;
        }
    }
}