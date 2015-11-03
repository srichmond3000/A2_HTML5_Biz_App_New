using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using A2_HTML5_Biz_App_New.Models;

namespace A2_HTML5_Biz_App_New.Controllers
{
    public class CustomAPIController : ApiController
    {
        private PropertyStoreEntities _ctx;

        public CustomAPIController()
        {
            _ctx = new PropertyStoreEntities();
        }

        [Route("Owner/Get")]
        [ResponseType(typeof(OwnerInfo))]
        public IHttpActionResult GetOwner()
        {
            try
            {
                var user = this.User.Identity.Name;
                var owner = (from o in _ctx.OwnerInfoes.ToList()
                             where o.Email == user
                             select new OwnerInfo()
                             {
                                 OwnerId = o.OwnerId,
                                 OwnerName = o.OwnerName,
                                 Address = o.Address,
                                 City = o.City,
                                 Email = o.Email,
                                 Contact1 = o.Contact1,
                                 Contact2 = o.Contact2
                             }).First();

                return Ok(owner);
            }
            catch(Exception e)
            {
                return Ok(new OwnerInfo());
            }
        }

        [Route("Customer/Get")]
        [ResponseType(typeof(CustomerInfo))]
        public IHttpActionResult GetCustomer()
        {
            try
            {
                var user = this.User.Identity.Name;
                var customer = (from c in _ctx.CustomerInfoes.ToList()
                                where c.Email == user
                                select new CustomerInfo()
                                {
                                    CustomerId = c.CustomerId,
                                    CustomerName = c.CustomerName,
                                    Address = c.Address,
                                    City = c.City,
                                    Contact1 = c.Contact1,
                                    Contact2 = c.Contact2,
                                    Email = c.Email
                                }).First();
                
                return Ok(customer);
            }
            catch(Exception e)
            {
                return Ok(new CustomerInfo());
            }
        }
        
    }
}
