using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Users.Controllers
{
    public class UsersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            List<ClassLibrary1.UsersModelView> lst = new List<ClassLibrary1.UsersModelView>();
            using (Models.CustomerEntities1 db = new Models.CustomerEntities1())
            {
                lst = (from d in db.Details
                       select new ClassLibrary1.UsersModelView
                       {
                           Id = d.Id,
                           Nombre = d.Nombre,
                           Apellido = d.Apellido,
                           Email = d.Email
                       }).ToList();
            }
            return Ok(lst);
        }

        [HttpPost]
        public IHttpActionResult Add(ClassLibrary1.UsersModelView model)
        {
            using (Models.CustomerEntities1 db = new Models.CustomerEntities1())
            {
                var user = new Models.Details();
                user.Nombre = model.Nombre;
                user.Apellido = model.Apellido;
                user.Email = model.Email;
                db.Details.Add(user);
                db.SaveChanges();
            }

            return Ok("Exito");
        }

        [HttpDelete]
        public IHttpActionResult Deleted(ClassLibrary1.UsersModelView model)
        {
            using(Models.CustomerEntities1 db = new Models.CustomerEntities1())
            {
                var user = new Models.Details();
                user.Id = model.Id;
                var deleted = db.Details.Where(c => c.Id == user.Id).FirstOrDefault();
                db.Details.Attach(deleted);
                db.Details.Remove(deleted);
                db.SaveChanges();
            }
            return Ok("Borrado");
        }
    }
}
