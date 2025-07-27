using LinkShortner.Api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LinkShortner.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedirectController : Controller
    {
        private readonly LinkShortenerDbDbContext _dbContext;
         public RedirectController(LinkShortenerDbDbContext dbContext)  
         {
            _dbContext = dbContext;
         }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> Follow(string shortCode)
        {
            var url = await _dbContext.Urls.FirstOrDefaultAsync(user => user.ShortCode == shortCode);

            if (url == null)
            {
                return NotFound("Short link not found.");
            }

            var click = new Click { UrlId = url.Id };
            _dbContext.Clicks.Add(click);
            await _dbContext.SaveChangesAsync();

            return Redirect(url.LongUrl);
        }

    }
}
