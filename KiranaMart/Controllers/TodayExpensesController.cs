using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KiranaMart.Models;

namespace KiranaMart.Controllers
{
    public class TodayExpensesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TodayExpenses
        public async Task<ActionResult> Index()
        {
            return View(await db.TodayExpenses.ToListAsync());
        }

        // GET: TodayExpenses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodayExpenses todayExpenses = await db.TodayExpenses.FindAsync(id);
            if (todayExpenses == null)
            {
                return HttpNotFound();
            }
            return View(todayExpenses);
        }

        // GET: TodayExpenses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TodayExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TodayExpence,Details,Date")] TodayExpenses todayExpenses)
        {
            if (ModelState.IsValid)
            {
                db.TodayExpenses.Add(todayExpenses);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(todayExpenses);
        }

        // GET: TodayExpenses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodayExpenses todayExpenses = await db.TodayExpenses.FindAsync(id);
            if (todayExpenses == null)
            {
                return HttpNotFound();
            }
            return View(todayExpenses);
        }

        // POST: TodayExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TodayExpence,Details,Date")] TodayExpenses todayExpenses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(todayExpenses).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(todayExpenses);
        }

        // GET: TodayExpenses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TodayExpenses todayExpenses = await db.TodayExpenses.FindAsync(id);
            if (todayExpenses == null)
            {
                return HttpNotFound();
            }
            return View(todayExpenses);
        }

        // POST: TodayExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TodayExpenses todayExpenses = await db.TodayExpenses.FindAsync(id);
            db.TodayExpenses.Remove(todayExpenses);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
