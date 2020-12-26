using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using task_3.Models;

namespace task_3.Controllers
{
    public class TalentsController : ApiController
    {
        static readonly ITalentRepository repository = new TalentRepository();
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        [Route("api/v1/talents")]
        public IEnumerable<Talent> GetAllTalents()
        {
            return repository.GetAll();
        }

        [Route("api/v1/talents/{id:int}")]
        public Talent GetTalent(int id)
        {
            Talent item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

    }
}
