
namespace Pin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using jsreport.AspNetCore;
    using jsreport.Types;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using Pin.Data;
    using Pin.Entitties;
    using Pin.Helpers;
    using Pin.Models;

    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string company, string year)
        {
            var filterdOrders = new List<Order>();

            if (company != null)
            {
                int id = int.Parse(company);
                filterdOrders = await _context.Orders
                    .Include(c => c.Company)
                    .Where(i => i.Company.Id == id)
                    .ToListAsync();
            }
            else
            {
                filterdOrders = await _context.Orders
                    .Include(c => c.Company)
                    .ToListAsync();
            }

            if (year != null)
            {
                int parsedYear = int.Parse(year);
                filterdOrders = filterdOrders
                    .Where(y => y.Date.Year == parsedYear)
                    .ToList();
            }


            var model = new OrderIndexVM()
            {
                Companies = await _context.Companies
                .Select(c =>

                      new SelectListItem()
                      {
                          Value = c.Id.ToString(),
                          Text = c.Name
                      }
                 )
                 .ToListAsync(),

                Years = _context.Orders.Select(y =>

              new SelectListItem()
              {
                  Value = y.Date.Year.ToString(),
                  Text = y.Date.Year.ToString()
              }).AsEnumerable().Distinct(new SelectListItemComparer()).ToList(),
                    
                Orders = filterdOrders
            };
            return View(model);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(c => c.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var vm = new OrderCreateVM
            {
                Companies = _context.Companies.Select(c =>

                new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }

            ).ToList()
            };

            return View(vm);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Receiver,Company,Date,Amount,Reason")] OrderCreateVM order)
        {
            var num = 0;

            if (_context.Orders.Where(c => c.Company.Id == int.Parse(order.Company)&& c.Date.Year==order.Date.Year).Any())
            {
                var a = _context.Orders.LastOrDefault(c => c.Company.Id == int.Parse(order.Company) && c.Date.Year == order.Date.Year).Number;
                num = _context.Orders.LastOrDefault(c => c.Company.Id == int.Parse(order.Company) && c.Date.Year == order.Date.Year).Number + 1;
            }

            if (num == 0)
            {
                num++;
            }

            var entity = new Order()
            {
                Number = num,
                Reason = order.Reason,
                Company = _context.Companies.FirstOrDefault(c => c.Id == int.Parse(order.Company)),
                Date = order.Date,
                Receiver = order.Receiver,
                Amount = order.Amount
            };

            if (ModelState.IsValid)
            {
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            order.Companies = _context.Companies.Select(c =>

                new SelectListItem()
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }

            ).ToList();

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Number,Receiver,Date,Amount,Reason")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }


        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> Print(int? id)
        {
        //    HttpContext.JsReportFeature().Recipe(Recipe.ChromePdf);
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [MiddlewareFilter(typeof(JsReportPipeline))]
        public async Task<IActionResult> DownloadPDF(int? id)
        {



            var order = await _context.Orders
                .Include(c=>c.Company)
             .FirstOrDefaultAsync(m => m.Id == id);

            HttpContext.JsReportFeature()
                
                .Recipe(Recipe.ChromePdf)
                .OnAfterRender(
                (r) => HttpContext.Response.Headers["Content-Disposition"] = "attachment; filename=\"myReport.pdf\"");

           
            return View("RKOPartial",order);
        }
    }
}
