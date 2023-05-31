using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SistemaVenta.DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly DBVENTAContext _dbContext;

        public GenericRepository(DBVENTAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try 
            { 
                //especificamos la entidad que estamos trabajando , y que encuentre el primero si no que nos devuelva nulo
            TEntity entidad = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entidad; 
            }
            catch
            {
                throw;
            }
                        
        }

        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                //agregamos la entidad, guardamos de forma asincrona y retornamos la entidad que hemos creado
               _dbContext.Set<TEntity>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                //_dbContext.Update(entidad);
                _dbContext.Set<TEntity>().Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                //_dbContext.Remove(entidad);
                _dbContext.Set<TEntity>().Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }
        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null)
        {
            //lo primero que hacemos es validar si nuestro filtro es igual a nulo, entonces que devuelva la consulta a esa tabla o entidad
            // en caso de que el filtro exista o sea diferente de nulo, entonces necesitamos hacer un select a esa tabla con esos filtros
            IQueryable<TEntity> queryEntidad = filtro == null ? _dbContext.Set<TEntity>() : _dbContext.Set<TEntity>().Where(filtro);
            return queryEntidad;
         }
    }
}
