using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordConvertor.API.Services;

namespace WordConvertor.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<WordConverter> _logger;
        private readonly IWordConverter _wordConverter;

        public ValuesController(IWordConverter wordConverter, ILogger<WordConverter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _wordConverter = wordConverter ?? throw new ArgumentNullException(nameof(wordConverter));
        }
        //WordConverter wordConverter = new WordConverter();


        [HttpGet("{number}")]
        public IActionResult Get(string number)
        {
            try {
                    if (number == null)
                    {
                        _logger.LogInformation($"User must provide a value for currency.");
                        return NotFound();
                    }
                    return Ok(new { Word = _wordConverter.ValidateNumber(number) });
                 }
                  
            catch (Exception ex)
            {
                _logger.LogInformation($"Currency value  {number} was not found to convert.", ex);
                return StatusCode(500, "A problem happened while handeling your request");
            }
        }
    }
}