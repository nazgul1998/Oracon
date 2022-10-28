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
    public class AdminController : Controller
    {
        OraconContext cnx;

        public readonly IConfiguration configuration;

        private readonly IHostingEnvironment hostingEnvironment;
       
        public AdminController(OraconContext cnx, IConfiguration configuration, IHostingEnvironment environment)
        {
            this.configuration = configuration;
            this.cnx = cnx;
            this.hostingEnvironment = environment;
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
        public ActionResult Index()
        {
            ViewBag.Pagos = cnx.Pagos.Include("matricula.usuario").Include("matricula.curso").ToList();

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult CreateDoctor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateDoctor(string nombres, string apellidos, DateTime fechaNac,
            string correo, string passwd, string usuario, string celular, string dni,
            string titulo, string descripcion, IFormFile foto)
        {
            var profesores = cnx.Profesores.ToList();
            Profesor u = new Profesor();

            if (cnx.Profesores.Where(o => o.correo == correo).FirstOrDefault() == null)
            {
                u.nombres = nombres;
                u.apellidos = apellidos;
                u.fechaNac = fechaNac;
                u.correo = correo;
                u.passwd = CreateHash(passwd);
                u.usuario = usuario;
                u.celular = celular;
                u.dni = dni;
                u.titulo = titulo;
                u.descripcion = descripcion;
                if (foto != null)
                {

                    u.foto = GetByteArrayFromImage(foto);

                    //to do : Save uniqueFileName  to your db table   
                }
                else
                {
                    u.foto = null;
                }
                cnx.Profesores.Add(u);
                cnx.SaveChanges();

                return RedirectToAction("VerProfesores", "Admin");
            }
            if (cnx.Profesores.Where(o => o.usuario == usuario).FirstOrDefault() == null)
            {
                u.nombres = nombres;
                u.apellidos = apellidos;
                u.fechaNac = fechaNac;
                u.correo = correo;
                u.passwd = CreateHash(passwd);
                u.usuario = usuario;
                u.celular = celular;
                u.dni = dni;
                u.titulo = titulo;
                u.descripcion = descripcion;
                if (foto != null)
                {

                    u.foto = GetByteArrayFromImage(foto);

                    //to do : Save uniqueFileName  to your db table   
                }
                else
                {
                    u.foto = null;
                }
                cnx.Profesores.Add(u);
                cnx.SaveChanges();

                return RedirectToAction("VerProfesores", "Admin");
            }

            return View();
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        [HttpGet]
        public ActionResult VerProfesores()
        {
            var list = cnx.Profesores.Include(o => o.cursos).ToList();
            ViewBag.Profesores = list;

            return View();
        }


        [HttpGet]
        public ActionResult AgregarCurso()
        {

            ViewBag.Profesores = cnx.Profesores.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult AgregarCurso(string nombre, string descripcion, 
            string videoPresentacion,int idProfesor,decimal precio, string estado)
        {
            var curso = cnx.Cursos.Where(o => o.nombre == nombre).FirstOrDefault();

            if (curso != null)
            {

            }
            else
            {
                var c = new Curso();
                c.nombre = nombre;
                c.descripcion = descripcion;
                c.videoPresentacion = videoPresentacion;
                c.idProfesor = idProfesor;
                c.precio = precio;
                c.estado = estado;
                cnx.Cursos.Add(c);
                cnx.SaveChanges();
            }



            return RedirectToAction("VerCursos", "Admin");
        }

        [HttpPost]
        public ActionResult ActivarCurso(int idCurso)
        {

            var curso = cnx.Cursos.Where(o => o.idCurso == idCurso).FirstOrDefault();

            curso.estado = "ACTIVO";

            cnx.Update(curso);
            cnx.SaveChanges();
            return RedirectToAction("VerCursos", "Admin");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> VerCursos()
        {

            var cursos = cnx.Cursos.Include(o => o.profesor).Include("etiquetas.categoria").ToList();
            ViewBag.Cursos = cursos;

            return View();
        }



        [HttpGet]
        public ActionResult VerPagos()
        {

            ViewBag.Pagos = cnx.Pagos.Include("matricula.usuario").Include("matricula.curso").ToList();

            return View();
        }

        [HttpPost]
        public ActionResult CancelarPago(int idPago)
        {
            var pago = cnx.Pagos.Include(o=>o.matricula).Where(o => o.idPago == idPago).FirstOrDefault();

            

                pago.matricula.estadoMatricula = "CONFIRMADO";
                pago.estado = "PAGO";
                cnx.Matriculas.Update(pago.matricula);
                cnx.Pagos.Update(pago);


                cnx.SaveChanges();
            
            return RedirectToAction("VerPagos", "Admin");
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string passwd)
        {
            var user = cnx.Admins.Where(o => o.usuario == usuario && CreateHash(passwd) == o.passwd).FirstOrDefault();
            if (user != null)
            {
                //Guardamos el claim
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(claimsPrincipal);

                return RedirectToAction("Index", "Admin");
            }
            TempData["LoginAdmin"] = "Usuario y/o contraseña incorrectos.";
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Admin");
        }



    }
}
