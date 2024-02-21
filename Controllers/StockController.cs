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


        /// <summary>
        /// adding Async, reason for this because we dont want the client to wait on us
        /// for better user experience
        /// add async and Task<YourDesiredReturn> on the function signature
        /// then you have to have 'await' on the code that leaves your system
        /// or any processes that takes long time to do
        /// side note ToList can be either ToListAsync since it can be a heavy process
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //defered execution
            var _stocks = await _context.Stocks.ToListAsync();
            var stockDTO = _stocks.Select(s => s.ToStockDto());

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

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            //make sure id is nothing but number!
            //find the record
            var _stock = _context.Stocks.FirstOrDefault(x => x.id == id);
            if (_stock == null)
            {
                return NotFound();
            }
            //update that object
            _stock.Symbol = stockDto.Symbol;
            _stock.CompanyName = stockDto.CompanyName;
            _stock.LastDiv = stockDto.LastDiv;
            _stock.Industry = stockDto.Industry;
            _stock.MarketCap = stockDto.MarketCap;
            //save the object
            _context.SaveChanges();
            return Ok(_stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var _stock = _context.Stocks.FirstOrDefault(x => x.id == id);
            if (_stock == null)
            {
                return NotFound();
            }
            _context.Stocks.Remove(_stock);
            _context.SaveChanges();

            return NoContent();
        }

    }
}