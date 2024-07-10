using Final_Inspection_Machine_v3._0.Pages;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Inspection_Machine_v3._0
{
    public class DB
    {
        private MySqlConnection con;

        private string server;
        private string database;
        private string uid;
        private string password;

        public DB()
        {
            Inicializar();
        }

        private void Inicializar()
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
                con.Open();
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
                con.Open();
            }
        }

        //Guardado del 3 Pass---
        public void Guardar(string Serial, string Modelo, DateTime Fecha, bool Pass, bool RoscaPass, int RoscaCal, bool CrackPass, int CrackD, int CrackT,
            bool LargoPass, int LargoCal, bool SentidoPass, int SentidoCal, int SentidoTipo, bool NutPass, int NutCal, int NutTipo)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("Guardar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@S", Serial);
                cmd.Parameters.AddWithValue("@Modelo", BuscarModelo(Modelo));
                cmd.Parameters.AddWithValue("@PASS", Pass);
                cmd.Parameters.AddWithValue("@Fecha", Fecha);
                cmd.Parameters.AddWithValue("@Rosca_Pass", RoscaPass);
                cmd.Parameters.AddWithValue("@Rosca_Cal", Fecha);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Seria un Update
        public void Guardar(string Serial, DateTime Fecha, bool Pass, bool TaponPass, 
            int TaponCal, bool EtiquetaPass)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("GuardarCompleto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@S", Serial);
                cmd.Parameters.AddWithValue("@PASS", Pass);
                cmd.Parameters.AddWithValue("@Fecha", Fecha);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int BuscarModelo(string Modelo)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("BuscarModelo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Modelo", Modelo);
                return int.Parse(cmd.ExecuteScalar().ToString());

            }
            catch (Exception)
            {

                throw;
            }

        }

        public int ObtenerMalas()
        {
            int malas;
            MySqlCommand cmd = new MySqlCommand("Malas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            malas = int.Parse(cmd.ExecuteScalar().ToString());
            return malas;
        }
        public int ObtenerBuenas()
        {
            int buenas;
            MySqlCommand cmd = new MySqlCommand("Buenas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            buenas = int.Parse(cmd.ExecuteScalar().ToString());
            return buenas;
        }

        public int ObtenerContador1()
        {
            int Contador1;
            MySqlCommand cmd = new MySqlCommand("ContadorE1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            Contador1 = int.Parse(cmd.ExecuteScalar().ToString());
            return Contador1;
        }

        public int ObtenerContador2()
        {
            int Contador2;
            MySqlCommand cmd = new MySqlCommand("ContadorE2", con);
            cmd.CommandType = CommandType.StoredProcedure;
            Contador2 = int.Parse(cmd.ExecuteScalar().ToString());
            return Contador2;
        }

        public void ResetContadores()
        {
            MySqlCommand cmd = new MySqlCommand("ResetContadores", con);
            cmd.CommandType= CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
        }
        public DataTable Produccion(int op)
        {
            DataTable produccion = new DataTable();
            MySqlCommand cmd;

            DateTime fecha;

            if (DateTime.Now.Hour<7)
            {
                fecha = DateTime.Now.AddDays(-1);
            }
            else
            {
                fecha = DateTime.Now;
            }

            if (op == 1)
            {
                cmd = new MySqlCommand("ProduccionT1N", con);
                cmd.CommandType = CommandType.StoredProcedure;
                produccion.Load(cmd.ExecuteReader());
            }
            else if (op == 2)
            {
                cmd = new MySqlCommand("ProduccionT1E", con);
                cmd.CommandType = CommandType.StoredProcedure;
                produccion.Load(cmd.ExecuteReader());
            }
            else if (op == 3)
            {
                cmd = new MySqlCommand("ProduccionT2N", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@F", fecha);
                produccion.Load(cmd.ExecuteReader());
            }
            else if (op == 4)
            {
                cmd = new MySqlCommand("ProduccionT2E", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@F", fecha);
                produccion.Load(cmd.ExecuteReader());
            }

            return produccion;
        }

        public DataTable ProduccionDiaria()
        {

            DataTable produccion = new DataTable();
            MySqlCommand cmd;
            cmd = new MySqlCommand("ProduccionDiaria", con);
            cmd.CommandType = CommandType.StoredProcedure;
            produccion.Load(cmd.ExecuteReader());

            return produccion;
        }

        public DataTable ProduccionSemanal()
        {

            DataTable produccion = new DataTable();
            MySqlCommand cmd;
            cmd = new MySqlCommand("ProduccionSemanal", con);
            cmd.CommandType = CommandType.StoredProcedure;
            produccion.Load(cmd.ExecuteReader());

            return produccion;
        }

        public DataTable ProduccionMensual()
        {

            DataTable produccion = new DataTable();
            MySqlCommand cmd;
            cmd = new MySqlCommand("ProduccionMensual", con);
            cmd.CommandType = CommandType.StoredProcedure;
            produccion.Load(cmd.ExecuteReader());

            return produccion;
        }

        public int Produccion()
        {
            MySqlCommand cmd = new MySqlCommand("ProduccionHoraActual", con);
            cmd.CommandType = CommandType.StoredProcedure;
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public int Produccion(DateTime Inicio, DateTime Fin)
        {
            MySqlCommand cmd = new MySqlCommand("Produccion", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Inicio", Inicio);
            cmd.Parameters.AddWithValue("@Fin", Fin);
            return int.Parse(cmd.ExecuteScalar().ToString());
        }

        public double TiempoCiclo(DateTime Inicio, DateTime Fin)
        {
            MySqlCommand cmd = new MySqlCommand("TiempoCiclo", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Inicio", Inicio);
            cmd.Parameters.AddWithValue("@Fin", Fin);
            try
            {

                return double.Parse(cmd.ExecuteScalar().ToString());
            }
            catch (Exception)
            {

                return 0;
            }
        }

        public DataSet ModelosProducidos(DateTime Inicio, DateTime Fin)
        {
            DataSet ds = new DataSet(); 
            MySqlCommand cmd = new MySqlCommand("ProduccionModelos", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Inicio", Inicio);
            cmd.Parameters.AddWithValue("@Fin", Fin);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public DataTable DetallePruebas(string serial, int? modelo, int? estacion, DateTime? Inicio, DateTime? Fin, bool? Pass, bool? Fail)
        {
            DataTable detalles = new DataTable();

            MySqlCommand cmd = new MySqlCommand("DetallePruebas", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@S", serial);
            cmd.Parameters.AddWithValue("@Estacion", estacion);
            cmd.Parameters.AddWithValue("@Modelo", modelo);
            cmd.Parameters.AddWithValue("@Inicio", Inicio);
            cmd.Parameters.AddWithValue("@Fin", Fin);
            cmd.Parameters.AddWithValue("@Pass", Pass);
            cmd.Parameters.AddWithValue("@Fail", Fail); 
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(detalles);
            }
            catch (Exception)
            {

                throw;
            }

            return detalles;
        }

        public DataTable ObtenerModelos()
        {
            DataTable modelos = new DataTable();

            MySqlCommand cmd = new MySqlCommand("ObtenerModelos", con);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(modelos);
            }
            catch (Exception)
            {

                throw;
            }

            return modelos;
        }

    }
}
