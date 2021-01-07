using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.BillingPortal;  //for the customer portal
using CSC_AY2020_Task6_new.Database;
using Microsoft.Data.SqlClient;
using CSC_AY2020_Task6_new.Models;
using System.Net;

namespace CSC_AY2020_Task6_new.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : Controller
    {
        [HttpGet("ListPrice")]
        public IActionResult ListPrice()
        {

            StripeConfiguration.ApiKey = "";


            var options = new PriceListOptions
            {
                Active = true

            };
            var service = new PriceService();
            StripeList<Price> prices = service.List(
              options
            );

            var priceList = new List<object>();
            foreach (var e in prices.Data)
            {
                var productService = new ProductService();
                var product = productService.Get(e.ProductId);

                priceList.Add(new
                {
                    price_id = e.Id,
                    product_id = e.ProductId,
                    product_name = product.Name,
                    currency = e.Currency,
                    amt = e.UnitAmountDecimal / 100,
                    paymentInterval = e.Recurring.Interval
                });

            }

            if (priceList.Count != 0)
            {
                return Ok(priceList);
            }
            else
            {
                var msg = "No products found";
                return StatusCode(500, msg);
            }

        }

        [HttpPost]
        [Route("PurchasePlan")]
        public IActionResult PurchasePlan(PurchaseModel purchaseData)
        {
            Db db = new Db();
            if (db == null)
            {
                return StatusCode(500, "Database connection failed");
            }

            StripeConfiguration.ApiKey = "";
            string custId = purchaseData.CustomerId;
            string priceId = purchaseData.PriceId;
            string token = purchaseData.CardToken;


            var paymentOptions = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Token = token

                },
            };
            //card tokens are single use
            var paymentService = new PaymentMethodService();
            var paymentMethod = paymentService.Create(paymentOptions);

            string cmdString = "INSERT INTO paymentmethods (paymentmethod_id,cust_stripe_id) " +
                        "VALUES (@val1, @val2)";
            using (SqlCommand comm = new SqlCommand(cmdString, db.conn))
            {

                comm.Parameters.AddWithValue("@val1", paymentMethod.Id);
                comm.Parameters.AddWithValue("@val2", custId);

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
            var attachPaymentOptions = new PaymentMethodAttachOptions
            {
                Customer = custId,
            };
            var service = new PaymentMethodService();
            service.Attach(
              paymentMethod.Id,
              attachPaymentOptions
            );


            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = custId,
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Price = priceId
                    },
                },
                DefaultPaymentMethod = paymentMethod.Id
            };
            var subscriptionService = new SubscriptionService();
            var subscription = subscriptionService.Create(subscriptionOptions);

            cmdString = "INSERT INTO subscriptions (subscription_id,cust_stripe_id,price_id) " +
                        "VALUES (@val1, @val2, @val3)";
            using (SqlCommand comm = new SqlCommand(cmdString, db.conn))
            {

                comm.Parameters.AddWithValue("@val1", subscription.Id);
                comm.Parameters.AddWithValue("@val2", custId);
                comm.Parameters.AddWithValue("@val3", priceId);

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
            return Ok();
        }

        [HttpPost]
        [Route("AccessPortal")]
        public IActionResult AccessPortal(string custId)
        {

            StripeConfiguration.ApiKey = "";

            var options = new SessionCreateOptions
            {
                Customer = custId,
                ReturnUrl = "https://localhost:44345/Home/Charge",
            };
            var service = new SessionService();
            var session = service.Create(options);

            return Ok(session.Url);
        }
    }
}
