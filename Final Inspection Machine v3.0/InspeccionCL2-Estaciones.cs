﻿using IV3_Keyence;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IV3_Keyence.Estructuras;

namespace Final_Inspection_Machine_v3._0
{
    public partial class InspeccionCL2
    {
        Thread HiloPrincipal;
        Thread Estacion1, Estacion2;

        Estructuras.ResultadosCorrugado[] ResultadosE1 = new ResultadosCorrugado[8];
        Estructuras.ResultadosCorrugado[] ResultadosE2 = new ResultadosCorrugado[8];
        Estructuras.Orifice ResultadosOrifice1 = new Orifice();
        IV3 Corrugado1, Corrugado2, Orifice11, Orifice12, Orifice21, Orifice22;
        bool[] Fail = new bool[2];
        bool[] Pass = new bool[2];

        ManualResetEvent EsperarTaponE1, EsperarTaponE2, EsperarEtiquetaE1, EsperarEtiquetaE2;
        DB db = new DB();
        Etiquetadora etiquetadora;

        bool nutrojo, sinsentido, pilotbracket, resorte;
        string serial1, serial2;
        string modelo;

        int Contador1, Contador2;

        private void Ejecucion()
        {
            ResultadosE1 = new ResultadosCorrugado[8]; 
            ResultadosE2 = new ResultadosCorrugado[8];

            Fail[0] = false;
            Pass[0] = false;
            Fail[1] = false;
            Pass[1] = false;
            Dispatcher.Invoke(new Action(() => HabilitarBotones(false)));

            //ModeloBtn.IsEnabled = false;
            //RegresarBtn.IsEnabled = false;
            EsperarEtiquetaE1 = new ManualResetEvent(false);
            EsperarEtiquetaE2 = new ManualResetEvent(false);
            EsperarTaponE1 = new ManualResetEvent(false);
            EsperarTaponE2 = new ManualResetEvent(false);
            Contador1 = db.ObtenerContador1();
            Contador2 = db.ObtenerContador2();
            modelo = Com.ModeloSeleccionado();
            nutrojo = Com.NutRojo();
            pilotbracket = Com.PilotBracket();
            sinsentido = Com.SinSentido();
            resorte = Com.Resorte();

            //Dispatcher.Invoke(new Action(() => OcultarPilotBracket(pilotbracket)));
            //Dispatcher.Invoke(new Action(() => OcultarResorte(resorte)));
            //OcultarPilotBracket(pilotbracket);
            //OcultarResorte(resorte);

            Estacion1 = new Thread(TaskE1);
            Estacion2 = new Thread(TaskE2);
            Estacion1.Start();
            Estacion2.Start();
            Estacion1.Join();
            Estacion2.Join();
            Com.Terminar();
            Thread.Sleep(2500);
            //ModeloBtn.IsEnabled = true;
            //RegresarBtn.IsEnabled = true;
            Dispatcher.Invoke(new Action(() => HabilitarBotones(true)));
            Dispatcher.Invoke(LimpiarPantalla);
            Dispatcher.Invoke(CargarContadores);
        }
        private void HabilitarBotones(bool op)
        {
            ModeloBtn.IsEnabled = op;
            RegresarBtn.IsEnabled = op;
        }
        private async void TaskE1()
        {
            await Corrugado1.CambioProgramaAsync(0);
            serial1 = 1.ToString();
            Task Orifice1 = TaskO1();

            //Largo de Corrugado
            #region
            ResultadosE1[0] = await Corrugado1.PruebaAsync(ResultadosE1[0]);
            if (ResultadosE1[0].OKNG && (ResultadosE1[0].Programa == 0))
            {
                Dispatcher.Invoke(() => LargoBI1.OK(true));
            }
            else
            {
                Dispatcher.Invoke(() => LargoBI1.OK(false));
                Fail[0] = true;
            }
            #endregion

            await Corrugado1.CambioProgramaAsync(1);

            //Sentido de Corrugado
            #region
            ResultadosE1[1] = await Corrugado1.PruebaAsync(ResultadosE1[1]);
            if (ResultadosE1[1].OKNG && (ResultadosE1[1].Programa == 1))
            {
                if (sinsentido)
                {
                    Dispatcher.Invoke(() =>SentidoBI1.OK(true));
                }
                else
                {
                    if (ResultadosE1[1].Res == "00")
                    {
                        Dispatcher.Invoke(() => SentidoBI1.OK(true));
                    }
                    else if (ResultadosE1[1].Res == "01")
                    {
                        Dispatcher.Invoke(() => SentidoBI1.OK(false));
                        ResultadosE1[1].OKNG = false;
                    }
                }
            }
            else
            {
                Dispatcher.Invoke(() => SentidoBI1.OK(false));
                Fail[0] = true;
            }
            #endregion

            await Corrugado1.CambioProgramaAsync(2);

            //Nut
            #region
            ResultadosE1[2] = await Corrugado1.PruebaAsync(ResultadosE1[2]);
            if (ResultadosE1[2].OKNG)
            {
                if (!nutrojo && (ResultadosE1[2].Res == "01"))
                {
                    Dispatcher.Invoke(() => NutBI1.OK(true));
                }
                else if (nutrojo && (ResultadosE1[2].Res == "00"))
                {
                    Dispatcher.Invoke(() => NutBI1.OK(true));
                }
                else
                {
                    Dispatcher.Invoke(() => NutBI1.OK(false));
                    Fail[0] = true;
                    ResultadosE1[2].OKNG = false;
                }
            }
            else
            {
                Dispatcher.Invoke(() => NutBI1.OK(false));
                Fail[0] = true;
            }
            #endregion

            //PilotBracket
            #region
            if (Com.PilotBracket())
            {
                if (Com.PilotBracket1())
                {
                    Dispatcher.Invoke(() => PilotBracketBI1.OK(true));
                }
                else
                {
                    Dispatcher.Invoke(() => PilotBracketBI1.OK(false));
                    ResultadosE1[5].OKNG = false;
                    Fail[0] = true;
                }
            }
            else
            {
                ResultadosE1[5].OKNG = true;
                ResultadosE1[5].Res = "SinNut";
            }
            #endregion

            await Orifice1;

            if (Fail[0])
            {
                db.Guardar(serial1, modelo, DateTime.Now, false);
                Thread.Sleep(500);
                Estacion1.Abort();
            }
            else
            {
                Com.E1_3Pass(true);
                EsperarTaponE1.WaitOne();
            }

            await Corrugado1.CambioProgramaAsync(3);

            //Tapon
            #region
            ResultadosE1[3] = await Corrugado1.PruebaAsync(ResultadosE1[3]);
            if (ResultadosE1[3].OKNG)
            {
                Dispatcher.Invoke(() => TaponBI1.OK(true));
                Com.E1_TAPON_COLOCADO(true);
                serial1 = GenerarSerial(modelo, 1, Contador1);
                etiquetadora.GenerarEtiqueta(serial1);
                db.Guardar(serial1, modelo, DateTime.Now, true);
            }
            else
            {
                Dispatcher.Invoke(() => TaponBI1.OK(false));
                Fail[0] = true;
            }

            #endregion

            if (Fail[0])
            {
                db.Guardar(serial1, modelo, DateTime.Now, false);
                Thread.Sleep(500);
                Estacion1.Abort();
            }
            else
            {
                EsperarEtiquetaE1.WaitOne();
            }

            await Corrugado1.CambioProgramaAsync(4);

            //Etiqueta
            #region
            ResultadosE1[4] = await Corrugado1.PruebaAsync(ResultadosE1[4]);
            if (ResultadosE1[4].OKNG)
            {
                Dispatcher.Invoke(() => EtiquetaBI1.OK(true));
                Pass[0] = true;
            }
            else
            {
                Dispatcher.Invoke(() => EtiquetaBI1.OK(false));
                db.Guardar(serial1, modelo, DateTime.Now, false);
                Fail[0] = true;
            }
            #endregion


        }
        private async Task TaskO1()
        {
            ResultadosOrifice1 = await Orifice11.PruebaOrifice();
            if (ResultadosOrifice1.OKNG)
            {
                Dispatcher.Invoke(() => OrificeBI1.OK(true));
            }
            else
            {
                Dispatcher.Invoke(() => OrificeBI1.OK(false));
                Fail[0] = true;
            }
        }
        private async void TaskE2()
        {
            //MensajeE1.Text = MensajeEstacion(0);
            await Corrugado2.CambioProgramaAsync(0);
            serial2 = 2.ToString();
            ////TaskO1();
            //MensajeE1.Text = MensajeEstacion(1);

            TaskO2();

            //Largo de Corrugado
            #region
            ResultadosE2[0] = await Corrugado2.PruebaAsync(ResultadosE2[0]);
            if (ResultadosE2[0].OKNG && (ResultadosE2[0].Programa == 0))
            {
                Dispatcher.Invoke(() => LargoBI2.OK(true));

            }
            else
            {
                Dispatcher.Invoke(() => LargoBI2.OK(false));
                Fail[1] = true;
            }
            #endregion

            await Corrugado2.CambioProgramaAsync(1);

            //Sentido de Corrugado
            #region
            ResultadosE2[1] = await Corrugado2.PruebaAsync(ResultadosE2[1]);
            if (ResultadosE2[1].OKNG && (ResultadosE2[1].Programa == 1))
            {
                if (sinsentido)
                {
                    Dispatcher.Invoke(() => SentidoBI2.OK(true));
                }
                else
                {
                    if (ResultadosE2[1].Res == "00")
                    {
                        Dispatcher.Invoke(() => SentidoBI2.OK(true));
                    }
                    else if (ResultadosE2[1].Res == "01")
                    {
                        Dispatcher.Invoke(() => SentidoBI2.OK(false));
                        ResultadosE2[1].OKNG = false;
                    }
                }
            }
            else
            {
                Dispatcher.Invoke(() => SentidoBI2.OK(false));
                Fail[1] = true;
            }
            #endregion

            await Corrugado2.CambioProgramaAsync(2);

            //Nut
            #region
            ResultadosE2[2] = await Corrugado2.PruebaAsync(ResultadosE2[2]);
            if (ResultadosE2[2].OKNG)
            {
                if (!nutrojo && (ResultadosE2[2].Res == "00"))
                {
                    Dispatcher.Invoke(() => NutBI2.OK(true));
                }
                else if (nutrojo && (ResultadosE2[2].Res == "01"))
                {
                    Dispatcher.Invoke(() => NutBI2.OK(true));
                }
                else
                {
                    Dispatcher.Invoke(() => NutBI2.OK(false));
                    Fail[1] = true;
                    ResultadosE2[2].OKNG = false;
                }
            }
            else
            {
                Dispatcher.Invoke(() => NutBI2.OK(false));
                Fail[1] = true;
            }
            #endregion

            //PilotBracket
            #region
            if (Com.PilotBracket())
            {
                if (Com.PilotBracket2())
                {
                    Dispatcher.Invoke(() => PilotBracketBI2.OK(true));
                }
                else
                {
                    Dispatcher.Invoke(() => PilotBracketBI2.OK(false));
                    ResultadosE2[5].OKNG = false;
                    Fail[1] = true;
                }
            }
            else
            {
                ResultadosE2[5].OKNG = true;
                ResultadosE2[5].Res = "SinNut";
            }
            #endregion

            if (Fail[1])
            {
                //MessageBox.Show("Falla");
                //MensajeE1.Text = MensajeEstacion(6);
                db.Guardar(serial2, modelo, DateTime.Now, false);
                Thread.Sleep(500);
                Estacion2.Abort();

            }
            else
            {
                Com.E2_3Pass(true);
                //MensajeE1.Text = MensajeEstacion(2);
                EsperarTaponE2.WaitOne();
            }

            await Corrugado2.CambioProgramaAsync(3);

            //Tapon
            #region
            ResultadosE2[3] = await Corrugado2.PruebaAsync(ResultadosE2[3]);
            if (ResultadosE2[3].OKNG)
            {
                Dispatcher.Invoke(() => TaponBI2.OK(true));
                Com.E2_TAPON_COLOCADO(true);
                //MensajeE1.Text = MensajeEstacion(3);
                serial2 = GenerarSerial(modelo, 2, Contador2);
                Thread.Sleep(500);
                etiquetadora.GenerarEtiqueta(serial2);
                db.Guardar(serial2, modelo, DateTime.Now, true);
                //Contador1++;
            }
            else
            {
                Dispatcher.Invoke(() => TaponBI2.OK(false));
                Fail[1] = true;
            }

            #endregion

            if (Fail[1])
            {
                //MensajeE1.Text = MensajeEstacion(6);
                db.Guardar(serial2, modelo, DateTime.Now, false);
                Thread.Sleep(500);
                //Com.Terminar();
                Estacion2.Abort();
            }
            else
            {
                //MensajeE1.Text = MensajeEstacion(4);
                EsperarEtiquetaE2.WaitOne();
            }

            await Corrugado2.CambioProgramaAsync(4);

            //Etiqueta
            #region
            ResultadosE2[4] = await Corrugado2.PruebaAsync(ResultadosE2[4]);
            if (ResultadosE2[4].OKNG)
            {
                //MensajeE1.Text = MensajeEstacion(5);
                Dispatcher.Invoke(() => EtiquetaBI2.OK(true));
                Pass[1] = true;
            }
            else
            {
                //MensajeE1.Text = MensajeEstacion(6);
                Dispatcher.Invoke(() => EtiquetaBI2.OK(false));
                db.Guardar(serial2, modelo, DateTime.Now, false);
                Fail[1] = true;
            }
            #endregion

        }
        private async void TaskO2()
        {
            if ((await Orifice21.PruebaOrifice()).OKNG)
            {
                Dispatcher.Invoke(() => OrificeBI2.OK(true));
            }
            else
            {
                Dispatcher.Invoke(() => OrificeBI2.OK(false));
                Fail[1] = true;
            }
        }
        private string GenerarSerial(string Modelo, int Estacion, int Contador)
        {
            string serial = modelo + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")
                + DateTime.Now.ToString("HH") + Estacion.ToString() + Contador.ToString("D3");
            return serial;
        }
        private void CargarContadores()
        {
            ContadorBuenas.Text = "PIEZAS BUENAS: " + db.ObtenerBuenas().ToString("D3");
            ContadorMalas.Text = "PIEZAS MALAS:  " + db.ObtenerMalas().ToString("D3");
        }
    }
}
