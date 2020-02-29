using OrderManagerAPI.Context;
using OrderManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using static OrderManagerAPI.Controllers.ClientsController;
using static OrderManagerAPI.Controllers.ProductsController;

namespace OrderManagerAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrdersController : ApiController
    {

        public class OrderProductData
        {
            public ProductData product { get; set; }
            public int amount { get; set; }
        }

        public class OrderData
        {
            public long id { get; set; }
            public DateTime createdAt { get; set; }
            public virtual ICollection<OrderProductData> products { get; set; }
            public ClientData client { get; set; }
            public double value { get; set; }
            public double discount { get; set; }
            public double totalValue { get; set; }
        }

        static OrderProductData getOrderProductData(OrderProduct orderProduct)
        {
            return new OrderProductData
            {
                product = ProductsController.getData(orderProduct.Product),
                amount = orderProduct.Amount
            };
        }


        static OrderData getData(Order order)
        {
            return new OrderData
            {
                id = order.Id,
                createdAt = order.CreatedAt,
                products = order.Products.Select(getOrderProductData).ToList(),
                client = ClientsController.getData(order.Client),
                value = order.Value,
                discount = order.Discount,
                totalValue = order.TotalValue,
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

        public class NewOrderProductData
        {
            public int ProductId { get; set; }
            public int Amount { get; set; }
        }

        public class NewOrderData
        {
            public virtual List<NewOrderProductData> Products { get; set; }
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
                    var products = new List<OrderProduct>();

                    if (data.Products != null)
                    {
                        foreach (var orderProductData in data.Products)
                        {
                            var p = context.Products.Find(orderProductData.ProductId);
                            if (p != null)
                            {
                                products.Add(new OrderProduct { Product = p, Amount = orderProductData.Amount, CreatedAt = DateTime.Now });
                            }
                            else
                            {
                                return BadRequest($"O produto com o id {orderProductData.ProductId} não pode ser encontrado");
                            }
                        }
                    }

                    double value = products?.Aggregate(0.0, (sum, next) => sum + (next.Amount * next.Product.Price)) ?? 0;
                    data.Discount = Math.Max(0, data.Discount);
                    double totalValue = Math.Max(value - data.Discount, 0);

                    var order = new Order()
                    {
                        CreatedAt = DateTime.Now,
                        Client = client,
                        Products = products,
                        Value = value,
                        Discount = data.Discount,
                        TotalValue = totalValue
                    };

                    order = context.Orders.Add(order);
                    context.SaveChanges();

                    return Ok<OrderData>(getData(order));
                }
                catch (DbEntityValidationException validationException)
                {
                    return BadRequest(validationException.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                }
            }
        }
    }
}