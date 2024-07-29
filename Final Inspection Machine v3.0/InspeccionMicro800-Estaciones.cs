using IV3_Keyence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IV3_Keyence.Estructuras;

namespace Final_Inspection_Machine_v3._0
{
    public partial class InspeccionMicro800
    {
        Thread HiloPrincipal;
        Thread Estacion1, Estacion2;

        Estructuras.ResultadosCorrugado[] ResultadosE1 = new ResultadosCorrugado[8];
        Estructuras.ResultadosCorrugado[] ResultadosE2 = new ResultadosCorrugado[8];
        Estructuras.ResultadosCorrugado ResultadosOrifice1 = new ResultadosCorrugado();
        Estructuras.ResultadosCorrugado ResultadosOrifice2 = new ResultadosCorrugado();
        IV3 Corrugado1, Corrugado2, Orifice11, Orifice12, Orifice21, Orifice22;
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
            Dispatcher.Invoke(new Action(() => HabilitarBotones(false)));

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
            Estacion1.Start();
            Estacion2.Start();
            Estacion1.Join();
            Estacion2.Join();
            Com.Terminar();
            Thread.Sleep(2500);

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
            serial1 = 3.ToString();
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
                    Dispatcher.Invoke(() => SentidoBI1.OK(true));
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
                    ResultadosE1[5].OKNG = true;
                }
                else
                {
                    Dispatcher.Invoke(() => PilotBracketBI1.OK(false));
                    ResultadosE1[5].OKNG = false;
                    //Fail[0] = true;
                }
            }
            else
            {
                if (Com.PilotBracket1())
                {
                    Dispatcher.Invoke(() => PilotBracketBI1.OK(false));
                    ResultadosE1[5].OKNG = false;
                    //Fail[0] = true;
                }
                else
                {
                    Dispatcher.Invoke(() => PilotBracketBI1.OK(true));
                    ResultadosE1[5].OKNG = true;
                }
                ResultadosE1[5].Res = "SinPB";
            }
            #endregion

            await Orifice1;

            serial1 = GenerarSerial(modelo, 3, Contador1);

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
                Dispatcher.Invoke(() => TaponBI1.OK(true));
                Com.E1_TAPON_COLOCADO(true);
                etiquetadora.GenerarEtiqueta(serial1);
            }
            else
            {
                Dispatcher.Invoke(() => TaponBI1.OK(false));
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
                Dispatcher.Invoke(() => EtiquetaBI1.OK(true));
                Pass[0] = true;
            }
            else
            {
                Dispatcher.Invoke(() => EtiquetaBI1.OK(false));
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
            await Corrugado2.CambioProgramaAsync(0);
            serial2 = 4.ToString();
            Task Orifice2 = TaskO2();

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
                    ResultadosE2[5].OKNG = true;
                }
                else
                {
                    Dispatcher.Invoke(() => PilotBracketBI2.OK(false));
                    ResultadosE2[5].OKNG = false;
                    //Fail[1] = true;
                }
            }
            else
            {
                if (Com.PilotBracket2())
                {
                    Dispatcher.Invoke(() => PilotBracketBI2.OK(false));
                    ResultadosE2[5].OKNG = false;
                    //Fail[1] = true;
                }
                else
                {
                    Dispatcher.Invoke(() => PilotBracketBI2.OK(true));
                    ResultadosE2[5].OKNG = true;
                }
                ResultadosE2[5].Res = "SinPB";
            }
            #endregion

            await Orifice2;

            serial2 = GenerarSerial(modelo, 4, Contador2);

            if (Fail[1])
            {
                DM.Guardar(serial2, modelo, DateTime.Now, false, true, ResultadosOrifice2.OKNG, ResultadosOrifice2.Calificacion,
                    false, -1, -1, false, ResultadosE2[5].OKNG, ResultadosE2[5].Calificacion, ResultadosE2[0].OKNG, ResultadosE2[0].Calificacion,
                    ResultadosE2[1].OKNG, ResultadosE2[1].Calificacion, -1, ResultadosE2[2].OKNG, ResultadosE2[2].Calificacion, -1);
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
                Dispatcher.Invoke(() => TaponBI2.OK(true));
                Com.E2_TAPON_COLOCADO(true);
                Thread.Sleep(350);
                etiquetadora.GenerarEtiqueta(serial2);
            }
            else
            {
                Dispatcher.Invoke(() => TaponBI2.OK(false));
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
                Dispatcher.Invoke(() => EtiquetaBI2.OK(true));
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
            ContadorMalas.Text = "PIEZAS MALAS:  " + int.Parse(dt.Rows[0]["Valor"].ToString()).ToString("D3");
        }
    }
}
