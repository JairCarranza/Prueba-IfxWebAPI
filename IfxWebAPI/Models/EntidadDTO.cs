using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IfxWebAPI.Models
{
    public class EntidadDTO
    {
        public int ID { get; set; }
        [Required]
        public string Nombre { get; set; }
        public List<EmpleadoDTO> Empleados { get; set; }
    }
}
