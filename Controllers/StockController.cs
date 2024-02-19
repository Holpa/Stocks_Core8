using api.Data;
using api.DTOs;
using api.Mappers;
using api.DTOs.Stock;
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

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
            var _stockModel = stockDto.ToStockFromCreateDTO();
            _context.Stocks.Add(_stockModel);
            _context.SaveChanges();
            // here we are doing multiple things
            // create an ID and pass it with the stockDTO info 
            // reason for that beacuse stockDto is lacking id variable 
            // so first we save the entry then the DB will generate the id
            // then we return back the info to the user + the id included
            return CreatedAtAction(nameof(GetById), new { id = _stockModel.id },
            _stockModel.ToStockDto());
        }

    }
}