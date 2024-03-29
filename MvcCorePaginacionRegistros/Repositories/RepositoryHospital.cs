﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros.Data;
using MvcCorePaginacionRegistros.Models;

namespace MvcCorePaginacionRegistros.Repositories
{
    #region porcedures 
    //create view V_DEPARTAMENTOS_INDIVIDUAL
    //as
    //	select CAST(
    //    ROW_NUMBER() over (ORDER BY DEPT_NO) AS INT) AS POSICION,
    //    ISNULL(DEPT_NO, 0)AS DEPT_NO, DNOMBRE, LOC FROM DEPT
    //go

    //SELECT* FROM V_DEPARTAMENTOS_INDIVIDUAL WHERE POSICION = 1

    //    create procedure SP_GRUPO_DEPARTAMENTOS(@posicion int)
    //as
    //	select DEPT_NO, DNOMBRE, LOC from V_DEPARTAMENTOS_INDIVIDUAL where POSICION >= @posicion and POSICION<(@posicion + 2)
    //go

    //exec SP_GRUPO_DEPARTAMENTOS 1

    //create view V_GRUPO_EMPLEADOS
    //as
    //	select CAST(
    //    ROW_NUMBER() over (ORDER BY EMP_NO) AS INT) AS POSICION,
    //    ISNULL(EMP_NO, 0)AS EMP_NO, APELLIDO, OFICIO, SALARIO  FROM EMP
    //go


    //select* from emp


    //    SELECT* FROM V_GRUPO_EMPLEADOS WHERE POSICION = 1

    //    create procedure SP_GRUPO_EMPLEADOS(@posicion int)
    //as
    //	select EMP_NO, APELLIDO, OFICIO, SALARIO from V_GRUPO_EMPLEADOS where POSICION >= @posicion and POSICION<(@posicion + 2)
    //go


    //exec SP_GRUPO_EMPLEADOS 1
    #endregion
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Departamento>> GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @posicion";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            var consulta = this.context.Departamentos.FromSqlRaw(sql, pamPosicion);
            return await consulta.ToListAsync();
        }

        public async Task<List<VistaDepartamento>> GetGrupoVistaDepartamentoAsync(int posicion)
        {
            //SELECT* FROM V_DEPARTAMENTOS_INDIVIDUAL WHERE POSICION >= 1 AND POSICION< 3
            var consulta = from datos in this.context.VistaDepartamentos where datos.Posicion >= posicion && datos.Posicion < (posicion + 2) select datos;
            return await consulta.ToListAsync();
        }

        public async Task<int> GetNumeroRegistrosVistaDepartamentos()
        {
            return await this.context.VistaDepartamentos.CountAsync();
        }

        public async Task<VistaDepartamento> GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento vista = await this.context.VistaDepartamentos.Where(z => z.Posicion == posicion).FirstOrDefaultAsync();
            return vista;
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            return await this.context.Departamentos.ToListAsync();
        }

        public async Task <List<Empleado>> GetEmpleadosDepartamentoAsync(int idDepartamento)
        {
            var empleados = this.context.Empleados.Where(x => x.IdDepartamento == idDepartamento);

            if (empleados.Count() == 0)
            {
                return null;
            }
            else
            {
                return await empleados.ToListAsync();
            }
        }
    }
}
