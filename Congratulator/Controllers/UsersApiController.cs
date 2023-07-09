using Microsoft.AspNetCore.Mvc;
using Congratulator.Data;

namespace Congratulator.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersApiController : Controller
    {
        private readonly UserDbContext context;

        public UsersApiController(UserDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = context.Users.SingleOrDefault(t => t.Id == id);

            if (user == null)
                return NotFound();

            return Json(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Models.User newUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new Models.UserDbModel(newUser);

            context.Add(user);
            context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Models.User updatedUser)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = context.Users.SingleOrDefault(t => t.Id == id);

            if (user == null)
                return NotFound();

            user.Update(updatedUser);

            context.Update(user);
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = context.Users.SingleOrDefault(t => t.Id == id);

            if (user == null)
                return NotFound();

            context.Users.Remove(user);
            context.SaveChanges();

            return Ok();
        }
    }
}