using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using KiranaMart.Models;
using KiranaMart.VIewModels;

namespace KiranaMart.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            return View(await db.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            bool flag = false;
            Random random = new Random();
            long barCodeNo = random.Next(10000, 100000000);
            foreach (var item in db.Products.ToList())
            {
                if (item.BarCodeId == barCodeNo.ToString())
                {
                    flag = true;
                    break;
                }
            }
            if (flag == false)
            {
                product.BarCodeId = barCodeNo.ToString();
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                random = new Random();
                barCodeNo = random.Next(10000, 100000000);
                product.BarCodeId = barCodeNo.ToString();
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }

            return View(product);
        }

        public ActionResult CreatewithBarcode()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreatewithBarcode(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);

        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductName,BarCodeId,Unit,Price,CostPrice,ExpiryDate,Quantity,Brand,MRP")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
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
        public ActionResult PrintBarCode(Product product)
        {
            string barcode = product.BarCodeId;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap bitMap = new Bitmap(barcode.Length * 40, 100))
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        Font oFont = new Font("IDAutomationHC39M", 16);
                        Font bFont = new Font(FontFamily.GenericSerif, 8);
                        PointF point = new PointF(2f, 2f);
                        //PointF point3 = new PointF(45f, 0.5f);
                        PointF point2 = new PointF(56f, 76f);
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        //graphics.DrawString("V Men's Wear", bFont, blackBrush, point3);
                        graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                        graphics.DrawString("Maa Shkati Super Mart Price:   " + product.Price.ToString(), bFont, blackBrush, point2);

                    }

                    bitMap.Save(memoryStream, ImageFormat.Jpeg);

                    ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            return View();
        }

        public ActionResult BillGenerator()
        {
            return View();
        }

        public ActionResult GenerateBill(TempProduct tempProduct)
        {
            string barcode = tempProduct.BarCodeId.ToString();
            IEnumerable<Product> products = db.Products.ToList();
            bool flag = false;
            foreach (var item in products)
            {
                if (item.BarCodeId.Equals(barcode))
                {
                    flag = true;
                    tempProduct.BarCodeId = item.BarCodeId;
                    tempProduct.Brand = item.Brand;
                    tempProduct.ProductName = item.ProductName;
                    tempProduct.Unit = item.Unit;
                    tempProduct.Price = item.Price;
                    tempProduct.Quantity = item.Quantity;
                    tempProduct.ExpiryDate = item.ExpiryDate;
                    tempProduct.QtyPurchased = 0;
                }
            }
            if (flag)
            {
                bool fl = false;
                foreach (var item in db.TempProducts.ToList())
                {
                    if (tempProduct.BarCodeId == item.BarCodeId)
                    {

                        item.QtyPurchased = item.QtyPurchased + 1;
                        item.Quantity = item.Quantity - 1;
                        db.Entry(item).State = EntityState.Modified;
                        fl = true;
                        if (item.Quantity >= 1)
                        {
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.Message = "Insufficient Quantity";
                        }
                        break;
                    }
                }
                if (!fl)
                {
                    if (tempProduct.Quantity >= 1)
                    {
                        tempProduct.Quantity = tempProduct.Quantity - 1;
                        tempProduct.QtyPurchased = tempProduct.QtyPurchased + 1;
                        db.TempProducts.Add(tempProduct);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "Insufficient Quantity";
                    }

                }


            }
            else
            {
                ViewBag.Message = "Incorrect Product";
            }
            AllClassViewModel viewModel = new AllClassViewModel
            {
                TempProducts = db.TempProducts.ToList()
            };
            ModelState.Clear();
            return View(viewModel);
        }


        public ActionResult RemoveTemp(int? id)
        {
            TempProduct temp = db.TempProducts.Find(id);
            db.TempProducts.Remove(temp);
            db.SaveChanges();
            AllClassViewModel viewModel = new AllClassViewModel
            {
                TempProducts = db.TempProducts.ToList()
            };
            ModelState.Clear();
            return View("GenerateBill", viewModel);
        }

        public ActionResult AddItem(int? id)
        {
            TempProduct temp = db.TempProducts.Find(id);
            if (temp.Quantity >= 1)
            {
                temp.QtyPurchased = temp.QtyPurchased + 1;
                temp.Quantity = temp.Quantity - 1;
                db.Entry(temp).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                ViewBag.Message = "Insufficient Quntity";
            }

            AllClassViewModel viewModel = new AllClassViewModel
            {
                TempProducts = db.TempProducts.ToList()
            };
            ModelState.Clear();
            return View("GenerateBill", viewModel);
        }

        public ActionResult RemItem(int? id)
        {
            TempProduct temp = db.TempProducts.Find(id);
            temp.QtyPurchased = temp.QtyPurchased - 1;
            temp.Quantity = temp.Quantity + 1;
            if (temp.QtyPurchased <= 1)
            {
                temp.QtyPurchased = 1;
            }
            db.Entry(temp).State = EntityState.Modified;
            db.SaveChanges();
            AllClassViewModel viewModel = new AllClassViewModel
            {
                TempProducts = db.TempProducts.ToList()
            };
            ModelState.Clear();
            return View("GenerateBill", viewModel);
        }

        public ActionResult ConfirmBill()
        {
            double totalAmount = 0;
            foreach (var item in db.TempProducts.ToList())
            {
                totalAmount = totalAmount + (item.Price) * (item.QtyPurchased);
            }
            ViewBag.TotalAmount = totalAmount;
            return View();
        }

        public ActionResult PrintBill(AllClassViewModel allClass)
        {

            bool flag = false;
            int id = 0;
            foreach (var item in db.Customers.ToList())
            {
                if (allClass.Customer.PhoneNo.Equals(item.PhoneNo))
                {
                    flag = true;
                    id = item.Id;
                    break;
                }
            }

            if (!flag)
            {
                Customer customer = new Customer()
                {
                    Address = allClass.Customer.Address,
                    Name = allClass.Customer.Name,
                    PhoneNo = allClass.Customer.PhoneNo
                };
                db.Customers.Add(customer);
                db.SaveChanges();
                foreach (var item in db.Customers.ToList())
                {
                    if (item.PhoneNo == allClass.Customer.PhoneNo)
                    {
                        id = item.Id;
                        break;
                    }
                }
            }

            double totalAmount = 0;
            foreach (var item in db.TempProducts.ToList())
            {
                totalAmount = totalAmount + (item.Price) * (item.QtyPurchased);
            }

            Billing bill = new Billing()
            {
                CustomerId = id,
                AmountPaid = allClass.Billing.AmountPaid,
                Date = allClass.Billing.Date,
                DueDate = allClass.Billing.DueDate,
                AmountDue = totalAmount - allClass.Billing.AmountPaid,
                Total = totalAmount - (totalAmount * allClass.Discount) / 100

                //enter amount due here
            };
            db.Billings.Add(bill);
            db.SaveChanges();
            var lastbills = db.Billings.ToList();
            Billing lastbill = lastbills.Last();
            int lastId = lastbill.Id;

            SellingHistory history = new SellingHistory() { };

            foreach (var item in db.TempProducts.ToList())
            {
                foreach (var item1 in db.Products.ToList())
                {
                    if (item1.BarCodeId == item.BarCodeId)
                    {
                        history.BillingId = lastId;
                        history.ProductId = item1.Id;
                        history.Quantity = item.QtyPurchased;
                        history.Margin = item1.Price - item1.CostPrice;
                        history.SellingPrice = item1.Price;
                        history.CostPrice = item1.CostPrice;
                        history.MRP = item1.MRP;
                        db.SellingHistories.Add(history);
                        item1.Quantity = item1.Quantity - item.QtyPurchased;
                        db.Entry(item1).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            foreach (var item in db.TempProducts.ToList())
            {
                db.TempProducts.Remove(item);
                db.SaveChanges();
            }

            return RedirectToAction("FinalBillPrint", lastbill);
        }

        public ActionResult FinalBillPrint(Billing bill)
        {
            string barcode = bill.Id.ToString();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap bitMap = new Bitmap(barcode.Length * 40, 100))
                {
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        Font oFont = new Font("IDAutomationHC39M", 16);
                        Font bFont = new Font(FontFamily.GenericSerif, 8);
                        PointF point = new PointF(2f, 2f);
                        //PointF point3 = new PointF(45f, 0.5f);
                        PointF point2 = new PointF(56f, 76f);
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);

                    }

                    bitMap.Save(memoryStream, ImageFormat.Jpeg);

                    ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                }
            }



            //Billing bill = db.Billings.Find(id);
            Customer customer = new Customer() { };
            foreach (var item in db.Customers.ToList())
            {
                if (item.Id == bill.CustomerId)
                {
                    customer = item;
                    break;
                }
            }
            List<SellingHistory> sellingHistories = new List<SellingHistory>();
            foreach (var item in db.SellingHistories.ToList())
            {
                if (item.BillingId == bill.Id)
                {
                    sellingHistories.Add(item);
                }
            }
            List<Product> products = new List<Product>();
            double sum = 0;
            foreach (var item in db.Products.ToList())
            {
                foreach (var item1 in sellingHistories)
                {
                    if (item.Id == item1.ProductId)
                    {
                        products.Add(item);
                        sum = sum + (item1.Quantity) * (item.Price);
                    }
                }
            }
            ViewBag.TotalPrice = sum;
            ViewBag.AmountDue = sum - bill.AmountPaid;

            AllClassViewModel viewModel = new AllClassViewModel()
            {
                Customer = customer,
                Billing = bill,
                Products = products,
                SellingHistories = sellingHistories
            };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult ProfitandLoss()
        {
            return View();
        }

        [Authorize]
        public ActionResult BaiKhata(FormCollection form)
        {
            string start = Request.Form["StartDate"];
            string end = Request.Form["EndDate"];
            DateTime sd = DateTime.ParseExact(start, "dd/MM/yyyy", null);
            DateTime ed = DateTime.ParseExact(end, "dd/MM/yyyy", null);
            ProfitLossViewModel profitLoss = new ProfitLossViewModel();
            List<ProfitLossViewModel> viewModels = new List<ProfitLossViewModel>();
           
            double tpl = 0;
            double total = 0;
            int expense = 0;
            int totalexpense = 0;
            for (DateTime dateA = sd; dateA <= ed; dateA = dateA.AddDays(1))
            {
                foreach (var item1 in db.Billings.ToList())
                {
                    foreach (var item in db.SellingHistories.ToList())
                    {
                        if (item1.Id == item.BillingId)
                        {
                            if (item1.Date == dateA)
                            {
                                tpl = tpl + item.Margin;
                            }
                        }

                    }


                }
                profitLoss = new ProfitLossViewModel();
                profitLoss.BillDate = dateA;
                profitLoss.TotalProfit = tpl;
                totalexpense = totalexpense + expense;
                viewModels.Add(profitLoss);
                total = total + tpl;
                tpl = 0;
             
                expense = 0;
            }
            ViewBag.total = total;
            return View(viewModels.ToList());
        }
        public ActionResult ExpiryView()
        {
            return View(db.Products.ToList());
        }

        public ActionResult TotalAmountDueOfClient()
        {
            var customers = db.Customers.ToList();
            var billings = db.Billings.ToList();

            AllClassViewModel viewModel = new AllClassViewModel
            {
                Customers = customers,
                Billings = billings
            };
            return View(viewModel);
        }

        public ActionResult ViewDueBills(int? id)
        {
            Customer customer = db.Customers.Find(id);
            IList<Billing> bills = new List<Billing>();
            foreach (var item in db.Billings.ToList())
            {
                if (item.CustomerId == customer.Id && item.AmountDue > 0)
                {
                    bills.Add(item);
                }
            }
            AllClassViewModel viewModel = new AllClassViewModel
            {
                Customer = customer,
                Billings = bills
            };
            return View(viewModel);
        }

        public ActionResult ClearBill(int? id)
        {
            Billing bill = db.Billings.Find(id);
            bill.AmountPaid = bill.AmountPaid + bill.AmountDue;
            bill.AmountDue = 0;
            db.Entry(bill).State = EntityState.Modified;
            db.SaveChanges();
            Customer customer = db.Customers.Find(bill.CustomerId);
            return RedirectToAction("ViewDueBills", new { customer.Id });
        }


        public JsonResult GetSearchValue(string Prefix)
        {
            //Note : you can bind same list from database  
            var customer = db.Customers.ToList();
            //Searching records from list using LINQ query  
            var CityList = (from N in customer
                            where N.PhoneNo.StartsWith(Prefix)
                            select new { N.PhoneNo });
            return Json(CityList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddtoBill(FormCollection form)
        {
            string qty = Request.Form["Quantity"];
            string unit = Request.Form["Unit"];
            string id = Request.Form["id"];
            int idd = Convert.ToInt32(id);
            double qtyy = Convert.ToDouble(qty);
            Product item = db.Products.Find(idd);
            bool flag = false;
            int bar = 0;
            TempData.Keep();
          
            foreach (var item1 in db.TempProducts.ToList())
            {
               
                if (item.BarCodeId == item1.BarCodeId)
                {
                    flag = true;
                    bar = item1.Id;
                    break;
                }
            }
            if (!flag)
            {
                if (qtyy<=item.Quantity && item.Quantity > 0 && qtyy>0)
                {
                    TempProduct tempProduct = new TempProduct()
                    {
                        BarCodeId = item.BarCodeId,
                        Brand = item.Brand,
                        ProductName = item.ProductName,
                        Unit = unit,
                        Price = item.Price,
                        Quantity = item.Quantity - qtyy,
                        ExpiryDate = item.ExpiryDate,
                        QtyPurchased = qtyy
                    };
                    db.TempProducts.Add(tempProduct);
                    db.SaveChanges();
                }
                else
                {
                    TempData["msg"] = "Insufficient Quantity or Wrong Input";
                }

            }
            else
            {
                if (qtyy <= item.Quantity && item.Quantity > 0 && qtyy>0)
                {
                    TempProduct tp = db.TempProducts.Find(bar);
                    tp.QtyPurchased = tp.QtyPurchased + qtyy;
                    tp.Quantity = tp.Quantity - qtyy;
                    if (ModelState.IsValid)
                    {
                        db.Entry(tp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    TempData["msg"] = "Insufficient Quantity or Wrong Input";
                }

            }




                //GenerateBill(tempProduct);
                return RedirectToAction("KhulaBill");
                //return RedirectToAction("GenerateBill", tempProduct);

            }

            public ActionResult ClearTemp()
            {
                foreach (var item in db.TempProducts.ToList())
                {
                    db.TempProducts.Remove(item);
                    db.SaveChanges();
                }

                return RedirectToAction("MakeBill");
            }

            public ActionResult MakeBill()
            {
                AllClassViewModel viewModel = new AllClassViewModel
                {
                    TempProducts = db.TempProducts.ToList()
                };
                ModelState.Clear();
                return View(viewModel);
            }

            public ActionResult KhulaBill()
            {
               
            AllClassViewModel allClassViewModel = new AllClassViewModel()
            {
                TempProducts = db.TempProducts.ToList(),
                Products=db.Products.ToList()
            };
                return View(allClassViewModel);
            }
            [Authorize]
            public ActionResult MProduct()
            {
                return View(db.Products.ToList().Where(m=>m.CostPrice==0));
            }





         }

    }


 

