

using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/protfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        public readonly IPortfolioRepository _portRepo;
        public PortfolioController(UserManager<AppUser> userManager,
         IStockRepository stockRepo, IPortfolioRepository portRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portRepo = portRepo;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }
    }
}