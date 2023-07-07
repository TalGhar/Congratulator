using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Congratulator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Congratulator.Models;

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
        public async Task<IActionResult> Create([Bind("Name,Surname,BDate,Avatar")] Models.User newUser)
        {

            var user = new Models.UserDbModel(newUser);

            this.context.Add(user);

            await this.context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await this.context.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await this.context.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(user => user.Id == id);

            this.context.Users.Remove(user);

            await this.context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = await this.context.Users.SingleOrDefaultAsync(user => user.Id == id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Surname,Bdate,Avatar")] Models.UserDbModel task)
        {
            if (id != task.Id)
                return NotFound();

            if (!this.context.Users.Any(t => t.Id == id))
                return NotFound();

            if (ModelState.IsValid)
            {
                this.context.Update(task);

                await this.context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sort)
        {
            ViewBag.NameSortParm = (sort == "Name" ? "Name_desc" : "Name");
            ViewBag.SurnameSortParm = (sort == "Surname" ? "Surname_desc" : "Surname");
            ViewBag.BDateSortParm = (sort == "BDate" ? "BDate_desc" : "BDate");
            ViewBag.AvatarSortParm = (sort == "Avatar" ? "Avatar_desc" : "Avatar");

            ViewData["sortJSON"] = sort;

            return View(await this.GetSorted(sort).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetJSON(string sort)
        {
            return Json(await this.GetSorted(sort).ToListAsync());
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
                "Avatar" => users.OrderBy(s => s.Avatar),
                "Avatar_desc" => users.OrderByDescending(s => s.Avatar),
                _ => users.OrderBy(s => s.Name),
            };

            return users;
        }
    }
}