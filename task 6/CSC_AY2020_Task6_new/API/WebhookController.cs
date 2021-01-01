using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Stripe;
using System.IO;
using Microsoft.Data.SqlClient;
using CSC_AY2020_Task6_new.Database;

namespace CSC_AY2020_Task6_new.API
{
    [Route("webhook")]
    [ApiController]
    public class WebhookController : Controller
    {
        [HttpPost]

        public async Task<IActionResult> Index()

        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            

            var stripeEvent = EventUtility.ParseEvent(json);
            Db db = new Db();

            if (stripeEvent.Type == Events.PaymentIntentSucceeded || stripeEvent.Type == Events.PaymentIntentPaymentFailed)
            {
                string transaction_result = "Failed";
                if(stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    transaction_result = "Successful";
                }

                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                var cust_id = paymentIntent.CustomerId;

                string cmdString = "INSERT INTO transaction_log (cust_stripe_id, paymentintent_id, event_date, event_result) " +
                    "VALUES (@val1, @val2, @val3, @val4)";
                DateTime now = DateTime.Now;
                using (SqlCommand comm = new SqlCommand(cmdString, db.conn))
                {

                    comm.Parameters.AddWithValue("@val1", cust_id);
                    comm.Parameters.AddWithValue("@val2", paymentIntent.Id);
                    comm.Parameters.AddWithValue("@val3", now);
                    comm.Parameters.AddWithValue("@val4", transaction_result);

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
            }

            else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
            {

                var subscription = stripeEvent.Data.Object as Subscription;
                var cust_id = subscription.CustomerId;
                var price_id = subscription.Items.Data[0].Price.Id;


                string cmdString = "UPDATE subscriptions SET price_id = @val1 " +
                    "WHERE subscription_id = @val2 AND cust_stripe_id = @val3 ";

                using (SqlCommand comm = new SqlCommand(cmdString, db.conn))
                {

                    comm.Parameters.AddWithValue("@val1", price_id);
                    comm.Parameters.AddWithValue("@val2", subscription.Id);
                    comm.Parameters.AddWithValue("@val3", cust_id);

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

            }

            else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
            {

                var subscription = stripeEvent.Data.Object as Subscription;


                string cmdString = "DELETE FROM subscriptions WHERE subscription_id = @val1 ";

                using (SqlCommand comm = new SqlCommand(cmdString, db.conn))
                {

                    comm.Parameters.AddWithValue("@val1", subscription.Id);

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

            }

            

            else

            {

                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);

            }

            return Ok();

            

    }
    }
}
