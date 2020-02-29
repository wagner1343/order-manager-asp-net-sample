using OrderManagerAPI.Context;
using OrderManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OrderManagerAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductsController : ApiController
    {
        public class ProductData
        {
            public long id { get; set; }
            public DateTime createdAt { get; set; }
            public string description { get; set; }
            public double price { get; set; }
            public string imageURL { get; set; }
        }

        public static ProductData getData(Product product)
        {
            return new ProductData
            {
                id = product.Id,
                createdAt = product.CreatedAt,
                description = product.Description,
                price = product.Price,
                imageURL = product.ImageURL,
            };
        }

        public List<ProductData> Get()
        {
            using (var context = new OrderManagerDBContext())
            {
                return context.Products.Select(getData).ToList();
            }
        }

        public ProductData Get(int id)
        {
            using (var context = new OrderManagerDBContext())
            {
                var products = context.Products.Find(id);
                return getData(products);
            }
        }

        public class NewProductData
        {
            public string Description { get; set; }
            public double Price { get; set; }
        }



        public IHttpActionResult Post(NewProductData data)
        {
            using (var context = new OrderManagerDBContext())
            {
                try
                {
                    data.Description = data.Description?.Trim();

                    var product = new Product()
                    {
                        CreatedAt = DateTime.Now,
                        Description = data.Description,
                        Price = data.Price,
                    };

                    product = context.Products.Add(product);
                    context.SaveChanges();
                    var files = HttpContext.Current.Request.Files;
                    if (files.Count > 0)
                    {
                        var image = files[0];
                        string serverPath = HttpContext.Current.Server.MapPath("~/");
                        string relativeFolderPath = "public/product_images/";
                        string relativePath = Path.Combine(relativeFolderPath, $"{product.Id}.jpg");
                        string absoluteFolderPath = Path.Combine(serverPath, relativeFolderPath);
                        string absolutePath = Path.Combine(serverPath, relativePath);
                        string hostName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                        Directory.CreateDirectory(absoluteFolderPath);
                        image.SaveAs(absolutePath);
                        product.ImageURL = $"{hostName}/{relativePath}";
                        context.SaveChanges();
                    }

                    return Ok<ProductData>(getData(product));
                }
                catch (DbEntityValidationException validationException)
                {
                    return BadRequest(validationException.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                }

            }
        }
    }
}