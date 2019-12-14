using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace RealEstate.API.Controllers
{
   
   
    public class ValuesController : ApiController
    {
         [AllowAnonymous]
        [HttpGet]
        [Route("api/values/forall")]
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" , "Now server time is: " };
        }
        [Authorize (Roles ="admin")]
        [HttpGet]
        [Route("api/values/admin/authenticate")]
        public IHttpActionResult GetAdminAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok(" Hello " + identity.Name);
        }
        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("api/values/user/authenticate")]
        public IHttpActionResult GetUserAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;

            var roles = identity.Claims.Where(x => x.Type == ClaimTypes.Role)
                                .Select(c => c.Value);
            return Ok(" Xin chào " + identity.Name +" Roles: " + string.Join(",",roles.ToList()));
        }


        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}