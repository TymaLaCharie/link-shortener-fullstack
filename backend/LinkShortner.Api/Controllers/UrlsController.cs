using LinkShortner.Api.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NanoidDotNet;
using System.Security.Claims;


namespace LinkShortner.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UrlsController : Controller
    {
        private readonly LinkShortenerDbDbContext _dbContext;

        public UrlsController(LinkShortenerDbDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("analytics")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserAnalytics()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userLinks = await _dbContext.Urls
                .Where(user => user.UserId == userId)
                .OrderByDescending(user => user.CreatedAt)
                .Select(user => new
                {
                    user.LongUrl,
                    ShortUrl = $"{Request.Scheme}://{Request.Host}/redirect/{user.ShortCode}",
                    user.ShortCode,
                    user.CreatedAt,
                    ClickCount = user.Clicks.Count()
                })
                .ToListAsync();

            return Ok(userLinks);
        }

        [HttpPost]
        public async Task<ActionResult<Url>> PostUrl(UrlCreateRequest request)
        {
            if (!Uri.TryCreate(request.LongUrl,UriKind.Absolute, out _))
            {
                return BadRequest("Invalid URL format.");
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var shortCode = Nanoid.Generate(size: 5);

            while (await _dbContext.Urls.AnyAsync(u => u.ShortCode == shortCode))
            {
                shortCode = Nanoid.Generate(size: 5);
            }

            var url = new Url
            {
                LongUrl = request.LongUrl,
                ShortCode = shortCode,
                UserId = userId
            };

            _dbContext.Urls.Add(url);
            await _dbContext.SaveChangesAsync();


            return Ok(new
            {
                ShortUrl = $"{Request.Scheme}://{Request.Host}/redirect/{url.ShortCode}"
            });
        }

        public class UrlCreateRequest
        {
            public string LongUrl { get; set; } = string.Empty;
        }


    }
}
