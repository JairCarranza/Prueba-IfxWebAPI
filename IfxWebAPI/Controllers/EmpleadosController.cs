using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IfxWebAPI.Contents;
using IfxWebAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IfxWebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    
    public class EmpleadosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public EmpleadosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        //Devuelve un listado de empleados on un action result
        [HttpGet]
        public ActionResult<IEnumerable<Empleado>> Get()
        {
            return context.Empleados.Include(x => x.Entidad).ToList();//Adicional de los empleados trae la entidad
        }

        //Consulta Empleados por Id
        [HttpGet("{id}", Name = "ObtenerEmpleado")]
        public async Task<ActionResult<Empleado>> Get(int id)
        {
            //Valida si existe Id igual al parámetro recibido.
            var empleado = await context.Empleados.Include(x => x.Entidad).FirstOrDefaultAsync(x => x.Id == id);
            if (empleado == null)
            {
                return NotFound();//Devuelve 404
            }

            return empleado;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]//Requiere Autorización
        public ActionResult Post([FromBody] Empleado empleado)
        {
            context.Empleados.Add(empleado);//Agrega Empleado en Base de datos
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerEmpleado", new { id = empleado.Id }, empleado);//devuelve 201 created
        }

        //Actualiza Recurso Empleado
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]//Requiere Autorizacion
        public ActionResult Put(int id, [FromBody] Empleado empleado)//Parametros Id de la URL y Empleados del cuerpo
        {
            if (id != empleado.Id) //Si no coincide el id con el parametro se retorna un bad request
            {
                return BadRequest();
            }

            context.Entry(empleado).State = EntityState.Modified;//Se actulaiza el registro y se marca como dificado
            context.SaveChanges();
            return Ok();
        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]//requiere autorizacion
        public ActionResult<Empleado> Delete(int id) //Recibe parámetro de la URL
        {
            var empleado = context.Empleados.FirstOrDefault(x => x.Id == id); //Se busca entidad en la base de datos

            if (empleado == null)
            {
                return NotFound();//Retorna 404
            }

            context.Empleados.Remove(empleado);//Marca entidad como borrado
            context.SaveChanges();
            return empleado;
        }

    }
}
