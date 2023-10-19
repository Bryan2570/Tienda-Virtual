
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace SistemaVenta.AplicacionWeb.Utilidades.ViewComponents
{
    public class MenuUsuarioViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsPrincipal claimUser = HttpContext.User; //obtenemos el contexto del usuario que se encuentra logueado

            string nombreUsuario = "";
            string urlFotoUsuario = "";

            if (claimUser.Identity.IsAuthenticated) //Validmos si existe o no el usuario
            {
                nombreUsuario = claimUser.Claims
                    .Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();

                urlFotoUsuario = ((ClaimsIdentity)claimUser.Identity).FindFirst("UrlFoto").Value; //hacemos un casteo para convertir a claims identity
            }

            ViewData["nombreUsuario"] = nombreUsuario;
            ViewData["urlFotoUsuario"] = urlFotoUsuario;

            return View();  

            }
        }
}
