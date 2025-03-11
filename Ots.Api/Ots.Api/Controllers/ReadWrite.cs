using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ots.Api.Controllers
{
    [Route("api/[controller]")]   // Route : bu apiye ulasirken kullanacagimiz api path
    [ApiController]  //bu attribure ustteki de
    public class ReadWrite : ControllerBase
    {
        // GET: api/<ReadWrite>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ReadWrite>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ReadWrite>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<ReadWrite>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<ReadWrite>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
