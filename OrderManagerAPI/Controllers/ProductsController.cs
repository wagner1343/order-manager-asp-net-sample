using OrderManagerAPI.Context;
using OrderManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OrderManagerAPI.Controllers
{
    public class ProductsController : ApiController
    {
        public class ProductData
        {
            public long Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public string ImageURL { get; set; }
        }

        public static ProductData getData(Product product)
        {
            return new ProductData
            {
                Id = product.Id,
                CreatedAt = product.CreatedAt,
                Description = product.Description,
                Price = product.Price,
                ImageURL = product.ImageURL,
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

                    return Ok();
                }
                catch (DbEntityValidationException validationException)
                {
                    return BadRequest(validationException.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                }

            }
        }
    }
}