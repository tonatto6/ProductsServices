using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET.DTO
{
    public class ProductsDTO
    {
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo Descripción es requerido")]
        public string Description { get; set; }

        [Range(1,10000,
            ErrorMessage = "El precio debe estar entre {1} y {2}")]
        public double Price{ get; set; }

        [Required(ErrorMessage = "El campo SKU es requerido")]
        public string SKU { get; init; }
    }
}
