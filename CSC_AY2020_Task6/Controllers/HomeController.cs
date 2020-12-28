using CSC_AY2020_Task6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Stripe;

namespace CSC_AY2020_Task6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Charge()
        {
            ViewBag.Message = "Learn how to process payments with Stripe";
            return View(new RegistrationModel());
        }

        public ActionResult PlanSelect(string customerId)
        {
            ViewBag.Message = customerId;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Charge(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var customer = await RegisterCustomer(model);
            var cID = customer.Id;
            return RedirectToAction("PlanSelect", "Home", new { customerId = cID }); ;
        }



        private static async Task<Stripe.Customer> RegisterCustomer(RegistrationModel model)
        {
            return await Task.Run(() =>
            {
                StripeConfiguration.ApiKey = "sk_test_51I2bbHBYTNT0yLeeR90ErQBVJPbr5wYhg9NySyKsKb3HpFJ64fiTvVAsSYQpq53Vx4yk180e4uJbLMZ1F7AwhUTD00bbyfNUiO";


                var custOptions = new CustomerCreateOptions
                {
                    
                    Name = model.Name,
                    Phone = model.Phone,
                    Email = model.Email,
                    Address = new AddressOptions()
                    {
                        Line1 = model.AddressLine1,
                        Line2 = model.AddressLine2,
                        PostalCode = model.AddressPostcode,
                        City = model.AddressCity,
                        Country = model.AddressCountry
                    }

                };

                var service = new CustomerService();
                var account = service.Create(custOptions);

                return account;
            });
        }
    }
}
