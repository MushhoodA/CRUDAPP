using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using crudapp.Models;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace crudapp.Controllers
{
    public class AlphaController : ApiController
    {
        MushhoodEntities db = new MushhoodEntities();

        [HttpPost]
        public Object GetToken(alpha al)
        {
            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "http://mysite.com";  //normally this will be your site URL    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", (al.id).ToString()));
            permClaims.Add(new Claim("id", (al.id).ToString()));
            permClaims.Add(new Claim("myname", al.myname));

            //Create Security Token object by giving required parameters    
            var token = new JwtSecurityToken(issuer, //Issure    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwt_token };
        }




        [Authorize]
        [HttpPost]
        public Object GetName2()
        {
            var identity = User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var name = claims.Where(p => p.Type == "name").FirstOrDefault()?.Value;
                var id = claims.Where(p => p.Type == "id").FirstOrDefault()?.Value;
                return new
                {
                    data = name, id
                };

            }
            return null;
        }



        [Authorize]
        [HttpGet]
        public HttpResponseMessage allalphadata()
        {
            try
            {
                var data = db.alphas.ToList();
                return Request.CreateResponse(HttpStatusCode.OK, data);

            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public HttpResponseMessage addalphadata()
        {
            try
            {

                //db.alphas.Add(al);
                //db.SaveChanges();
                //return Request.CreateResponse(HttpStatusCode.OK, al.id);

                var identity = User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    var myname = claims.Where(p => p.Type == "myname").FirstOrDefault()?.Value;
                    var id = claims.Where(p => p.Type == "id").FirstOrDefault()?.Value;
                    alpha al = new alpha();
                    al.id = Int16.Parse(id);

                    al.myname = myname;
                    db.alphas.Add(al);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, al.id);
                }
                return null;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        public HttpResponseMessage updatealphadata(alpha al)
        {
            try
            {
                var original = db.alphas.Find(al.id);
            if(original == null)
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
        public HttpResponseMessage deletealphadata(int id)
        {
            try
            {
                var original = db.alphas.Find(id);
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
