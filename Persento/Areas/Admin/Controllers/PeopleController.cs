using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persento.DAL;
using Persento.Models;
using Persento.Utilities;
using Persento.ViewModel.People;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Persento.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PeopleController : Controller
    {
        private AppDbContext _context;
        private IWebHostEnvironment _env;

        public PeopleController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.People.Where(p=>p.IsDeleted==false).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM create)
        {
            if (!ModelState.IsValid) return View();
            bool isExist = await _context.People.AnyAsync(p => p.Fullname.Trim().ToLower() == create.Fullname.Trim().ToLower());
            if (isExist)
            {
                ModelState.AddModelError("Fullname", $"{create.Fullname} artiq movcuddur.");
                return View(create);
            }
            if (create.Photo.Length/1024>200)
            {
                ModelState.AddModelError("Photo", $"Sekilin olcusu {200}kb-dan boyukdur");
                return View(create);
            }
            if (!create.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Sekilin tipi duzgun deyil");
                return View(create);
            }
            string fileName = Guid.NewGuid().ToString() + "_" + create.Photo.FileName;
            string rootPath = Path.Combine(_env.WebRootPath, "assets/img", fileName);
            using(FileStream file=new FileStream(rootPath, FileMode.Create))
            {
                await create.Photo.CopyToAsync(file);
            }
            Person person = new Person
            {
                Fullname=create.Fullname,
                Position=create.Position,
                Content=create.Content,
                Image=fileName
            };
            await _context.People.AddAsync(person);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "People");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Person dbPerson = await _context.People.Where(p => p.Id == id && p.IsDeleted == false)
                                                                                        .FirstOrDefaultAsync();
            if (dbPerson == null) return BadRequest();
            dbPerson.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            Person person = _context.People.Find(id);
            if (person == null) return BadRequest();
            return View(person);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Person update,int id)
        {
            Person dbPerson = await _context.People.Where(p => p.IsDeleted == false && p.Id == id).FirstOrDefaultAsync();

            if (dbPerson == null) return NotFound();
            if (!ModelState.IsValid) return View(update);
            bool isExist = await _context.People.AnyAsync(p => p.Fullname.Trim().ToLower() == update.Fullname.Trim().ToLower());
            bool isExistContent = dbPerson.Content.Trim().ToLower() == update.Content.Trim().ToLower();
            bool isExistPosition = dbPerson.Position.Trim().ToLower() == update.Position.Trim().ToLower();
            if (!isExist)
            {
                dbPerson.Fullname = update.Fullname;
            }
            if (!isExistContent)
            {
                dbPerson.Content = update.Content;
            }
            if (!isExistPosition)
            {
                dbPerson.Position = update.Position;
            }
            if (update.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Photo", $"Sekilin olcusu {200}kb-dan boyukdur");
                return View(update);
            }
            if (!update.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "Sekilin tipi duzgun deyil");
                return View(update);
            }
            Helper.RemoveFile(_env.WebRootPath, "assets/img", dbPerson.Image);
            string fileName = Guid.NewGuid().ToString() + "_" + update.Photo.FileName;
            string rootPath = Path.Combine(_env.WebRootPath, "assets/img", fileName);
            using (FileStream file = new FileStream(rootPath, FileMode.Create))
            {
                await update.Photo.CopyToAsync(file);
            }
            dbPerson.Image = fileName;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "People");
        }
    }
}
