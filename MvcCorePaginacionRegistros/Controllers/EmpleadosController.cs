using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros.Models;
using MvcCorePaginacionRegistros.Repositories;

namespace MvcCorePaginacionRegistros.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryHospital repo;

        public EmpleadosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> EmpleadosDepartamento()
        {
            List<Departamento> departamentos = await this.repo.GetDepartamentosAsync();
            return View(departamentos);
        }
        public async Task<IActionResult> DepartamentoEmpleados(int iddepartamento)
        {
            List<Empleado> emp = await this.repo.GetEmpleadosDepartamentoAsync(iddepartamento);
            return View(emp);

        }
    }
}
