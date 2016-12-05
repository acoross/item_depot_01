using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bank.DbModel;
using bank.Contents;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace bank.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        readonly BankDbContext context;

        public ItemsController(BankDbContext context)
        {
            this.context = context;
        }

        // POST api/values
        [HttpPost]
        public ActionResult Post([FromBody]Transaction tran)
        {
            var personalBank = new PersonalBank(context, tran.owner_id);
            bool ret = personalBank.ProcessTransaction(tran);

            if (ret)
                return Ok();
            else
                return NotFound();
        }
    }
}
