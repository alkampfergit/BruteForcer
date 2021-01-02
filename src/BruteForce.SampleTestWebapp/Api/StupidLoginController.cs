using Microsoft.AspNetCore.Mvc;

namespace BruteForce.SampleTestWebapp.Api
{
    [Route("api/stupid-login")]
    public class StupidLoginController : Controller
    {
        /// <summary>
        /// Ach, someone uses get login... :D
        /// Sample call: curl -i http://localhost:5000/api/stupid-login/login-get?password=buongusto&userName=fred 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("login-get")]
        public IActionResult Index(string userName, string password)
        {
            if (userName == "fred" && password == "buongusto")
            {
                return Json(new
                {
                    Success = true,
                    Id = "User_123"
                });
            }
            
            return Json(new
            {
                Success = false,
                Id = "User_123"
            });
        }
    }
}