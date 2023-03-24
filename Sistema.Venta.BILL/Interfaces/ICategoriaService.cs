﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaVentas.Entity;

namespace Sistema.Venta.BLL.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> Lista();
        Task<Categoria> Crear(Categoria entidad);
        Task<Categoria> Editar(Categoria entidad);
        Task<bool> Eliminar(int idCategoria);


    }
}