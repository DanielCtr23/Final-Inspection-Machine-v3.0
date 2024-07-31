using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Final_Inspection_Machine_v3._0.DBM
{
    public class DBMysql
    {
        private MySqlConnection con;
        private MySqlCommand cmd;
        private string server;
        private string database;
        private string uid;
        private string password;
        private static readonly object lockObject = new object();
        public DBMysql() 
        {
            server = "localhost";
            database = "bst_local";
            uid = "root";
            password = "Rheem";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            con = new MySqlConnection(connectionString);
        }


        //CREATE
        public void Guardar(string Serial, string Modelo, DateTime Fecha, bool Pass, bool Fail, bool RoscaPass, int RoscaCal, bool CrackPass, int CrackD, int CrackT, bool ResortePass, bool PilotBracketPass, int PilotBracketTipo,
            bool LargoPass, int LargoCal, bool SentidoPass, int SentidoCal, int SentidoTipo, bool NutPass, int NutCal, int NutTipo)
        {
            lock (lockObject) // Bloquear el método
            {
                try
                {
                    con.Open();
                    using (cmd = new MySqlCommand("GuardarPrueba", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@S", Serial);
                        cmd.Parameters.AddWithValue("@Modelo", Modelo);
                        cmd.Parameters.AddWithValue("@PASS", Pass);
                        cmd.Parameters.AddWithValue("@Fail", Fail);
                        cmd.Parameters.AddWithValue("@Fecha", Fecha);
                        cmd.Parameters.AddWithValue("@RoscaPass", RoscaPass);
                        cmd.Parameters.AddWithValue("@RoscaCal", RoscaCal);
                        cmd.Parameters.AddWithValue("@CrackPass", CrackPass);
                        cmd.Parameters.AddWithValue("@CrackD", CrackD);
                        cmd.Parameters.AddWithValue("@CrackT", CrackT);
                        cmd.Parameters.AddWithValue("@ResortePass", ResortePass);
                        cmd.Parameters.AddWithValue("@PilotBracketPass", PilotBracketPass);
                        cmd.Parameters.AddWithValue("@PilotBracketTipo", PilotBracketTipo);
                        cmd.Parameters.AddWithValue("@LargoPass", LargoPass);
                        cmd.Parameters.AddWithValue("@LargoCal", LargoCal);
                        cmd.Parameters.AddWithValue("@SentidoPass", SentidoPass);
                        cmd.Parameters.AddWithValue("@SentidoCal", SentidoCal);
                        cmd.Parameters.AddWithValue("@SentidoTipo", SentidoTipo);
                        cmd.Parameters.AddWithValue("@NutPass", NutPass);
                        cmd.Parameters.AddWithValue("@NutCal", NutCal);
                        cmd.Parameters.AddWithValue("@NutTipo", NutTipo);

                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                catch (Exception e)
                {
                    // Manejo de errores
                    MessageBox.Show(e.Message);
                    throw;
                }
            }
        }

        //READ

        public int TipoPLC()
        {
            int PLC = 0;
            try
            {
                con.Open();

                using (cmd = new MySqlCommand("PLC", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    PLC = int.Parse(cmd.ExecuteScalar().ToString());
                }

                con.Close();
            }
            catch (Exception)
            {
                PLC = 0;
            }

            return PLC;
        }


        public int Estacion(int n)
        {
            int E = 0;
            try
            {
                con.Open();

                using (cmd = new MySqlCommand("NumeroEstacion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@n", n);
                    E = int.Parse(cmd.ExecuteScalar().ToString());
                }

                con.Close();
            }
            catch (Exception)
            {

            }
            return E;
        }

        public string PilotBracketNombre(int n)
        {
            string Nombre = "";
            try
            {
                con.Open();

                using (cmd = new MySqlCommand("PilotBracket", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@n", n);
                    Nombre = (cmd.ExecuteScalar().ToString());
                }

                con.Close();
            }
            catch (Exception)
            {

            }
            return Nombre;
        }


        //Contador de Buenas y Malas
        public DataTable Contador()
        {
            DataTable dt = new DataTable();

                try
                {
                    con.Open();
                    using (cmd = new MySqlCommand("Contador", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                    con.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            return dt;
        }

        public int ContadorSerial(int Estacion)
        {
                try
                {
                    con.Open();
                    using (cmd = new MySqlCommand("ContadorEstacion", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Estacion", Estacion);
                        object result = cmd.ExecuteScalar();
                        con.Close();
                    if (result != null && int.TryParse(result.ToString(), out int count))
                        {
                            return count;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            
        }

        public DataTable Produccion(DateTime Inicio, DateTime Fin, String Formato)
        {
            DataTable dt = new DataTable();

                try
                {
                    con.Open();
                    using (cmd = new MySqlCommand("Produccion",con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Inicio", Inicio);
                        cmd.Parameters.AddWithValue("@Fin", Fin);
                        cmd.Parameters.AddWithValue("@Formato", Formato);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            

            return dt;
        }

        public DataTable ProduccionActual(String Formato)
        {
            DataTable dt = new DataTable();

                try
                {
                    con.Open();
                    using (cmd = new MySqlCommand("ProduccionActual", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Rango", Formato);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            return dt;
        }

        public DataTable ProduccionModelos(DateTime Inicio, DateTime Fin, String Formato)
        {
            DataTable dt = new DataTable();

                try
                {
                    con.Open();
                    using (cmd = new MySqlCommand("ProduccionModelos", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Inicio", Inicio);
                        cmd.Parameters.AddWithValue("@Fin", Fin);
                        cmd.Parameters.AddWithValue("@Formato", Formato);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            

            return dt;
        }

        public DataTable ProduccionModelos(String Horario)
        {
            DataTable dt = new DataTable();

                try
                {
                    con.Open();
                    using (cmd = new MySqlCommand("ProduccionModelosTurno", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Turno", Horario);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            

            return dt;
        }

        public DataTable TiempoCiclo(String Horario)
        {
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                using (cmd = new MySqlCommand("TiempoCiclo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Turno", Horario);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            return dt;
        }

        public DataTable Detalles(string serial, int? modelo, int? estacion, DateTime? Inicio, DateTime? Fin, bool? Pass, bool? Fail)
        {
            DataTable dt = new DataTable();

                try
                {
                    con.Open();
                    using (cmd = new MySqlCommand("Detalles", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@S", serial);
                        cmd.Parameters.AddWithValue("@Estacion", estacion);
                        cmd.Parameters.AddWithValue("@Modelo", modelo);
                        cmd.Parameters.AddWithValue("@Inicio", Inicio);
                        cmd.Parameters.AddWithValue("@Fin", Fin);
                        cmd.Parameters.AddWithValue("@Pass", Pass);
                        cmd.Parameters.AddWithValue("@Fail", Fail);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                    con.Close() ;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            

            return dt;
        }

        public DataTable Detalle(int id)
        {
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                using (cmd = new MySqlCommand("Detalle", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@S", id);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            return dt;
        }

        public DataTable ObtenerModelos()
        {
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                using (cmd = new MySqlCommand("ObtenerModelos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }


            return dt;
        }


        //UPDATE
        public void Guardar(string Serial, DateTime Fecha, bool Pass, bool Fail, bool TaponPass,
            int TaponCal, bool EtiquetaPass, int EtiquetaCal)
        {
            lock (lockObject) // Bloquear el método
                try
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand("ActualizarPrueba", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@S", Serial);
                        cmd.Parameters.AddWithValue("@PASS", Pass);
                        cmd.Parameters.AddWithValue("@Fail", Fail);
                        cmd.Parameters.AddWithValue("@Fecha", Fecha);
                        cmd.Parameters.AddWithValue("@TaponPass", TaponPass);
                        cmd.Parameters.AddWithValue("@TaponCal", TaponCal);
                        cmd.Parameters.AddWithValue("@EtiquetaPass", EtiquetaPass);
                        cmd.Parameters.AddWithValue("@EtiquetaCal", EtiquetaCal);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
        }

        public void ResetContadores()
        {
            try
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("ResetContadores", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                con.Close();

            }
            catch (Exception)
            {

                throw;
            }
        }


        //DELETE
    }
}
