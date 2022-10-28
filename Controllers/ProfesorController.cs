using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracon.Models;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Oracon.Controllers
{
    public class ProfesorController : Controller
    {

        OraconContext cnx;

        public readonly IConfiguration configuration;
        private readonly IHostingEnvironment hostingEnvironment;

        public ProfesorController(OraconContext cnx, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.configuration = configuration;
            this.cnx = cnx;
            this.hostingEnvironment = hostingEnvironment;
        }

        [Authorize]
        public ActionResult Index()
        {
            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            if(user == null)
            {
                return RedirectToAction("Login","Profesor");
            }
            
           
            ViewBag.CursosProf = cnx.Cursos.Where(o => o.idProfesor == user.idProfesor).ToList();
            ViewBag.ProfesorLogeado = user;

            return View();
        }



        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Profesor");
        }

        [HttpPost]
        public ActionResult Login(string usuario, string passwd)
        {
            var user = cnx.Profesores.Where(o => o.usuario == usuario && CreateHash(passwd) == o.passwd).FirstOrDefault();
            if (user != null)
            {
                //Guardamos el claim
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(claimsPrincipal);

                return RedirectToAction("Index", "Profesor");
            }
            else
            {
                TempData["LoginProfesor"] = "Usuario y/o contraseña invalidos.";
            }

            return View();
        }

        private string CreateHash(string input)
        {
            var sha = SHA256.Create();
            input += configuration.GetValue<string>("Token");
            var hash = sha.ComputeHash(Encoding.Default.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

        [Authorize]
        [HttpGet]
        public ActionResult VerCurso(int idCurso)
        {
            
            ViewBag.IdCurso= cnx.Cursos.Where(o => o.idCurso == idCurso)
                .FirstOrDefault();
            ViewBag.Curso = cnx.Cursos.Where(o => o.idCurso == idCurso)
                .Include(o=>o.modulos).FirstOrDefault();
            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            ViewBag.ProfesorLogeado = user; 

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateModulo(int idCurso)
        {
            ViewBag.IdCurso = cnx.Cursos.Where(o => o.idCurso == idCurso).FirstOrDefault().idCurso;
            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            ViewBag.ProfesorLogeado = user;

            return View();
        }

        [HttpPost]
        public ActionResult CreateModulo(int idCurso, string titulo,string descripcion)
        {
            var modulos = cnx.Modulos.ToList();
            Modulo u = new Modulo();

            if (cnx.Modulos.Where(o => o.titulo == titulo && o.descripcion == descripcion
            && o.idCurso==idCurso).FirstOrDefault() == null)
            {
                u.idCurso = idCurso;
                u.titulo = titulo;
                u.descripcion = descripcion;
              
                cnx.Modulos.Add(u);
                cnx.SaveChanges();
                
                return RedirectToAction("VerCurso", "Profesor", new { idCurso = idCurso});
            }
            

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult AgregarVideoModulo(int idModulo)
        {
            ViewBag.IdModulo = idModulo;

            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            ViewBag.ProfesorLogeado = user;


            
            return View();
        }

        [HttpPost]
        public ActionResult AgregarVideoModulo(int idModulo, string link, string titulo, string descripcion)
        {
            var vid = cnx.Videos.Where(o => o.idModulo == idModulo &&
            o.link == link && o.titulo == titulo &&
            o.descripcion == descripcion).FirstOrDefault();

            if (vid != null)
            {
                TempData["AgregarRecurso"] = "El video ya existe.";
                return RedirectToAction("VerMaterialModulo", "Profesor", new { idModulo = idModulo });
                
            }
            var video = new Video();
            video.idModulo = idModulo;
            //video.link = link.Replace("watch?v=", "embed/");
            video.link = link;
            video.titulo = titulo;
            video.descripcion = descripcion;
            cnx.Videos.Add(video);
            cnx.SaveChanges();
            TempData["AgregarRecurso"] = "El video se agrego con éxito.";
            return RedirectToAction("VerMaterialModulo", "Profesor", new { idModulo = idModulo });
        }


        [Authorize]
        [HttpGet]
        public ActionResult AgregarDocumentoModulo(int idModulo)
        {
            ViewBag.IdModulo = idModulo;

            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            ViewBag.ProfesorLogeado = user;

            return View();
        }

        [HttpPost]
        public ActionResult AgregarDocumentoModulo(int idModulo, IFormFile documento, string titulo, string descripcion)
        {
            var vid = cnx.Documentos.Where(o => o.idModulo == idModulo
            && o.titulo == titulo && o.descripcion == descripcion).FirstOrDefault();

            if (vid != null)
            {
                TempData["AgregarRecurso"] = "El documento ya existe.";
                return RedirectToAction("VerMaterialModulo", "Profesor", new { idModulo = idModulo });
            }
            var document = new Documento();
            document.idModulo = idModulo;
           
            document.documento = GetByteArrayFromDocumento(documento);
            
            document.titulo = titulo;
            document.descripcion = descripcion;
            cnx.Documentos.Add(document);
            cnx.SaveChanges();
            TempData["AgregarRecurso"] = "El documento se agrego con éxito.";
            return RedirectToAction("VerMaterialModulo", "Profesor", new { idModulo = idModulo });
        }


        public FileResult PDFDisplay(int idDocumento)
        {
            var documento = cnx.Documentos.Where(o => o.idDocumento == idDocumento).FirstOrDefault();

            if (documento != null)
            {
                return File(documento.documento, "application/pdf");
            }
            else
            {
                return null;
            }
            
        }

        public FileResult PDFDisplayTarea(int idTarea)
        {
            var entrega = cnx.Entregas.Where(o => o.idTarea == idTarea).FirstOrDefault();

            if (entrega != null)
            {
                return File(entrega.documento, "application/pdf");
            }
            else
            {
                return null;
            }

        }

        private byte[] GetByteArrayFromDocumento(IFormFile file)
        {
            
                byte[] x;
                
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        x = fileBytes;
                        // act on the Base64 data
                    }
                
                return x;
            
        }




        [Authorize]
        [HttpGet]
        public ActionResult AgregarEnlaceModulo(int idModulo)
        {
            ViewBag.IdModulo = idModulo;

            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            ViewBag.ProfesorLogeado = user;

            

            return View();
        }

        [HttpPost]
        public ActionResult AgregarEnlaceModulo(int idModulo, string link, string titulo, string descripcion)
        {
            var li = cnx.Enlaces.Where(o => o.idModulo == idModulo
            && o.titulo == titulo && o.descripcion == descripcion).FirstOrDefault();

            if (li != null)
            {
                TempData["AgregarRecurso"] = "El enlace externo sya existe.";
                return RedirectToAction("VerMaterialModulo", "Profesor", new { idModulo = idModulo });
            }
            var enlace = new Enlace();
            enlace.titulo = titulo;
            enlace.link = link;
            enlace.descripcion = descripcion;
            enlace.idModulo = idModulo;
            
            cnx.Enlaces.Add(enlace);
            cnx.SaveChanges();
            TempData["AgregarRecurso"] = "El enlace externo se agrego con éxito.";


            return RedirectToAction("VerMaterialModulo", "Profesor", new { idModulo = idModulo });
        }


        [Authorize]
        [HttpGet]
        public ActionResult VerMaterialModulo(int idModulo)
        {
            //obtener modulo
            var modulo = cnx.Modulos.Where(o => o.idModulo == idModulo).Include(o => o.videos)
                .Include(o => o.documentos).Include(o => o.enlaces).Include(o => o.tareas).FirstOrDefault();
            //videos del modulo .
            ViewBag.Videos = modulo.videos.ToList();
            //Documentos del modulo . 
            ViewBag.Documentos = modulo.documentos.ToList();
            //Enlaces Externos .
            ViewBag.Enlaces = modulo.enlaces.ToList();
            //Tareas pendientes
            ViewBag.Tareas = modulo.tareas.ToList();

            ViewBag.Modulo = modulo;

            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            ViewBag.ProfesorLogeado = user;

            return View();
        }



        [Authorize]
        [HttpGet]
        public ActionResult AgregarTareaModulo(int idModulo)
        {
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            ViewBag.ProfesorLogeado = user;

            ViewBag.IdModulo = idModulo;
            return View();
        }

        [HttpPost]
        public ActionResult AgregarTareaModulo(int idModulo, string nombre, string instrucciones, 
            DateTime fechaCreacion, DateTime fechaMaxima)
        {
            var vid = cnx.Tareas.Where(o => o.idModulo == idModulo
            && o.nombre == nombre && o.instrucciones == instrucciones && o.fechaMaxima == fechaMaxima
            && o.fechaCreacion == fechaCreacion).FirstOrDefault();


            if (vid != null)
            {
                TempData["AgregarRecurso"] = "La tarea ya existe.";
                return RedirectToAction("VerMaterialModulo", "Profesor", new { idModulo = idModulo });
            }
            var tarea = new Tarea();
            tarea.idModulo = idModulo;

            tarea.nombre =nombre;
            tarea.instrucciones = instrucciones;
            tarea.fechaCreacion = DateTime.Now;
            tarea.fechaMaxima = fechaMaxima;

            cnx.Tareas.Add(tarea);
            cnx.SaveChanges();
            TempData["AgregarRecurso"] = "La tarea se agrego con éxito.";
            return RedirectToAction("VerMaterialModulo", "Profesor", new { idModulo = idModulo });
        }

        [HttpGet]
        public ActionResult VerEntregas(int idTarea)
        {
            ViewBag.IdTarea = idTarea;

            var entregas = cnx.Entregas.Where(o => o.idTarea == idTarea).Include(o=>o.usuario).ToList();
            ViewBag.Entregas = entregas;
            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Profesores.Where(o => o.usuario == claim.Value).FirstOrDefault();

            ViewBag.ProfesorLogeado = user;

            return View();
        }


    }
}
