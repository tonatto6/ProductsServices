using API_NET.DAL;
using API_NET.DTO;
using API_NET.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace API_NET.Services
{
    public class ProductsServices : IProductsServices
    {
        private string connectionString;
        private readonly ILogger<ProductsServices> log;

        public ProductsServices(DBAccess dbAccess, ILogger<ProductsServices> l)
        {
            connectionString = dbAccess.GetConnectionString;
            this.log = l;
        }

        private SqlConnection Conexion()
        {
            return new SqlConnection(connectionString);
        }

        public IEnumerable<Products> ListProducts(string SKU)
        {
            SqlConnection cnn = Conexion();
            SqlCommand command = null;
            List<Products> listProducts = new List<Products>();
            Products product;
            try
            {
                cnn.Open();
                command = cnn.CreateCommand();
                command.CommandText = "dbo.usp_Products_Seek_All";
                command.CommandType = CommandType.StoredProcedure;
                if (SKU is not null)
                {
                    command.Parameters.Add("@Filtro", SqlDbType.VarChar, 100).Value = SKU;
                }
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    product = new Products
                    {
                        Id = Convert.ToInt32(reader["Id"].ToString()),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDouble(reader["Price"].ToString()),
                        SKU = reader["SKU"].ToString()
                    };

                    listProducts.Add(product);
                }

                return listProducts;
            }
            catch (Exception ex)
            {
                throw new Exception("Se produjo un error al insertar un producto" + ex.ToString());
            }
            finally
            {
                command.Dispose();
                cnn.Close();
                cnn.Dispose();
            }
        }

        public Products seekProduct(string sku)
        {
            SqlConnection cnn = Conexion();
            SqlCommand command = null;
            Products product = null;
            try
            {
                cnn.Open();
                command = cnn.CreateCommand();
                command.CommandText = "dbo.usp_Products_Seek_By_SKU";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SKU", sku);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    product = new Products
                    {
                        Id = Convert.ToInt32(reader["Id"].ToString()),
                        Name = reader["Name"].ToString(),
                        SKU = reader["SKU"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = Convert.ToDouble(reader["Price"].ToString())
                    };
                }

                return product;
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message.ToString());
                throw new Exception(ex.ToString());
            }
            finally
            {
                command.Dispose();
                cnn.Close();
                cnn.Dispose();
            }
        }

        public void addProduct(Products product)
        {
            SqlConnection cnn = Conexion();
            SqlCommand command = null;
            try
            {
                cnn.Open();
                command = cnn.CreateCommand();
                command.CommandText = "dbo.usp_Products_Insert";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = product.Name;
                command.Parameters.Add("@SKU", SqlDbType.VarChar, 50).Value = product.SKU;
                command.Parameters.Add("@Description", SqlDbType.VarChar, 100).Value = product.Description;
                command.Parameters.Add("@Price", SqlDbType.Decimal).Value = product.Price;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message.ToString());
                throw new Exception("Se produjo un error al insertar un producto" + ex.ToString());
            }
            finally
            {
                command.Dispose();
                cnn.Close();
                cnn.Dispose();
            }
        }

        public void deleteProduct(string SKU)
        {
            SqlConnection cnn = Conexion();
            SqlCommand command = null;
            try
            {
                cnn.Open();
                command = cnn.CreateCommand();
                command.CommandText = "dbo.usp_Products_Delete";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SKU", SKU);
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message.ToString());
                throw new Exception(ex.ToString());
            }
            finally
            {
                command.Dispose();
                cnn.Close();
                cnn.Dispose();
            }
        }

        public void updateProduct(ProductsDTO product)
        {
            SqlConnection cnn = Conexion();
            SqlCommand command = null;
            try
            {
                cnn.Open();
                command = cnn.CreateCommand();
                command.CommandText = "dbo.usp_Products_Update";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SKU", product.SKU);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                log.LogError(ex.Message.ToString());
                throw new Exception(ex.ToString());
            }
            finally
            {
                command.Dispose();
                cnn.Close();
                cnn.Dispose();
            }
        }
    }
}
