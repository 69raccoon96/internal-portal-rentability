using System.Collections.Generic;
using ManagersApi.DataBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("customers")]
    public class CustomerController : BaseControllerWithDb
    {
        public CustomerController(IDataBase db) : base(db)
        {
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.GetCustomers());
        }
    }
}