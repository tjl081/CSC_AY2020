using CSC_AY2020_Task6.Models;
using Stripe;
using Stripe.BillingPortal;  //for the customer portal
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace CSC_AY2020_Task6.Controllers
{
    public class StripeController : ApiController
    {


        [HttpGet]
        [Route("api/Stripe/ListPrice")]
        public  IHttpActionResult ListPrice()
        {
            
                StripeConfiguration.ApiKey = "sk_test_51I2bbHBYTNT0yLeeR90ErQBVJPbr5wYhg9NySyKsKb3HpFJ64fiTvVAsSYQpq53Vx4yk180e4uJbLMZ1F7AwhUTD00bbyfNUiO";

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

                if(priceList.Count != 0)
                {
                    return Ok(priceList);
                }
                else
                {
                    var msg = "No products found";
                    return Content(HttpStatusCode.BadRequest, msg);
                }

        }

        [HttpPost]
        [Route("api/Stripe/PurchasePlan")]
        public IHttpActionResult PurchasePlan(PurchaseModel purchaseData)
        {
            StripeConfiguration.ApiKey = "sk_test_51I2bbHBYTNT0yLeeR90ErQBVJPbr5wYhg9NySyKsKb3HpFJ64fiTvVAsSYQpq53Vx4yk180e4uJbLMZ1F7AwhUTD00bbyfNUiO";
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
            var paymentService = new PaymentMethodService();
            var paymentMethod = paymentService.Create(paymentOptions);



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

            return Ok();
        }

        [HttpPost]
        [Route("api/Stripe/AccessPortal")]
        public IHttpActionResult AccessPortal(string custId)
        {
            
            StripeConfiguration.ApiKey = "sk_test_51I2bbHBYTNT0yLeeR90ErQBVJPbr5wYhg9NySyKsKb3HpFJ64fiTvVAsSYQpq53Vx4yk180e4uJbLMZ1F7AwhUTD00bbyfNUiO";

            var options = new SessionCreateOptions
            {
                Customer = custId,
                ReturnUrl = "https://localhost:44344/Home/Charge",
            };
            var service = new SessionService();
            var session = service.Create(options);

            return Ok(session.Url);
        }
    }
}
