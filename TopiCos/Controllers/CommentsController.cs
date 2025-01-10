using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TopiCos.Data;
using TopiCos.Models;

namespace TopiCos.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;


        public CommentsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult AddComment(int TopicId)
        {
            ViewBag.TopicId = TopicId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment, int TopicId)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var topic = await _context.Topics.Where(c => c.Id == TopicId).FirstOrDefaultAsync();

                var roomDetails = await _context.RoomDetails.Where(c => c.RoomId == topic.RoomId && c.UserId == user.Id).FirstOrDefaultAsync();

                if (roomDetails != null)
                {
                    Comment comment1 = new Comment()
                    {
                        CommentData = comment.CommentData,
                        Created = DateTime.Now,
                        TopicId = TopicId,
                        UserId = user.Id
                    };

                    await _context.Comments.AddAsync(comment1);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("TopicAndComments", "Topics", new { TopicId = TopicId });

            }

            return View(comment);
        }
    }
}
