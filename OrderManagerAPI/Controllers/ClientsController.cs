using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OrderManagerAPI.Context;
using OrderManagerAPI.Models;

namespace OrderManagerAPI.Controllers
{
    public class ClientsController : ApiController
    {
        public class ClientData
        {
            public long Id { get; set; }
            public DateTime createdAt { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public static ClientData getData(Client client)
        {
            return new ClientData
            {
                Id = client.Id,
                createdAt = client.CreatedAt,
                Name = client.Name,
                Email = client.Email
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

                    context.Clients.Add(client);
                    context.SaveChanges();

                    return Ok();
                }
                catch (DbEntityValidationException validationException)
                {
                    return BadRequest(validationException.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                }
                catch(DbUpdateException e)
                {
                    return BadRequest($"Já existe um cliente com o email {data.Email} cadastrado.");
                }
                
            }
        }

    }
}
