using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Linq;
using TopiCos.Data;
using TopiCos.Models;

namespace TopiCos.Controllers
{
    [Authorize]
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<IdentityUser> _userManager;


        public TopicsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> AllTopics(int RoomId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var member = await _context.RoomDetails.Where(c => c.RoomId == RoomId && c.UserId==user.Id).FirstOrDefaultAsync();  

            if (member == null) 
            {
                return NotFound();
            }

            var roomData = await _context.Rooms.Where(c => c.Id==RoomId).FirstOrDefaultAsync();
            ViewBag.RoomName = roomData.Name;
            ViewBag.RoomDesc = roomData.Description;

            ViewBag.hasPer = false;


            if (member.MemberTypeId == 1 || member.MemberTypeId == 2)
            {
                ViewBag.hasPer = true;
                ViewBag.RoomId = RoomId;
            }

            ViewBag.hasRemovePer = false;

            if (member.MemberTypeId == 1)
            {
                ViewBag.hasRemovePer = true;
              
            }


            var data = await _context.Topics.Where(c=>c.RoomId==RoomId).Include(c => c.User).ToListAsync();

            return View(data);
        }

        public async Task<IActionResult> TopicAndComments(int TopicId)
        {
            var topic = await _context.Topics.Where(c => c.Id == TopicId).Include(c => c.User).FirstOrDefaultAsync();
            
            var all_comments = await _context.Comments.Where(c => c.TopicId == TopicId).Include(c => c.User).ToListAsync();

            dynamic mymodel = new ExpandoObject();

            

            mymodel.Topic = topic;
            mymodel.Comments = all_comments;

            return View(mymodel);
        }

        public async Task<IActionResult> CreateTopic(int RoomId)
        {
           
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var roomData = await _context.RoomDetails.Where(c => c.RoomId == RoomId && c.UserId == user.Id).FirstOrDefaultAsync();

                if (roomData == null)
                {
                    return NotFound();
                }

                if (roomData.MemberTypeId == 3)
                {
                    return NotFound(); //"No permission";
                }

                ViewBag.UserId = user.Id;
                ViewBag.RoomId = RoomId;


            return View();



        }

        [HttpPost]

        public async Task<IActionResult> CreateTopic(Topic topic)
        {

            if (ModelState.IsValid)
            {
                var data = await _context.RoomDetails.Where(c => c.RoomId == topic.RoomId &&
            c.UserId == topic.UserId).FirstOrDefaultAsync();

                if (data == null)
                {
                    return NotFound();
                }

                if (data.MemberTypeId == 3)
                {
                    return NotFound(); //"No permission"
                }

                Topic topic1 = new Topic()
                {
                    Title = topic.Title,
                    Description = topic.Description,
                    Created = DateTime.Now,
                    UserId = topic.UserId,
                    RoomId = topic.RoomId
                };

                await _context.Topics.AddAsync(topic1);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(AllTopics), new { RoomId = topic.RoomId });
            }

            return View(topic);
            

        }



    }
}
