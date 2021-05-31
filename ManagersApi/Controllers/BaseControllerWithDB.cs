using ManagersApi.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace ManagersApi.Controllers
{
    public class BaseControllerWithDb : ControllerBase
    {
        protected IDataBase db;

        public BaseControllerWithDb(IDataBase db)
        {
            this.db = db;
        }
    }
}