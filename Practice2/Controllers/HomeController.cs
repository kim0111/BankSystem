using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice2.Data;
using Practice2.Models;
using System;
using System.Diagnostics;

namespace Practice2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BankContext _dbContext;

        public HomeController(ILogger<HomeController> logger, BankContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /*        [HttpGet]
                public async Task<IActionResult> ReadDeposit()
                {
                    List<Bank> person = (from m in _dbContext.Banks select m).ToList();

                    return View(person);
                }*/

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _dbContext.Banks.ToListAsync();

            return View(response);
        }


        [HttpPost]
        public async Task<IActionResult> Post([Bind("OnAccount,inComing,Out,DateRegis")] Bank model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbContext.Banks.Add(model);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Get");
                }
            }

            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " + "Try again or call system admin");
            }

            return View(model);

        }


        public async Task<IActionResult> Post()
        {

            return View();
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPostDepo(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var depo = await _dbContext.Banks.FirstOrDefaultAsync(s => s.Id == Id);


            if (await TryUpdateModelAsync<Bank>(
                depo, "", s => s.OnAccount, s => s.inComing, s => s.Out, s => s.DateRegis))
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again or call system admin");
                }
            }

            return View(depo);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var person = await _dbContext.Banks.FirstOrDefaultAsync(m => m.Id == Id);

            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        
/*         public int money(int m,string name)
         {
             int total=m;

            if (name == "USD")
            {
                total *= 470;
            }else if (name == "EUR")
            {
                total *= 460;
            }
            else
            {
                total *= 64;
            }

            return total;
         }*/




        public async Task<IActionResult> DeleteDeposit(int? id, bool? Savechangeserror = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depo = await _dbContext.Banks.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            if (depo == null)
            {
                return NotFound();
            }

            if (Savechangeserror.GetValueOrDefault())
            {
                ViewData["DeleteError"] = "Delete failed, please try again later ... ";
            }

            return View(depo);
        }


        [HttpPost, ActionName("DeleteDeposit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeleteDeposit(int id)
        {
            var depo = await _dbContext.Banks.FindAsync(id);

            if (depo == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _dbContext.Banks.Remove(depo);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(DeleteDeposit), new { id = id, Savechangeserror = true });
            }

        }




    }
}