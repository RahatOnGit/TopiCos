using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TopiCos.Data;
using TopiCos.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace TopiCos.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public RoomController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult CreateRoom()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> CreateRoom(Room model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var aRoom = await _context.Rooms.Where(c => c.Name == model.Name && c.UserId == user.Id).FirstOrDefaultAsync();

                if (aRoom != null)
                {
                    ViewBag.Mess = ".. A room exists using the same name";
                    return View(model);
                }

                string roomid = user.UserName;
                string guid = System.Guid.NewGuid().ToString();

                roomid += guid;

                string current_time = DateTime.Now.ToString();

                roomid += ("-a14P;-" + current_time);

                Room data = new Room()
                {
                    Name = model.Name,
                    Description = model.Description,
                    RoomId = roomid,
                    Created = DateTime.Now,
                    UserId = user.Id
                };

                await _context.Rooms.AddAsync(data);
                await _context.SaveChangesAsync();

                RoomDetails roomdetails = new RoomDetails()
                {
                    RoomId = data.Id,
                    UserId = user.Id,
                    MemberTypeId = 1

                };

                await _context.RoomDetails.AddAsync(roomdetails);
                await _context.SaveChangesAsync();

                return RedirectToAction("AllRooms");
            }

            return View(model);

               
          
        }

        public IActionResult JoinRoom()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> JoinRoom(string RoomId)
        {
            var room = await _context.Rooms.Where(c=>c.RoomId==RoomId).FirstOrDefaultAsync();   

            if (room == null)
            {
                ViewBag.Mess = "No room exists,!";

                return View();
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userInRoom = await _context.RoomDetails.Where(c=>c.UserId==user.Id && c.RoomId== room.Id).FirstOrDefaultAsync();
            if (userInRoom != null)
            {
                ViewBag.Mess = "User already in the room!";

                return View();
            }

            RoomDetails roomDetails = new RoomDetails()
            {
                RoomId = room.Id,
                UserId = user.Id,
                MemberTypeId = 3
            };

            await _context.RoomDetails.AddAsync(roomDetails);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllRooms");


        }

        public async Task<IActionResult> AllRooms()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var allRoomsDetails = await _context.RoomDetails.Where(c => c.UserId == user.Id).ToListAsync();

            List<Room> data = new List<Room>();
            foreach (var room in allRoomsDetails)
            {


                var aRoom = await _context.Rooms.Where(c => c.Id == room.RoomId).FirstOrDefaultAsync();
                data.Add(aRoom);
            }

            return View(data);
        }
        
        public async Task<IActionResult> ShareRoomID(int RoomId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var roomDetails = await _context.RoomDetails.Where(c => c.RoomId == RoomId && c.UserId==user.Id).FirstOrDefaultAsync();

            if (roomDetails == null || roomDetails.MemberTypeId!=1)
            {
                return View("AllRooms");
            }

            var roomData = await _context.Rooms.Where(c => c.Id == RoomId).FirstOrDefaultAsync();

            return View(roomData);



        }



    }
}
