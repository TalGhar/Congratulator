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

        [HttpGet]
        public JsonResult Get()
        {
            return Json(from user in this.context.Users select user);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = this.context.Users.SingleOrDefault(t => t.Id == id);

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

            this.context.Add(user);
            this.context.SaveChanges();

            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }


        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] Models.User updatedUser)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = this.context.Users.SingleOrDefault(t => t.Id == id);

            if (user == null)
                return NotFound();

            user.Update(updatedUser);

            this.context.Update(user);
            this.context.SaveChanges();

            return Ok();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var user = this.context.Users.SingleOrDefault(t => t.Id == id);

            if (user == null)
                return NotFound();

            this.context.Users.Remove(user);
            this.context.SaveChanges();

            return Ok();
        }
    }
}