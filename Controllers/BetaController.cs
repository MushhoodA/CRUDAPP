using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using crudapp.Models;
namespace crudapp.Controllers
{
    public class BetaController : ApiController
    {
        MushhoodEntities db = new MushhoodEntities();

        [HttpGet]
        public HttpResponseMessage allbetadata()
        {
            try
            {
                var data = db.betas.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        public HttpResponseMessage addbetadata(beta ba)
        {
            try
            {
                db.betas.Add(ba);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, ba.id);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public HttpResponseMessage updatebetadata(beta al)
        {
            try
            {
                var original = db.betas.Find(al.rollnum);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found");
                }
                db.Entry(original).CurrentValues.SetValues(al);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Data Updated");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage deletebetadata(int id)
        {
            try
            {
                var original = db.betas.Find(id);
                if (original == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Data not found");
                }
                db.Entry(original).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Data Deleted Successfully");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
