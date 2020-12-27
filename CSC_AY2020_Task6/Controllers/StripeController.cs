using CSC_AY2020_Task6.Models;
using Stripe;
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


        [HttpPost]
        [Route("Register")]
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
                    Address =
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
