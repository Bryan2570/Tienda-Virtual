using System.Reflection;
using System.Runtime.Loader;

namespace SistemaVenta.AplicacionWeb.Utilidades.Extensiones
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {

        //ESTE ARCHIVO NOS PERMITE DESCARGAR CON EXTENSIONES EXTERNAS
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }



    }
}
