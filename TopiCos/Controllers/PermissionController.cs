using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TopiCos.Data;
using TopiCos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TopiCos.Controllers
{
    public class PermissionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PermissionController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> RemoveUsers(int RoomId)
        {
            var roomDetails = await _context.RoomDetails.Where(c=>c.RoomId==RoomId && c.MemberTypeId==3).Include(c=>c.User).ToListAsync(); 

            var data = new List<RemoveUserFromRoomViewModel>();

            ViewBag.hasData = false;

            if (roomDetails.Count != 0)
            {
                ViewBag.hasData = true;
            }

            foreach (var room in roomDetails)
            {
                RemoveUserFromRoomViewModel removeUserData = new RemoveUserFromRoomViewModel()
                {
                    IsSelected = false,
                    UserId = room.UserId,
                    Name = room.User.UserName
                };    

                data.Add(removeUserData);
                

            
            
            
            }

            ViewBag.RoomId = RoomId;    
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUsers(List<RemoveUserFromRoomViewModel> allData, int RoomId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var exists_Room = await _context.RoomDetails.Where(c=>c.RoomId==RoomId && c.MemberTypeId==1).FirstOrDefaultAsync(); 

            if (exists_Room != null && exists_Room.UserId==user.Id)
            {
                foreach (var data in allData)
                {
                    if(data.IsSelected)
                    {
                       var check_data = await _context.RoomDetails.Where(c => c.RoomId == RoomId && c.UserId == data.UserId).FirstOrDefaultAsync(); 
                       
                       _context.RoomDetails.Remove(check_data);
                       await _context.SaveChangesAsync();

                       var allTopics = await _context.Topics.Where(c=>c.RoomId==RoomId).ToListAsync();

                       foreach (var topic in allTopics)
                        {
                            var allComments = await _context.Comments.Where(c => c.TopicId == topic.Id && c.UserId == data.UserId).ToListAsync();

                            foreach (var comment in allComments)
                            {
                                _context.Comments.Remove(comment);
                                await _context.SaveChangesAsync();
                            }
                        }

                       
                    }
                }
            }

            return RedirectToAction("AllTopics", "Topics", new { RoomId = RoomId});
           
        }
    }
}
