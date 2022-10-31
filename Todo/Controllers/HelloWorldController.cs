using Microsoft.AspNetCore.Mvc;

namespace Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        private static string WELCOME_MSG = "Hello World";
        
        // GET: api/HelloWorld
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { WELCOME_MSG.Split(" ")[0], WELCOME_MSG.Split(" ")[1] };
        }

        // GET: api/HelloWorld/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return WELCOME_MSG;
        }

        // POST: api/HelloWorld
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/HelloWorld/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/HelloWorld/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
