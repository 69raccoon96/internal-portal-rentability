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
        /// <summary>
        /// Validate user and return all customers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.GetCustomers());
        }
    }
}