﻿using Microsoft.AspNetCore.Mvc;

using SistemaVenta.AplicacionWeb.Models.ViewModels;
using Sistema.Venta.BLL.Interfaces;
using SistemaVentas.Entity;

using System.Security.Claims; //Seguridad de autenticacion por cokiies
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;

        public AccesoController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User; //guardamos la sesion del usuario

            if (claimUser.Identity.IsAuthenticated) { //validmaos si existe un usuario o no 
                return RedirectToAction("Index", "Home");           
            }
            return View();
        }

        public IActionResult RestablecerClave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMUsuarioLogin modelo)
        {
            Usuario usuario_encontrado = await _usuarioServicio.ObtenerPorCredenciales(modelo.Correo, modelo.Clave);

            if (usuario_encontrado == null) {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }
            ViewData["Mensaje"] = null;

            if (modelo.Clave == null) {
                ViewData["Mensaje"] = "Por favor escribir contraseña";
                return View();
            }

            //Lista de claims nos sirve para guardar la información de un usuario

            List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuario_encontrado.IdRol.ToString()),
                new Claim("UrlFoto", usuario_encontrado.UrlFoto),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); //tipo de autenticación

            AuthenticationProperties properties = new AuthenticationProperties() //propiedades
            {
                AllowRefresh = true, //permite el refrescado de la pagina
                IsPersistent = modelo.MantenerSesion     // persiste la sesion y la saca del modelo   
            };

            //registramos el logueo
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties);

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult> RestablecerClave(VMUsuarioLogin modelo)
        {
            try
            {
                string urlPlantillaCorreo = $"{this.Request.Scheme}://{this.Request.Host}/Plantilla/RestablecerClave?clave=[clave]";

                bool resultado = await _usuarioServicio.RestablecerClave(modelo.Correo, urlPlantillaCorreo);

                if (resultado)
                {
                    ViewData["Mensaje"] = "Listo!, la contrasela se ha restablecido por favor revise el correo.";
                    ViewData["MensajeError"] = null;
                }
                else {
                    ViewData["MensajeError"] = "Tenemos problemas. Por favor inténtelo mas tarde. ";
                    ViewData["Mensaje"] = null;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

    }
}
