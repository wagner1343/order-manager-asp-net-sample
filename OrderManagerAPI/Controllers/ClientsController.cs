using System;
using System.Collections.Generic;
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
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public List<ClientData> Get()
        {
            using (var context = new OrderManagerDBContext())
            {
                var clients = context.Clients.ToList();
                var clientsData = clients.Select<Client, ClientData>
                    (
                        c => new ClientData
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Email = c.Email
                        }
                    );

                return clientsData.ToList();
            }
        }

        public ClientData Get(int id)
        {
            using (var context = new OrderManagerDBContext())
            {
                var client = context.Clients.Find(id);

                var clientData = new ClientData()
                {
                    Id = client.Id,
                    Name = client.Name,
                    Email = client.Email
                };

                return clientData;
            }
        }

        public class NewClientData
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public void Post(NewClientData data)
        {
            using (var context = new OrderManagerDBContext())
            {
                var client = new Client()
                {
                    Name = data.Name,
                    Email = data.Email
                };

                context.Clients.Add(client);
                context.SaveChanges();
            }
        }

    }
}
