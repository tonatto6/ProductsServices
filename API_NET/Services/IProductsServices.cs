using API_NET.DTO;
using API_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET.Services
{
    public interface IProductsServices
    {
        IEnumerable<Products> ListProducts(string SKU);

        Products seekProduct(string sku);

        void addProduct(Products product);

        void updateProduct(ProductsDTO product);

        void deleteProduct(string SKU);
    }
}
