using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Managers
{
    public static class RequestParser
    {
        public static string ParseRequest(HttpContext content)
        {
            return "a.ToString()";
        }
    }
}