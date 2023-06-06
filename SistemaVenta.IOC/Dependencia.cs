using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Implementacion;
using SistemaVenta.DAL.Interfaces;
using Sistema.Venta.BLL.Interfaces;
using Sistema.Venta.BLL.Implementacion;
using SistemaVentas.Entity;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    {

        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            //añadimos referencias de la conexion
            services.AddDbContext<DBVENTAContext>(options =>
            {
                //leemos la cadena de conexion
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));//utilizamos trasiente porque es generico y no sabemos con que entidad estamos trabajando ni los valores
            services.AddScoped<IVentaRepository, VentaRepository>();

            //Dependencia del envio de correo
            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<IFirebaseService, FirebaseService>();


            services.AddScoped<IUtilidadesService, UtilidadesService>(); //Password
            services.AddScoped<IRolService, RolService>();


            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<INegocioServices, NegocioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();

            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<ITipoDocumentoVentaService, TipoDocumentoVentaService>();
            services.AddScoped<IVentaService, VentaService>();

            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<IMenuService, MenuService>();

        }
    }
}
