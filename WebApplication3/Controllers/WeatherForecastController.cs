using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.OracleClient;
using WebApplication3.Models;


namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class OracleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        string connectionString4 = "Data Source=(DESCRIPTION="
                          + "(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))"
                          + "(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = orcl2)));"
                          + "User id=Elian;Password=123;";

        string connectionString = "Data Source=(DESCRIPTION="
                          + "(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = adb.mx-queretaro-1.oraclecloud.com)(PORT = 1522)))"
                          + "(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = g5c675e2f4c5103_contabilidad_tpurgent.adb.oraclecloud.com)));"
                          + "User id=Admin;Password=Contabilidad2024;";
            
     
        //OracleConfiguration.WalletLocation= "C:\Users\SOPORTE\Downloads\wallet_contabilidad";
              


string connectionString5 = "Data Source=(DESCRIPTION="
                  + "(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = adb.mx-queretaro-1.oraclecloud.com)(PORT = 1522)))"
                  + "(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = g5c675e2f4c5103_contabilidad_tpurgent.adb.oraclecloud.com))"
                  + "(SECURITY = (SSL_SERVER_CERT_DN = \"CN=adb.mx-queretaro-1.oraclecloud.com,OU=Oracle ADB,OU=Oracle Cloud,O=Oracle Corporation,L=Redwood City,ST=California,C=US\"))"
                  + "(SSL_KEY_STORE = (SOURCE = (METHOD = URL)(METHOD_DATA = (URL =C://Users/SOPORTE/Downloads/Wallet_contabilidad;))))"
                  + ");"
                  + "User Id=Admin;Password=Contabilidad2024;";
        public OracleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        

        [HttpGet]
       public IActionResult GetAll()
        {
           /* string connectionString = "Data Source=(DESCRIPTION="
                 + "(ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521)))"
                 + "(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = orcl2)));"
                + "User id=Elian;Password=123;";*/

            /*string connectionString = _configuration.GetConnectionString("OracleConnection");*/

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Conexión exitosa a Oracle.");

                    // Ejecutar una consulta SELECT para obtener todos los productos
                    string sql = "SELECT * FROM CLIENTES";
                    using (OracleCommand cmd = new OracleCommand(sql, connection))
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        List<cliente> clientes = new List<cliente>();
                        while (reader.Read())
                        {
                            clientes.Add(new cliente
                            {
                                CLIEN_ID = reader.GetInt32(0),
                                NOMBRE = reader.GetString(1),
                                DIRECCION = reader.GetString(2),
                                TELEFONO = reader.GetString(3),
                                EMAIL = reader.GetString(4),
                                NIT = reader.GetString(5)
                            });
                        }
                        //connection.Close();
                        return Ok(clientes);
                       
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar a Oracle2: " + ex.Message);
                    return StatusCode(500, "Error al conectar a Oracle2: " + ex.Message);
                }

            }
            
        }

      



        [HttpPost]
        public IActionResult Insert(cliente cliente)
        {
            

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Conexión exitosa a Oracle.");

                    // Ejecutar una consulta INSERT para insertar un nuevo producto
                    string sql = "INSERT INTO CLIENTES (CLIEN_ID, NOMBRE, DIRECCION, TELEFONO, EMAIL, NIT) VALUES (:CLIEN_ID,:NOMBRE, :DIRECCION, :TELEFONO, :EMAIL, :NIT)";
                    using (OracleCommand cmd = new OracleCommand(sql, connection))
                    {
                        cmd.Parameters.Add(":Clien_ID", OracleDbType.Int64).Value = cliente.CLIEN_ID;
                        cmd.Parameters.Add(":Nombre", OracleDbType.Varchar2).Value = cliente.NOMBRE;
                        cmd.Parameters.Add(":Direccion", OracleDbType.Varchar2).Value = cliente.DIRECCION;
                        cmd.Parameters.Add(":Telefono", OracleDbType.Varchar2).Value = cliente.TELEFONO;
                        cmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = cliente.EMAIL;
                        cmd.Parameters.Add(":NIT", OracleDbType.Varchar2).Value = cliente.NIT;
                        int rowsInserted = cmd.ExecuteNonQuery();
                        
                        return Ok($"Se insertaron {rowsInserted} filas.");

                    }
                    //connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar a Oracle: " + ex.Message);
                    return StatusCode(500, "Error al conectar a Oracle: " + ex.Message);
                }
            }
        }



        [HttpPut("{Clien_ID}")]
        public IActionResult Update(int Clien_ID, cliente cliente)
        {
            

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Conexión exitosa a Oracle.");

                    // Ejecutar una consulta UPDATE para actualizar un producto
                    string sql = "UPDATE CLIENTES SET NOMBRE =:Nombre, DIRECCION =:Direccion, TELEFONO =:Telefono, EMAIL =:Email, NIT =:NIT WHERE CLIEN_ID =:Clien_ID";
                    using (OracleCommand cmd = new OracleCommand(sql, connection))
                    {
                        
                        cmd.Parameters.Add(":Nombre", OracleDbType.Varchar2).Value = cliente.NOMBRE;
                        cmd.Parameters.Add(":Direccion", OracleDbType.Varchar2).Value = cliente.DIRECCION;
                        cmd.Parameters.Add(":Telefono", OracleDbType.Varchar2).Value = cliente.TELEFONO;
                        cmd.Parameters.Add(":Email", OracleDbType.Varchar2).Value = cliente.EMAIL;
                        cmd.Parameters.Add(":NIT", OracleDbType.Varchar2).Value = cliente.NIT;
                        cmd.Parameters.Add(":Clien_ID", OracleDbType.Int32).Value = Clien_ID;
                        int rowsUpdated = cmd.ExecuteNonQuery();
                        
                        return Ok($"Se actualizaron {rowsUpdated} filas.");
                    }
                    //connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar a Oracle: " + ex.Message);
                    return StatusCode(500, "Error al conectar a Oracle: " + ex.Message);
                }
            }
        }

       [HttpDelete("{Clien_ID}")]
        public IActionResult Delete(int Clien_ID)
        {
            

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Conexión exitosa a Oracle.");

                    // Ejecutar una consulta DELETE para eliminar un producto
                    string sql = "DELETE FROM CLIENTES WHERE CLIEN_ID =:Clien_ID";
                    using (OracleCommand cmd = new OracleCommand(sql, connection))
                    {
                        cmd.Parameters.Add(":Clien_ID", OracleDbType.Int32).Value = Clien_ID;
                        int rowsDeleted = cmd.ExecuteNonQuery();
                        return Ok($"Se eliminaron {rowsDeleted} filas.");

                    }
                    //connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al conectar a Oracle: " + ex.Message);
                }
            }

            return Ok("Conexión realizada exitosamente.");
        }
    }

}
