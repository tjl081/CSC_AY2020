using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebAPI2.Models;

namespace WebAPI2.Controllers
{
    public class ProductsV1Controller : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();

        [HttpGet]
        [Route("api/v1/products")]
        //https://localhost:44384/api/v1/products

        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();

        }

        [HttpGet]
        [Route("api/v1/products")]
        //https://localhost:44384/api/v1/products
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        [HttpGet]
        [Route("api/v1/products/{id:int:min(1)}", Name = "getProductById")]
        //https://localhost:44384/api/v1/products/1
        public HttpResponseMessage GetProduct(int id)
        {
            var product = repository.Get(id);
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to find product!");
            }
            return Request.CreateResponse<Product>(HttpStatusCode.OK, product);
        }

        [HttpPost]
        [Route("api/v1/products")]
        //https://localhost:44384/api/v1/products
        public HttpResponseMessage CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                product = repository.Add(product);
                var response = Request.CreateResponse<Product>(HttpStatusCode.Created, product);

                string uri = Url.Link("getProductById", new { id = product.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            

        }

        [HttpPut]
        [Route("api/v1/products/{id:int:min(1)}")]
        //https://localhost:44384/api/v1/products/2
        public HttpResponseMessage UpdateProduct(Product product, int id)
        {
            product.Id = id;
            if (ModelState.IsValid)
            {
                repository.Update(product);
                return Request.CreateResponse(HttpStatusCode.OK, "Product updated successfully!");
                
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }

        [HttpDelete]
        [Route("api/v1/products/{id:int:min(1)}")]
        //https://localhost:44384/api/v1/products/1
        public HttpResponseMessage DeleteProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Product deleting failed!");
            }

            repository.Remove(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Product deleted successfully!");

        }
    }
}
