using API_NET.DTO;
using API_NET.Models;
using API_NET.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET.Controllers
{
    [Route("products")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProductsController : ControllerBase
    {
        private readonly IProductsServices services;
        
        public ProductsController(IProductsServices _iproduct)
        {
            services = _iproduct;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<Products> listProducts(string SKU)
        {

            var productsList = services.ListProducts(SKU);
            return productsList;
        }

        [HttpGet("{SKU}")]
        [Authorize]
        public ActionResult<Products> getProductBySKU(string SKU)
        {
            var product = services.seekProduct(SKU);

            if(product is null)
            {
                return NotFound();
            }

            return product;
        }
    
        [HttpPost]
        [Authorize]
        public ActionResult<ProductsDTO> addProduct(ProductsDTO productDTO)
        {
            Products product = new Products
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
                SKU = productDTO.SKU,
            };

            services.addProduct(product);

            return product.transformToProductsDTO();
        }
    
        [HttpPut("{SKU}")]
        [Authorize]
        public ActionResult<ProductsDTO> updateProduct(string SKU, ProductUpdateDTO productUpdateDTO)
        {
            ProductsDTO product = new ProductsDTO
            {
                Name = productUpdateDTO.Name,
                SKU = SKU,
                Description = productUpdateDTO.Description,
                Price = productUpdateDTO.Price
            };

            services.updateProduct(product);
            return product;
        }
    
        [HttpDelete("{SKU}")]
        [Authorize]
        public ActionResult deleteProduct(string SKU)
        {
            services.deleteProduct(SKU);
            return NoContent();
        }
    }
}
