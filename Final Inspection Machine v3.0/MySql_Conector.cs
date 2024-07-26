using Final_Inspection_Machine_v3._0.UC;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Inspection_Machine_v3._0
{
    internal class MySql_Conector
    {
        private MySqlConnection con;
        private string server;
        private string database;
        private string uid;
        private string password;
        public MySql_Conector() 
        {
            try
            {
                server = "localhost";
                database = "bstfim";
                uid = "root";
                password = "Rheem";
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                con = new MySqlConnection(connectionString);
            }
            catch (Exception)
            {
                server = "localhost";
                database = "mydb";
                uid = "root";
                password = "Daniel123";
                string connectionString;
                connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
                con = new MySqlConnection(connectionString);
            }
        }

        public DataTable Produccion(DateTime Inicio, DateTime Fin, String Tipo)
        {
            DataTable produccion = new DataTable();
            using (con)
            {
                try
                {
                    using (var cmd = new MySqlCommand("Produccion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Inicio", Inicio);
                        cmd.Parameters.AddWithValue("@Fin", Fin);
                        cmd.Parameters.AddWithValue("@Formato", Tipo);

                        con.Open();

                        using (MySqlDataAdapter Adaptador = new MySqlDataAdapter(cmd))
                        {
                            Adaptador.Fill(produccion);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Log the exception, handle it, or rethrow it
                    throw;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return produccion;
        }

        public DataTable ProduccionModelo(DateTime Inicio, DateTime Fin, String Tipo)
        {
            DataTable produccion = new DataTable();
            using (con)
            {
                if (Inicio==null || Fin == null || Tipo == null)
                {
                    throw new ArgumentNullException("Los parametros son nulos");
                }

                try
                {
                    using (var cmd = new MySqlCommand("ProduccionModelo", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Inicio", Inicio);
                        cmd.Parameters.AddWithValue("@Fin", Fin);
                        cmd.Parameters.AddWithValue("@Formato", Tipo);

                        con.Open();

                        using (MySqlDataAdapter Adaptador = new MySqlDataAdapter(cmd))
                        {
                            Adaptador.Fill(produccion);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    // Log the exception, handle it, or rethrow it
                    throw;
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }
            return produccion;
        }

    }
}
