﻿using MySql.Data.MySqlClient;
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

        public void Guardar(string Serial, string Modelo, DateTime Fecha, bool Pass)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("Guardar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@S", Serial);
                cmd.Parameters.AddWithValue("@Modelo", BuscarModelo(Modelo));
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
    }
}
