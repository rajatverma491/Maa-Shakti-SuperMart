using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using KiranaMart.Models;
using KiranaMart.VIewModels;

namespace KiranaMart.Controllers
{
    public class BillingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Billings
        public async Task<ActionResult> Index()
        {
            var billings = db.Billings.Include(b => b.Customer);
            return View(await billings.ToListAsync());
        }

        // GET: Billings/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billing billing = await db.Billings.FindAsync(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            return View(billing);
        }
        //public async Task<ActionResult> Edit(int? id)
        //{
           
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Billing billing = await db.Billings.FindAsync(id);
        //    int pl = 0;
        //    int tpl = 0;
        //    foreach (var item in db.SellingHistories.ToList())
        //    {
        //        foreach (var item3 in db.Billings.ToList())
        //        {
        //            if (item.BillingId == item3.Id)
        //            {

        //                foreach (var item1 in db.Products.ToList())
        //                {
        //                    if (item1.Id == item.ProductId)
        //                    {
        //                        pl = item.Quantity * (item1.Price - item1.CostPrice);
        //                        tpl = tpl + pl;
        //                    }
        //                }

        //            }
        //        }
        //    }

        //    billing.AmountDue = tpl - billing.AmountPaid;
        //    if (billing == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", billing.CustomerId);
        //    return View(billing);
        //}

        //// POST: Billings/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(Billing billing)
        //{
        //     if (ModelState.IsValid)
        //    {
        //        db.Entry(billing).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", billing.CustomerId);
        //    return View(billing);
        //}

        public ActionResult ExcahngeProduct(string error)
        {
            ViewBag.Error = error;
            return View();
        }

        public ActionResult ExchangeView(FormCollection form)
        {
            try
            {
                Billing bill = null;
                string billID = Request.Form["BillId"];
                int id = Convert.ToInt32(billID);
                bool isBillExit = false;
                foreach (var item in db.Billings.ToList())
                {
                    if (item.Id == id)
                    {
                        isBillExit = true;
                        break;
                    }

                }

                if (isBillExit == true)
                {
                    bill = db.Billings.Find(id);
                }
                else
                {
                    return RedirectToAction("ExcahngeProduct", new { error = "Invalid Bill Number" });
                }





                AllClassViewModel viewModel = new AllClassViewModel
                {
                    Billing = bill,
                    Products = db.Products.ToList(),
                    SellingHistories = db.SellingHistories.Where(m => m.BillingId == bill.Id)

                };

                return View(viewModel);
            }
            catch(Exception e)
            {
                
                return RedirectToAction("ExcahngeProduct",new { error = "Invalid Bill Number" });
            }
        }
        
        public ActionResult RemoveTemp(int? id)
        {
            SellingHistory sellingHistory = db.SellingHistories.Find(id);
            Billing bill = db.Billings.Find(sellingHistory.BillingId);
            Product product = db.Products.Find(sellingHistory.ProductId);
            product.Quantity = product.Quantity + sellingHistory.Quantity;
            db.SellingHistories.Remove(sellingHistory);
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            AllClassViewModel viewModel = new AllClassViewModel
            {
                Billing = bill,
                Products = db.Products.ToList(),
                SellingHistories = db.SellingHistories.Where(m => m.BillingId == bill.Id)

            };
            return View("ExchangeView", viewModel);
        }

        //public ActionResult AddItem(int? id)
        //{
        //    TempProduct temp = db.TempProducts.Find(id);
        //    if (temp.Quantity >= 1)
        //    {
        //        temp.QtyPurchased = temp.QtyPurchased + 1;
        //        temp.Quantity = temp.Quantity - 1;
        //        db.Entry(temp).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Insufficient Quntity";
        //    }

        //    AllClassViewModel viewModel = new AllClassViewModel
        //    {
        //        TempProducts = db.TempProducts.ToList()
        //    };
           
        //    return View("ExchangeView", viewModel);
        //}

        public ActionResult RemItem(int? id)
        {
            SellingHistory sellingHistory = db.SellingHistories.Find(id);
            Billing bill = db.Billings.Find(sellingHistory.BillingId);
            Product product = db.Products.Find(sellingHistory.ProductId);
            if (sellingHistory.Quantity <= 1)
            {
                sellingHistory.Quantity = 1;
                AllClassViewModel viewModel1 = new AllClassViewModel
                {
                    Billing = bill,
                    Products = db.Products.ToList(),
                    SellingHistories = db.SellingHistories.Where(m => m.BillingId == bill.Id)

                };
                return View("ExchangeView", viewModel1);
            }
            product.Quantity = product.Quantity + 1;
            sellingHistory.Quantity = sellingHistory.Quantity - 1;
            db.Entry(sellingHistory).State = EntityState.Modified;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            AllClassViewModel viewModel = new AllClassViewModel
            {
                Billing = bill,
                Products = db.Products.ToList(),
                SellingHistories = db.SellingHistories.Where(m => m.BillingId == bill.Id)

            };
            return View("ExchangeView", viewModel);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ConfirmBillExchange(int? id)
        {
            TempData.Keep();
            Billing bill = db.Billings.Find(id);
            return View(bill);
        }

        public ActionResult ExchangeBillPrint(Billing bill)
        {
            TempData.Keep();
            db.Entry(bill).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("FinalBillPrint","Products",bill);
        }
        [Authorize]
        public ActionResult StarCustomer()
        { 

            AllClassViewModel allClassViewModel = new AllClassViewModel
            {
                Billings=db.Billings.ToList(),
                Customers=db.Customers.ToList()
            };
            
            return View(allClassViewModel);
        }

        public ActionResult ViewProducts(int? id)
        {
            Billing billing = db.Billings.Find(id);
            AllClassViewModel viewModel = new AllClassViewModel()
            {
                SellingHistories = db.SellingHistories.Where(m => m.BillingId == id),
                Products = db.Products.ToList()
            };
            return View(viewModel);
        }
        [Authorize]
        public ActionResult Galla()
        {

            return View();
        }
        [Authorize]
        public ActionResult GallaDekho(FormCollection form)
        {
            GallaViewModel gallaViewModel = new GallaViewModel();
            List<GallaViewModel> viewModels = new List<GallaViewModel>();
            string start = Request.Form["StartDate"];
            string end = Request.Form["EndDate"];
            DateTime sd = DateTime.ParseExact(start, "dd/MM/yyyy", null);
            DateTime ed = DateTime.ParseExact(end, "dd/MM/yyyy", null);
            double tpl = 0;
            double total = 0;
            for (DateTime dateA = sd; dateA <= ed; dateA = dateA.AddDays(1))
            {
                foreach (var item in db.Billings.ToList())
                {
                    if (item.Date == dateA)
                    {
                        tpl = tpl + item.Total;
                    }

                }
                gallaViewModel = new GallaViewModel();
                gallaViewModel.BillDate = dateA;
                gallaViewModel.TotalGalla = tpl;
                viewModels.Add(gallaViewModel);
                total = total + tpl;
                tpl = 0;
            }

            ViewBag.total = total;
            return View(viewModels.ToList());
        }
    }
}
