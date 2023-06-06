using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sistema.Venta.BLL.Interfaces;
using Firebase.Auth;
using Firebase.Storage;
using SistemaVentas.Entity;
using SistemaVenta.DAL.Interfaces;

namespace Sistema.Venta.BLL.Implementacion
{
    public class FirebaseService : IFirebaseService
    {

        private readonly IGenericRepository<Configuracion> _repositorio;


        public FirebaseService(IGenericRepository<Configuracion> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<string> SubirStorage(Stream StreamArchivo, string CarpetaDestino, string NombreArchivo)
        {
            string UrlImagen = "";

            try 
            {
                IQueryable<Configuracion> query = await _repositorio.Consultar(c => c.Recurso.Equals("FireBase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                //token de cancelacion
                var cancellation = new CancellationTokenSource();

                //creamos una tarea que ejecute el servicio de firebase storage
                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions //opciones de firebase
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true //si ocurre un error que lo cancele
                    })
                    .Child(Config[CarpetaDestino]) //creamos las carpetas
                    .Child(NombreArchivo)  //creamos nombre de archivo
                    .PutAsync(StreamArchivo, cancellation.Token); // PutAsync copiamos el archivo como un formato de Stream

                UrlImagen = await task;
            }
            catch 
            { 
                UrlImagen= "";
            }
            return UrlImagen;
        }
        public async Task<bool> EliminarStorage(string CarpetaDestino, string NombreArchivo)
        {
            try
            {
                IQueryable<Configuracion> query = await _repositorio.Consultar(c => c.Recurso.Equals("FireBase_Storage"));

                Dictionary<string, string> Config = query.ToDictionary(keySelector: c => c.Propiedad, elementSelector: c => c.Valor);

                var auth = new FirebaseAuthProvider(new FirebaseConfig(Config["api_key"]));
                var a = await auth.SignInWithEmailAndPasswordAsync(Config["email"], Config["clave"]);

                //token de cancelacion
                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    Config["ruta"],
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child(CarpetaDestino)
                    .Child(NombreArchivo)
                    .DeleteAsync(); // Eliminar el archivo de nuestro servicio

                 await task;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
