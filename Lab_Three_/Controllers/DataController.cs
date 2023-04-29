using Lab_Three_.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Lab_Three_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly UserManager<Employee> _userManager;


        public DataController(UserManager<Employee> userManager)
        {
            _userManager = userManager;
        }
        #region Getting Data
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetSecureData()
        {
            //  var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // var User = await _userManager.FindByIdAsync(UserId);
            var user = await _userManager.GetUserAsync(User);
            return Ok(new String[] {
                "Reem",
                "Samy",
                "Mustafa",
               user!.Email!,
               user!.Department

                });
        }
        #endregion

          #region Admin Accsses
        [HttpGet]
        [Authorize(Policy="Admin")]
        [Route("ForAdmin")]
        public ActionResult GetSecureDataForManagers()
        {
            return Ok(new string[]
            {
                "Reem",
                "Khadija",
                "Yazen",
                "Jamal",
                "This Data From Admins Only"
            });
        }
        #endregion

        #region Users Accssec
        [HttpGet]
        [Authorize(Policy = "User")]
        [Route("ForUser")]
        public ActionResult GetSecureDataForEmployees()
        {
            return Ok(new string[]
            {
                "Malek",
                "Samy",
                "Mustafa",
                "Abdullrahman",
                "Tasbeeh",
                "This Data From Users Only"
            });
        }
    }
}
        #endregion



