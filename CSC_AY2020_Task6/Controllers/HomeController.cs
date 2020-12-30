using CSC_AY2020_Task6.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Stripe;
using CSC_AY2020_Task6.Database;
using Microsoft.Data.SqlClient;

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

            Db db = new Db();
            if (db == null)
            {
                return new HttpStatusCodeResult(500, "Database connection failed");
            }
            var customer = await RegisterCustomer(model, db);
            var cID = customer.Id;
            return RedirectToAction("PlanSelect", "Home", new { customerId = cID }); ;
        }



        private static async Task<Stripe.Customer> RegisterCustomer(RegistrationModel model, Db db)
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

                string cmdString = "INSERT INTO customer (cust_stripe_id,cust_name,phone,email,addressLine1,addressLine2,city,postcode,country) " +
                        "VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9)";
                using (SqlCommand comm = new SqlCommand(cmdString, db.conn))
                {

                    comm.Parameters.AddWithValue("@val1", account.Id);
                    comm.Parameters.AddWithValue("@val2", model.Name);
                    comm.Parameters.AddWithValue("@val3", model.Phone);
                    comm.Parameters.AddWithValue("@val4", model.Email);
                    comm.Parameters.AddWithValue("@val5", model.AddressLine1);
                    comm.Parameters.AddWithValue("@val6", model.AddressLine2);
                    comm.Parameters.AddWithValue("@val7", model.AddressCity);
                    comm.Parameters.AddWithValue("@val8", model.AddressPostcode);
                    comm.Parameters.AddWithValue("@val9", model.AddressCountry);
                    //try
                    //{
                        db.conn.Open();
                        comm.ExecuteNonQuery();
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //    return null;
                    //}
                }
                db.conn.Close();
                return account;
            });
        }
    }
}
