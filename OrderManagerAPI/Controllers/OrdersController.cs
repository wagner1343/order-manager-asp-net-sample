using OrderManagerAPI.Context;
using OrderManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using static OrderManagerAPI.Controllers.ClientsController;
using static OrderManagerAPI.Controllers.ProductsController;

namespace OrderManagerAPI.Controllers
{
    public class OrdersController : ApiController
    {

        public class OrderData
        {
            public long Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public long? Number { get; set; }
            public virtual ICollection<ProductData> Products { get; set; }
            public ClientData Client { get; set; }
            public double Value { get; set; }
            public double Discount { get; set; }
            public double TotalValue { get; set; }
        }


        static OrderData getData(Order order)
        {
            return new OrderData
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Products = order.Products.Select(ProductsController.getData).ToList(),
                Client = ClientsController.getData(order.Client),
                Value = order.Value,
                Discount = order.Discount,
                TotalValue = order.TotalValue,
            };
        }

        public List<OrderData> Get()
        {
            using (var context = new OrderManagerDBContext())
            {
                return context.Orders.Select(getData).ToList();
            }
        }

        public OrderData Get(int id)
        {
            using (var context = new OrderManagerDBContext())
            {
                var orders = context.Orders.Find(id);
                return getData(orders);
            }
        }

        public class NewOrderData
        {
            public virtual List<long> ProductIds { get; set; }
            public long? ClientId { get; set; }
            public double Discount { get; set; }
        }


        public IHttpActionResult Post(NewOrderData data)
        {
            using (var context = new OrderManagerDBContext())
            {
                try
                {
                    var client = data.ClientId != null ? context.Clients.Find(data.ClientId) : null;
                    var products = data.ProductIds != null ? context.Products.Where(p => data.ProductIds.Contains(p.Id)).ToList() : null;
                    double value = products?.Aggregate(0.0, (sum, next) => sum + next.Price) ?? 0;
                    data.Discount = Math.Max(0, data.Discount);
                    double totalValue = value - data.Discount;

                    var order = new Order()
                    {
                        CreatedAt = DateTime.Now,
                        Client = client,
                        Products = products,
                        Value = value,
                        Discount = data.Discount,
                        TotalValue = totalValue
                    };

                    context.Orders.Add(order);
                    context.SaveChanges();

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