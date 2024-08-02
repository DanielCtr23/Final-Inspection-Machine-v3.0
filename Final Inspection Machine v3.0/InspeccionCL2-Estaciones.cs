using IV3_Keyence;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Data;
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
        Estructuras.ResultadosCorrugado ResultadosOrifice1 = new ResultadosCorrugado();
        Estructuras.ResultadosCorrugado ResultadosOrifice2 = new ResultadosCorrugado();
        bool[] Fail = new bool[2];
        bool[] Pass = new bool[2];

        ManualResetEvent EsperarTaponE1, EsperarTaponE2, EsperarEtiquetaE1, EsperarEtiquetaE2;
        DataManager DM = new DataManager();
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
            Dispatcher.InvokeAsync(new Action(() => HabilitarBotones(false)));

            EsperarEtiquetaE1 = new ManualResetEvent(false);
            EsperarEtiquetaE2 = new ManualResetEvent(false);
            EsperarTaponE1 = new ManualResetEvent(false);
            EsperarTaponE2 = new ManualResetEvent(false);
            Contador1 = DM.ContadorSerial(1);
            Contador2 = DM.ContadorSerial(2);
            modelo = Com.ModeloSeleccionado();
            nutrojo = Com.NutRojo();
            pilotbracket = Com.PilotBracket();
            sinsentido = Com.SinSentido();
            resorte = Com.Resorte();

            Estacion1 = new Thread(TaskE1);
            Estacion2 = new Thread(TaskE2);
            Estacion1.IsBackground = true;
            Estacion2.IsBackground = true;
            Estacion1.Start();
            Estacion2.Start();
            Estacion1.Join();
            Estacion2.Join();
            Com.Terminar();
            Thread.Sleep(2500);

            Dispatcher.InvokeAsync(new Action(() =>
            {
                HabilitarBotones(true);
                LimpiarPantalla();
                CargarContadores();
            }
            ));
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
                Dispatcher.InvokeAsync(() => LargoBI1.OK(true));
            }
            else
            {
                Dispatcher.InvokeAsync(() => LargoBI1.OK(false));
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
                    Dispatcher.InvokeAsync(() =>SentidoBI1.OK(true));
                }
                else
                {
                    if (ResultadosE1[1].Res == "00")
                    {
                        Dispatcher.InvokeAsync(() => SentidoBI1.OK(true));
                    }
                    else if (ResultadosE1[1].Res == "01")
                    {
                        Dispatcher.InvokeAsync(() => SentidoBI1.OK(false));
                        ResultadosE1[1].OKNG = false;
                    }
                }
            }
            else
            {
                Dispatcher.InvokeAsync(() => SentidoBI1.OK(false));
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
                    Dispatcher.InvokeAsync(() => NutBI1.OK(true));
                }
                else if (nutrojo && (ResultadosE1[2].Res == "00"))
                {
                    Dispatcher.InvokeAsync(() => NutBI1.OK(true));
                }
                else
                {
                    Dispatcher.InvokeAsync(() => NutBI1.OK(false));
                    Fail[0] = true;
                    ResultadosE1[2].OKNG = false;
                }
            }
            else
            {
                Dispatcher.InvokeAsync(() => NutBI1.OK(false));
                Fail[0] = true;
            }
            #endregion

            //PilotBracket
            #region
            if (Com.PilotBracket1())
            {
                Dispatcher.InvokeAsync(() => PilotBracketBI1.OK(true));
                ResultadosE1[5].OKNG = true;
            }
            else
            {
                Dispatcher.InvokeAsync(() => PilotBracketBI1.OK(false));
                ResultadosE1[5].OKNG = false;
                //Fail[0] = true;
            }
            #endregion

            await Orifice1;

            serial1 = GenerarSerial(modelo, 1, Contador1);

            if (Fail[0])
            {
                DM.Guardar(serial1, modelo, DateTime.Now, false, true, ResultadosOrifice1.OKNG, ResultadosOrifice1.Calificacion,
                    false, -1, -1, false, ResultadosE1[5].OKNG, ResultadosE1[5].Calificacion, ResultadosE1[0].OKNG, ResultadosE1[0].Calificacion,
                    ResultadosE1[1].OKNG, ResultadosE1[1].Calificacion, -1, ResultadosE1[2].OKNG, ResultadosE1[2].Calificacion, -1);
                Thread.Sleep(500);
                Estacion1.Abort();
            }
            else
            {
                DM.Guardar(serial1, modelo, DateTime.Now, false, false, ResultadosOrifice1.OKNG, ResultadosOrifice1.Calificacion,
                    false, -1, -1, false, ResultadosE1[5].OKNG, ResultadosE1[5].Calificacion, ResultadosE1[0].OKNG, ResultadosE1[0].Calificacion,
                    ResultadosE1[1].OKNG, ResultadosE1[1].Calificacion, int.Parse(ResultadosE1[1].Res), ResultadosE1[2].OKNG, ResultadosE1[2].Calificacion, int.Parse(ResultadosE1[2].Res));
                Com.E1_3Pass(true);
                EsperarTaponE1.WaitOne();
            }

            await Corrugado1.CambioProgramaAsync(3);

            //Tapon
            #region
            ResultadosE1[3] = await Corrugado1.PruebaAsync(ResultadosE1[3]);
            if (ResultadosE1[3].OKNG)
            {
                etiquetadora.GenerarEtiqueta(serial1);
                Dispatcher.InvokeAsync(() => TaponBI1.OK(true));
                Com.E1_TAPON_COLOCADO(true);
            }
            else
            {
                Dispatcher.InvokeAsync(() => TaponBI1.OK(false));
                Fail[0] = true;
                ResultadosE1[3].Calificacion = 0;
            }

            DM.Guardar(serial1, DateTime.Now, false, !ResultadosE1[3].OKNG, ResultadosE1[3].OKNG, ResultadosE1[3].Calificacion, false, -1);

            #endregion

            if (Fail[0])
            {
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
                Dispatcher.InvokeAsync(() => EtiquetaBI1.OK(true));
                Pass[0] = true;
            }
            else
            {
                Dispatcher.InvokeAsync(() => EtiquetaBI1.OK(false));
                //db.Guardar(serial1, modelo, DateTime.Now, false);
                Fail[0] = true;
                Pass[0] = false;
            }
            #endregion

            DM.Guardar(serial1, DateTime.Now, Pass[0], !Pass[0], ResultadosE1[3].OKNG, ResultadosE1[3].Calificacion, ResultadosE1[4].OKNG, ResultadosE1[4].Calificacion);


        }
        private async Task TaskO1()
        {
            ResultadosOrifice1 = await Orifice11.PruebaAsync(ResultadosOrifice1);
            if (ResultadosOrifice1.OKNG)
            {
                Dispatcher.InvokeAsync(() => OrificeBI1.OK(true));
            }
            else
            {
                Dispatcher.InvokeAsync(() => OrificeBI1.OK(false));
                Fail[0] = true;
            }
        }
        private async void TaskE2()
        {
            await Corrugado2.CambioProgramaAsync(0);
            serial2 = 2.ToString();
            Task Orifice2 = TaskO2();

            //Largo de Corrugado
            #region
            ResultadosE2[0] = await Corrugado2.PruebaAsync(ResultadosE2[0]);
            if (ResultadosE2[0].OKNG && (ResultadosE2[0].Programa == 0))
            {
                Dispatcher.InvokeAsync(() => LargoBI2.OK(true));

            }
            else
            {
                Dispatcher.InvokeAsync(() => LargoBI2.OK(false));
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
                    Dispatcher.InvokeAsync(() => SentidoBI2.OK(true));
                }
                else
                {
                    if (ResultadosE2[1].Res == "00")
                    {
                        Dispatcher.InvokeAsync(() => SentidoBI2.OK(true));
                    }
                    else if (ResultadosE2[1].Res == "01")
                    {
                        Dispatcher.InvokeAsync(() => SentidoBI2.OK(false));
                        ResultadosE2[1].OKNG = false;
                    }
                }
            }
            else
            {
                Dispatcher.InvokeAsync(() => SentidoBI2.OK(false));
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
                    Dispatcher.InvokeAsync(() => NutBI2.OK(true));
                }
                else if (nutrojo && (ResultadosE2[2].Res == "01"))
                {
                    Dispatcher.InvokeAsync(() => NutBI2.OK(true));
                }
                else
                {
                    Dispatcher.InvokeAsync(() => NutBI2.OK(false));
                    Fail[1] = true;
                    ResultadosE2[2].OKNG = false;
                }
            }
            else
            {
                Dispatcher.InvokeAsync(() => NutBI2.OK(false));
                Fail[1] = true;
            }
            #endregion

            //PilotBracket
            #region

            if (Com.PilotBracket2())
            {
                Dispatcher.InvokeAsync(() => PilotBracketBI2.OK(true));
                ResultadosE2[5].OKNG = true;
            }
            else
            {
                Dispatcher.InvokeAsync(() => PilotBracketBI2.OK(false));
                ResultadosE2[5].OKNG = false;
                //Fail[1] = true;
            }
            #endregion

            await Orifice2;

            serial2 = GenerarSerial(modelo, 2, Contador2);

            if (Fail[1])
            {
                DM.Guardar(serial2, modelo, DateTime.Now, false, true, ResultadosOrifice2.OKNG, ResultadosOrifice2.Calificacion,
                    false, -1, -1, false, ResultadosE2[5].OKNG, ResultadosE2[5].Calificacion, ResultadosE2[0].OKNG, ResultadosE2[0].Calificacion,
                    ResultadosE2[1].OKNG, ResultadosE2[1].Calificacion,-1, ResultadosE2[2].OKNG, ResultadosE2[2].Calificacion, -1);
                Thread.Sleep(500);
                Estacion2.Abort();

            }
            else
            {
                DM.Guardar(serial2, modelo, DateTime.Now, false, false, ResultadosOrifice2.OKNG, ResultadosOrifice2.Calificacion,
                    false, -1, -1, false, ResultadosE2[5].OKNG, ResultadosE2[5].Calificacion, ResultadosE2[0].OKNG, ResultadosE2[0].Calificacion,
                    ResultadosE2[1].OKNG, ResultadosE2[1].Calificacion, int.Parse(ResultadosE2[1].Res), ResultadosE2[2].OKNG, ResultadosE2[2].Calificacion, int.Parse(ResultadosE2[2].Res));
                Com.E2_3Pass(true);
                EsperarTaponE2.WaitOne();
            }

            await Corrugado2.CambioProgramaAsync(3);

            //Tapon
            #region
            ResultadosE2[3] = await Corrugado2.PruebaAsync(ResultadosE2[3]);
            if (ResultadosE2[3].OKNG)
            {
                etiquetadora.GenerarEtiqueta(serial2);
                Dispatcher.InvokeAsync(() => TaponBI2.OK(true));
                Com.E2_TAPON_COLOCADO(true);
            }
            else
            {
                Dispatcher.InvokeAsync(() => TaponBI2.OK(false));
                Fail[1] = true;
                ResultadosE2[3].Calificacion = 0;
            }

            //Arreglar
            Thread.Sleep(100);
            DM.Guardar(serial2, DateTime.Now, false, !ResultadosE2[3].OKNG, ResultadosE2[3].OKNG, ResultadosE2[3].Calificacion, false, -1);

            #endregion

            if (Fail[1])
            {
                Thread.Sleep(500);
                Estacion2.Abort();
            }
            else
            {
                EsperarEtiquetaE2.WaitOne();
            }

            await Corrugado2.CambioProgramaAsync(4);

            //Etiqueta
            #region
            ResultadosE2[4] = await Corrugado2.PruebaAsync(ResultadosE2[4]);
            if (ResultadosE2[4].OKNG)
            {
                Dispatcher.InvokeAsync(() => EtiquetaBI2.OK(true));
                Pass[1] = true;
                
            }
            else
            {
                Dispatcher.Invoke(() => EtiquetaBI2.OK(false));
                //db.Guardar(serial2, modelo, DateTime.Now, false);
                Fail[1] = true;
                Pass[1] = false;
            }
            #endregion
            Thread.Sleep(100);
            DM.Guardar(serial2, DateTime.Now, Pass[1], !Pass[1], ResultadosE2[3].OKNG, ResultadosE2[3].Calificacion, ResultadosE2[4].OKNG, ResultadosE2[4].Calificacion);

        }
        private async Task TaskO2()
        {
            ResultadosOrifice2 = await Orifice21.PruebaAsync(ResultadosOrifice2);
            if (ResultadosOrifice2.OKNG)
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
            DataTable dt = DM.Contador();
            ContadorBuenas.Text = "PIEZAS BUENAS: " + int.Parse(dt.Rows[0]["Valor"].ToString()).ToString("D3");
            ContadorMalas.Text = "PIEZAS MALAS:  " + int.Parse(dt.Rows[1]["Valor"].ToString()).ToString("D3");
        }

        
    }
}
