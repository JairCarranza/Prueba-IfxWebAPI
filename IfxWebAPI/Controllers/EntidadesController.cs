using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IfxWebAPI.Contents;
using IfxWebAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using IfxWebAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IfxWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class EntidadesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EntidadesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;//Inyeccion de instancia del applicationDbContext
            this.mapper = mapper;//Inyeccion de dependencia de mapper para EntidadDTO

        }

        //Devuelve un listado de entidades on un action result
        [HttpGet]
        public ActionResult<IEnumerable<Entidad>> Get()
        {
            //return context.Entidades.ToList();
            return context.Entidades.Include(x => x.Empleados).ToList();
        }


        //Consulta Entidades por Id
        [HttpGet("{id}", Name = "ObtenerEntidad")]
        public async Task<ActionResult<EntidadDTO>> Get(int id)
        {
            var entidad = await context.Entidades.Include(x => x.Empleados).FirstOrDefaultAsync(x => x.ID == id);
            if (entidad == null)
            {
                return NotFound();//Devuelve 404
            }

            var entidadDTO = mapper.Map<EntidadDTO>(entidad);

            return entidadDTO;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]//Requiere Autorizacion
        public async Task<ActionResult> Post([FromBody] Entidad entidades)
        {
            context.Entidades.Add(entidades);//Agrega Autor en Base de datos
            await context.SaveChangesAsync();
            var entidadDTO = mapper.Map<EntidadDTO>(entidades);
            return new CreatedAtRouteResult("ObtenerEntidad", new { id = entidades.ID }, entidadDTO);//devuelve 201 created
        }


        //Actualiza Recurso Entidad
        [HttpPut("{id}")]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]//Requiere Autorizacion
        public async Task<ActionResult> Put(int id, [FromBody] Entidad Value)//Parametros Id de la URL y Entidades del cuerpo
        {
            if (id != Value.ID) //Si no coincide el id con el parametro se retorna un bad request
            {
                return BadRequest();
            }

            context.Entry(Value).State = EntityState.Modified;//Se actulaiza el registro y se marca como dificado
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]//Requiere Autorizacion
        public ActionResult<Entidad> Delete(int id) //Recibe parámetro de la URL
        {
            var entidad = context.Entidades.FirstOrDefault(x => x.ID == id); //Se busca entidad en la base de datos

            if (entidad == null)
            {
                return NotFound();//Retorna 404
            }

            context.Entidades.Remove(entidad);//Marca entidad como borrado
            context.SaveChanges();
            return entidad;
        }



    }
}
