using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("customers")]
    public class CustomerController
    {
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            var db = new DataBase();
            return db.GetCustomers();
        }
    }
}