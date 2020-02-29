using OrderManagerAPI.Context;
using OrderManagerAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OrderManagerAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClientsController : ApiController
    {
        public class ClientData
        {
            public long id { get; set; }
            public DateTime createdAt { get; set; }
            public string name { get; set; }
            public string email { get; set; }
        }

        public static ClientData getData(Client client)
        {
            return new ClientData
            {
                id = client.Id,
                createdAt = client.CreatedAt,
                name = client.Name,
                email = client.Email
            };
        }

        public List<ClientData> Get()
        {
            using (var context = new OrderManagerDBContext())
            {
                return context.Clients.Select(getData).ToList();
            }
        }

        public ClientData Get(int id)
        {
            using (var context = new OrderManagerDBContext())
            {
                var client = context.Clients.Find(id);
                return getData(client);
            }
        }

        public class NewClientData
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }



        public IHttpActionResult Post(NewClientData data)
        {

            using (var context = new OrderManagerDBContext())
            {
                try
                {
                    data.Email = data.Email.Trim().ToLower();

                    var client = new Client()
                    {
                        CreatedAt = DateTime.Now,
                        Name = data.Name,
                        Email = data.Email
                    };

                    client = context.Clients.Add(client);
                    context.SaveChanges();

                    return Ok<ClientData>(getData(client));
                }
                catch (DbEntityValidationException validationException)
                {
                    return BadRequest(validationException.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                }
                catch (DbUpdateException e)
                {
                    return BadRequest($"Já existe um cliente com o email {data.Email} cadastrado.");
                }

            }
        }

    }
}
