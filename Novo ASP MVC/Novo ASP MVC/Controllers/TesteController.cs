using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Novo_ASP_MVC.Controllers
{
    public class TesteController : ApiController
    {
        // GET: api/Teste
        public async Task<string> Get()
        {
            await Task.Delay(2000);
            return "async demorou 2 segundos";
        }

        // GET: api/Teste/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Teste
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Teste/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Teste/5
        public void Delete(int id)
        {
        }
    }
}
