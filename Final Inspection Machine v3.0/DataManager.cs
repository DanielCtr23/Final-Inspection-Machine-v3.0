using Final_Inspection_Machine_v3._0.DBM;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.AvalonDock.Controls;

namespace Final_Inspection_Machine_v3._0
{
    internal class DataManager
    {
        DBMysql dBMysql;
        public DataManager() 
        {
            dBMysql = new DBMysql();
        }

        //CREATE

        public void Guardar(string Serial, string Modelo, DateTime Fecha, bool Pass, bool Fail, bool RoscaPass, int RoscaCal, bool CrackPass, int CrackD, int CrackT, bool ResortePass, bool PilotBracketPass, int PilotBracketTipo,
            bool LargoPass, int LargoCal, bool SentidoPass, int SentidoCal, int SentidoTipo, bool NutPass, int NutCal, int NutTipo)
        {
            try
            {
                dBMysql.Guardar(Serial, Modelo, Fecha, Pass, Fail, RoscaPass, RoscaCal, CrackPass, CrackD, CrackT, ResortePass, PilotBracketPass, PilotBracketTipo,
             LargoPass,  LargoCal,  SentidoPass,  SentidoCal,  SentidoTipo,  NutPass,  NutCal, NutTipo);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //READ
        public DataTable Metas()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.Metas();
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int TipoPLC()
        {
            int PLC;

            try
            {
                PLC = dBMysql.TipoPLC();
            }
            catch (Exception)
            {

                throw;
            }

            return PLC;
        }

        public bool PBPermisivo()
        {
            bool PP;

            try
            {
                PP = dBMysql.PBPermisivo();
                return PP;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Estacion(int n)
        {
            try
            {
                return dBMysql.Estacion(n);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string PilotBracketNombre(int n)
        {
            try
            {
                return dBMysql.PilotBracketNombre(n);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable Contador()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.Contador();
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int ContadorSerial(int Estacion)
        {
            try
            {
                return dBMysql.ContadorSerial(Estacion);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable Produccion(DateTime Inicio, DateTime Fin, String Formato)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.Produccion(Inicio, Fin, Formato);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ProduccionActual(String Formato)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.ProduccionActual(Formato);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ProduccionModelos(DateTime Inicio, DateTime Fin, String Formato)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.ProduccionModelos(Inicio, Fin, Formato);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable ProduccionModelos( String Horario)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.ProduccionModelos(Horario);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable TiempoCiclo(String Horario)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.TiempoCiclo(Horario);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable Detalles(string serial, int? modelo, int? estacion, DateTime? Inicio, DateTime? Fin, bool? Pass, bool? Fail)
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.Detalles(serial, modelo, estacion, Inicio, Fin, Pass, Fail);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable Detalle(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = dBMysql.Detalle(id);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ObtenerModelos()
        {
            DataTable dt = new DataTable();

            try
            {
                dt = dBMysql.ObtenerModelos();
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        //UPDATE
        public void Guardar(string Serial, DateTime Fecha, bool Pass, bool Fail, bool TaponPass,
            int TaponCal, bool EtiquetaPass, int EtiquetaCal)
        {
            try
            {
                dBMysql.Guardar( Serial,  Fecha,  Pass,  Fail,  TaponPass,
                 TaponCal,  EtiquetaPass,  EtiquetaCal);
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
                dBMysql.ResetContadores();
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
