using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CSC_AY2020_Task6.Database;
using Microsoft.Data.SqlClient;
using Stripe;
using System.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;
using Newtonsoft.Json;

namespace CSC_AY2020_Task6.API
{

    [Route("webhook")]
    public class WebhookController : ApiController
    {
        [HttpPost]

        public async Task<IHttpActionResult> Index()

        {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                var stripeEvent = EventUtility.ParseEvent(json);

                if (stripeEvent.Type == Events.PaymentIntentSucceeded)

                {

                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    Console.WriteLine("A successful payment for {0} was made.", paymentIntent.Amount);

                    // Then define and call a method to handle the successful payment intent.

                    // handlePaymentIntentSucceeded(paymentIntent);

                }

                else if (stripeEvent.Type == Events.PaymentMethodAttached)

                {

                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;

                    // Then define and call a method to handle the successful attachment of a PaymentMethod.

                    // handlePaymentMethodAttached(paymentMethod);

                }

                else

                {

                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);

                }

                return Ok();

            


    }
    }
}
