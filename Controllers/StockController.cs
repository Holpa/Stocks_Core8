using api.Data;
using api.DTOs;
using api.Mappers;
using api.DTOs.Stock;
using Microsoft.AspNetCore.Mvc;
using api.Interfaces;


namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //defered execution
            var _stocks = await _stockRepo.GetAllAsync();
            var stockDTO = _stocks.Select(s => s.ToStockDto());

            return Ok(_stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            // 'validation'
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _stock = await _stockRepo.GetByIdAsync(id);

            if (_stock == null)
            {
                return NotFound();
            }
            return Ok(_stock.ToStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _stockModel = stockDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(_stockModel);
            // here we are doing multiple things
            // create an ID and pass it with the stockDTO info 
            // reason for that beacuse stockDto is lacking id variable 
            // so first we save the entry then the DB will generate the id
            // then we return back the info to the user + the id included
            return CreatedAtAction(nameof(GetById), new { id = _stockModel.id },
            _stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id,
         [FromBody] UpdateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //make sure id is nothing but number!
            //find the record
            var _stock = await _stockRepo.UpdateAsync(id, stockDto);
            if (_stock == null)
            {
                return NotFound();
            }
            //update that object

            //save the object
            return Ok(_stock.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var _stock = await _stockRepo.DeleteAsync(id);
            if (_stock == null)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}