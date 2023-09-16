using API_NET.DTO;
using API_NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET
{
    public static class Utils
    {
        public static ProductsDTO transformToProductsDTO(this Products product)
        {
            return new ProductsDTO
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                SKU = product.SKU
            };
        } 

        public static UserApiDTO transformToUserApiDTO(this UserApi userApi)
        {
            return new UserApiDTO
            {
                UserApi = userApi.User,
                Token = userApi.Token
            };
        }
    }
}
