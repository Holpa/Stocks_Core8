using api.Data;
using api.DTOs;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //defered execution
            var _stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());
            return Ok(_stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var _stock = _context.Stocks.Find(id);

            if (_stock == null)
            {
                return NotFound();
            }
            return Ok(_stock.ToStockDto());
        }

    }
}