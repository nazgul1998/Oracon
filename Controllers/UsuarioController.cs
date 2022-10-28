using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oracon.Models;
using Oracon.Models.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Oracon.Controllers
{
    public class UsuarioController : Controller
    {

        OraconContext cnx;

        public readonly IConfiguration configuration;


        public UsuarioController(OraconContext cnx, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.cnx = cnx;

        }


        public IActionResult Index()
        {
            var cursos = cnx.Cursos.Include(o => o.profesor).Include("etiquetas.categoria").ToList();
            ViewBag.Cursos = cursos;

            return View();
        }



        [Authorize]
        public IActionResult IndexUsuario()
        {

            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Usuarios.Where(o => o.usuario == claim.Value)
                .Include(o=>o.cursos).First();
            ViewBag.UsuarioLogeado = user;

            //obtener lista de cursos
            var cursos = cnx.Cursos.Include(o => o.profesor).Where(o=>o.estado == "Activo").Include("etiquetas.categoria").ToList();
            ViewBag.CursosDisponibles = cursos;

            var categorias = cnx.Categorias.ToList();
            
            //obtener categorias disponibles
            ViewBag.CategoriasDisponibles = categorias;
            
            return View();
        }

        public ActionResult CursoCategoria(int idCategoria)
        {


            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Usuarios.Where(o => o.usuario == claim.Value)
                .Include(o => o.cursos).First();
            ViewBag.UsuarioLogeado = user;

            //obtener lista de cursos
            var cursos = cnx.Etiquetas.Include("curso.profesor").Where(o=>o.idCategoria == idCategoria).ToList();
            ViewBag.CursosDisponibles = cursos;

            var categorias = cnx.Categorias.ToList();

            //obtener categorias disponibles
            ViewBag.CategoriasDisponibles = categorias;


            ViewBag.CategoriaSeleccionada = cnx.Categorias.Where(o=>o.idCategoria == idCategoria).FirstOrDefault();
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Usuario");
        }


        [Authorize]
        [HttpGet]
        public IActionResult VerCursos()
        {

            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Usuarios.Where(o => o.usuario == claim.Value).First();
            ViewBag.UsuarioLogeado = user;

            var cursos = cnx.Matriculas.Where(o => o.idUsuario == user.idUsuario).Include("curso.profesor").ToList();
            ViewBag.CursosInscritos = cursos;

            return View();
        }




        [HttpPost]
        public ActionResult AgregarCurso(int idCurso, int idUsuario)
        {
            var matriculado = cnx.Matriculas.Where(o => o.idCurso == idCurso &&
            o.idUsuario == idUsuario).FirstOrDefault();

            if (matriculado != null)
            {
                var nombrecurso = cnx.Cursos.Where(o => o.idCurso == idCurso).First();
                TempData["AgregarCurso"] = "Usted ya se encuentra inscrito en <<" + nombrecurso.nombre + ">>"; 
            }
            else
            {
                //obtener usuario logeado
                var claim = HttpContext.User.Claims.FirstOrDefault();
                var user = cnx.Usuarios.Where(o => o.usuario == claim.Value).First();

                //obtener curso
                var curso = cnx.Cursos.Where(o => o.idCurso == idCurso).FirstOrDefault();


                var matricula = new Matricula();

                matricula.idUsuario = idUsuario;
                matricula.idCurso = idCurso;

                matricula.fechaMatricula = DateTime.Now;
                matricula.estadoMatricula = "PENDIENTE CONFIRMACION";
                matricula.codigoMatricula = "ORACON-" + user.usuario.ToUpper() + "-" + curso.nombre.ToUpper();

                matricula.promedio = 0.0m;

                cnx.Matriculas.Add(matricula);
                cnx.SaveChanges();


                var pago = new Pago();
                var matriculafinal = cnx.Matriculas.AsEnumerable().OrderByDescending(o => o.idMatricula).FirstOrDefault();
                pago.idMatricula = matriculafinal.idMatricula;
                pago.estado = "SIN PAGO";
                pago.fechaPago = DateTime.Now;
                pago.monto = cnx.Cursos.Where(o => o.idCurso == idCurso).FirstOrDefault().precio;
                cnx.Pagos.Add(pago);
                cnx.SaveChanges();
            }



            return RedirectToAction("IndexUsuario", "Usuario");
        }



        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string usuario, string passwd)
        {
            var user = cnx.Usuarios.Where(o => o.usuario == usuario && CreateHash(passwd) == o.passwd).FirstOrDefault();
            if (user != null)
            {
                //Guardamos el claim
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, usuario)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Login");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(claimsPrincipal);

                return RedirectToAction("IndexUsuario", "Usuario");
            }
            else
            {

                TempData["LoginUsuario"] = "Usuario y/o password incorrectos...";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        public List<string> validaciones(string nombres, string apellidos, DateTime fechaNac, string correo, string passwd, string usuario)
        {
            List<string> validaciones = new List<string>();
            if (nombres == null)
            {
                validaciones.Add("El nombre es obligatorio.");

            }
            else
            {
                if (nombres.Length <= 3 || nombres == "")
                {
                    validaciones.Add("El nombre de usuario no es valido. (min. 4 caracteres).");
                }
            }
            if (apellidos == null)
            {
                validaciones.Add("Los apellidos son obligatorios.");

            }
            else
            {
                if (apellidos.Length <= 3 || apellidos == "")
                {
                    validaciones.Add("Los apellidos del usuario no son validos. (min. 4 caracteres).");
                }
            }
            DateTime error = new DateTime(01, 01, 01);
            if (fechaNac == error)
            {
                validaciones.Add("La fecha de nacimiento es un campo obligatorio.");

            }
            else
            {
               
                if (fechaNac > DateTime.Now)
                {
                    validaciones.Add("La fecha actual es menor a la fecha de nacimiento.");
                }
            }
            if (correo == null)
            {
                validaciones.Add("El correo electronico es un campo obligatorio.");
            }
            else
            {
                if (correo == "")
                {
                    validaciones.Add("Verifique el formato del correo. Permitidos: hotmail, gmail, yahoo, outlook.");
                }
            }
            if ( passwd == null)
            {
                validaciones.Add("La contraseña del usuario es un campo obligatiorio.");

            }
            else
            {
                if (passwd.Length <= 3 || passwd == "")
                {
                    validaciones.Add("La contraseña del usuario debe contar minimo con 4 caracteres.");

                }
            }
            if (cnx.Usuarios.Where(o => o.usuario == usuario).FirstOrDefault() != null)
            {
                validaciones.Add("El nombre de usuario ya existe.");

            }
            else
            {
                if ( usuario == null)
                {
                    validaciones.Add("El nombre de usuario es un campo obligatorio.");

                }
                else
                {
                    if (usuario.Length <= 3 || passwd == "")
                    {
                        validaciones.Add("El nombre de usuario de contener minimo 4 caracteres).");

                    }
                }
            }
            return validaciones;
        }


        [HttpPost]
        public ActionResult Create(string nombres, string apellidos, DateTime fechaNac, string correo, string passwd, string usuario)
        {

            var usuarios = cnx.Usuarios.ToList();
            Usuario u = new Usuario();
            bool flag = false;
            List<string> listValidaciones = validaciones(nombres, apellidos, fechaNac, correo, passwd, usuario);

    
            if(listValidaciones.Count == 0 || listValidaciones == null)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }


            if (flag && cnx.Usuarios.Where(o => o.usuario == usuario).FirstOrDefault() == null)
            {
                u.nombres = nombres;
                u.apellidos = apellidos;
                u.fechaNac = fechaNac;
                u.correo = correo;
                u.passwd = CreateHash(passwd);
                u.usuario = usuario;
                cnx.Usuarios.Add(u);
                cnx.SaveChanges();

                return RedirectToAction("Login", "Usuario");
            }

            

            string mensaje = "";

            foreach(var i in listValidaciones)
            {
                mensaje = mensaje + i +"*    ";  
            }

            TempData["CreateUsuario"] = mensaje;
            return View();

        }

        private string CreateHash(string input)
        {
            var sha = SHA256.Create();
            input += configuration.GetValue<string>("Token");
            var hash = sha.ComputeHash(Encoding.Default.GetBytes(input));

            return Convert.ToBase64String(hash);
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


        public ActionResult VerModulosCurso(int idCurso)
        {
            //obtener curso por id con sus modulos
            var curso = cnx.Cursos.Where(o => o.idCurso == idCurso).Include("modulos.videos")
                .Include("modulos.documentos").Include("modulos.enlaces").Include("modulos.tareas").FirstOrDefault();

            //enviamos el curso
            ViewBag.Curso = curso;

            //enviamos la lista de modulos a a vista
            ViewBag.Modulos = curso.modulos.ToList();

            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Usuarios.Where(o => o.usuario == claim.Value).First();
            ViewBag.UsuarioLogeado = user;


            //obtenemos y enviamos pago
            var matricula = cnx.Matriculas.Where(o => o.idUsuario == user.idUsuario && o.idCurso == idCurso)
                .FirstOrDefault();
            ViewBag.Pago = cnx.Pagos.Where(o => o.idMatricula == matricula.idMatricula).FirstOrDefault();


            return View();
        }

        public ActionResult VerMaterialModulo(int idModulo)
        {
            // obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Usuarios.Where(o => o.usuario == claim.Value).First();
            ViewBag.UsuarioLogeado = user;

            //obtener modulo
            var modulo = cnx.Modulos.Where(o => o.idModulo == idModulo).Include(o => o.videos)
                .Include(o => o.documentos).Include(o => o.enlaces).Include("tareas.entregas").FirstOrDefault();

            ViewBag.Modulo = cnx.Modulos.Where(o => o.idModulo == idModulo).Include(o => o.videos)
                .FirstOrDefault();

            ViewBag.TareasPresentadas = cnx.Entregas.Where(o=>o.idUsuario == user.idUsuario).Include(o=>o.tarea).ToList();

            //videos del modulo
            ViewBag.Videos = modulo.videos.ToList();
            //Documentos del modulo
            ViewBag.Documentos = modulo.documentos.ToList();
            //Enlaces Externos
            ViewBag.Enlaces = modulo.enlaces.ToList();
            //Tareas pendientes
            ViewBag.Tareas = modulo.tareas.ToList();


            return View();
        }

        public bool validarCurso(int idCurso,int idUsuario)
        {
            bool x = false;

            var user = cnx.Usuarios.Where(o => o.idUsuario == idUsuario)
                .Include(o => o.cursos).First();

            if (user.cursos.Where(o=>o.idCurso == idCurso) == null)
            {
                x = true;
            }


            return x;
        }

        [HttpGet]
        public ActionResult EnviarTarea(int idTarea)
        {
            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Usuarios.Where(o => o.usuario == claim.Value).FirstOrDefault();
            ViewBag.UsuarioLogeado = user;

            ViewBag.IdTarea = idTarea;

            return View();
        }
        [HttpPost]
        public ActionResult EnviarTarea(int idTarea, IFormFile documento, string observacion, string estado)
        {

            //obtener usuario logeado
            var claim = HttpContext.User.Claims.FirstOrDefault();
            var user = cnx.Usuarios.Where(o => o.usuario == claim.Value).FirstOrDefault();
            ViewBag.UsuarioLogeado = user;

            var entrega = new Entrega();
            entrega.idTarea = idTarea;

            entrega.documento = GetByteArrayFromDocumento(documento);
            entrega.fechaEntrega = DateTime.Now;
            entrega.observacion = observacion;
            entrega.estado = "Pendiente";
            entrega.idUsuario = user.idUsuario;
            cnx.Entregas.Add(entrega);
            cnx.SaveChanges();


            var tarea = cnx.Tareas.Where(o => o.idTarea == idTarea).FirstOrDefault();

            return RedirectToAction("VerMaterialModulo", "Usuario", new { idModulo = tarea.idModulo });

        }
    }
}
