using IV3_Keyence;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        private CancellationTokenSource _ctsTaskE1;
        private CancellationTokenSource _ctsTaskE2;
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

        private async Task Ejecucion()
        {
            // Inicializar resultados y estados
            ResultadosE1 = new ResultadosCorrugado[8];
            ResultadosE2 = new ResultadosCorrugado[8];
            Fail[0] = false;
            Pass[0] = false;
            Fail[1] = false;
            Pass[1] = false;

            Dispatcher.Invoke(() => HabilitarBotones(false));

            // Inicializar ManualResetEvents
            EsperarEtiquetaE1 = new ManualResetEvent(false);
            EsperarEtiquetaE2 = new ManualResetEvent(false);
            EsperarTaponE1 = new ManualResetEvent(false);
            EsperarTaponE2 = new ManualResetEvent(false);

            // Configuraciones iniciales
            Contador1 = DM.ContadorSerial(E1);
            Contador2 = DM.ContadorSerial(E2);
            modelo = Com.ModeloSeleccionado();
            nutrojo = Com.NutRojo();
            pilotbracket = Com.PilotBracket();
            sinsentido = Com.SinSentido();
            resorte = Com.Resorte();

            // Actualizar UI
            Dispatcher.Invoke(() =>
            {
                PilotBracketTxB1.Text = pilotbracket ? "PILOT BRACKET" : "PILOT BRACKET N/A";
                PilotBracketTxB2.Text = pilotbracket ? "PILOT BRACKET" : "PILOT BRACKET N/A";
            });

            // Ejecutar pruebas en paralelo
            var taskE1 = TaskE1();
            var taskE2 = TaskE2();

            try
            {
                await Task.WhenAll(taskE1, taskE2);
                await Task.Delay(1200);
            }
            catch (Exception ex)
            {
                // Manejar excepciones si es necesario
                // Por ejemplo, registrar el error
            }

            await Task.Delay(800);
            Com.Terminar();

            // Actualizar UI después de completar las tareas
            Dispatcher.Invoke(() =>
            {
                Error1 = "";
                EstadoE1(3);
                HabilitarBotones(true);
                LimpiarPantalla();
                CargarContadores();
            });
        }
        private void HabilitarBotones(bool op)
        {
            ModeloBtn.IsEnabled = op;
            RegresarBtn.IsEnabled = op;
        }
        private async Task TaskE1()
        {
            try
            {
                Dispatcher.InvokeAsync(() => EstadoE1(0));
                await Corrugado1.CambioProgramaAsync(0);
                serial1 = E1.ToString();
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
                    Error1 = Error1 + " Largo ";
                    Dispatcher.InvokeAsync(() => EstadoE1(0));
                }
                #endregion

                _ctsTaskE1.Token.ThrowIfCancellationRequested();
                await Corrugado1.CambioProgramaAsync(1);

                //Sentido de Corrugado
                #region
                ResultadosE1[1] = await Corrugado1.PruebaAsync(ResultadosE1[1]);
                if (ResultadosE1[1].OKNG && (ResultadosE1[1].Programa == 1))
                {
                    if (sinsentido)
                    {
                        Dispatcher.InvokeAsync(() => SentidoBI1.OK(true));
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
                            Fail[0] = true;
                            Error1 = Error1 + " Sentido I ";
                            Dispatcher.InvokeAsync(() => EstadoE1(0));
                        }
                    }
                }
                else
                {
                    ResultadosE1[1].Res = "-1";
                    Dispatcher.InvokeAsync(() => SentidoBI1.OK(false));
                    Fail[0] = true;
                    Error1 = Error1 + " Sentido -  ";
                    Dispatcher.InvokeAsync(() => EstadoE1(0));
                }
                #endregion

                _ctsTaskE1.Token.ThrowIfCancellationRequested();
                await Corrugado1.CambioProgramaAsync(2);

                //Nut
                #region
                ResultadosE1[2] = await Corrugado1.PruebaAsync(ResultadosE1[2]);
                string n = "";
                if (ResultadosE1[2].OKNG)
                {
                    if (ResultadosE1[2].Res == "01")
                    {
                        ResultadosE1[2].Res = "0";
                        n = "A ";
                    }
                    else if (ResultadosE1[2].Res == "00")
                    {
                        ResultadosE1[2].Res = "1";
                        n = "R ";
                    }

                    if (!nutrojo && (ResultadosE1[2].Res == "1"))
                    {
                        Dispatcher.InvokeAsync(() => NutBI1.OK(true));
                    }
                    else if (nutrojo && (ResultadosE1[2].Res == "0"))
                    {
                        Dispatcher.InvokeAsync(() => NutBI1.OK(true));
                    }
                    else
                    {
                        Dispatcher.InvokeAsync(() => NutBI1.OK(false));
                        Fail[0] = true;
                        ResultadosE1[2].OKNG = false;
                        Error1 = Error1 + " Nut " + n;
                        Dispatcher.InvokeAsync(() => EstadoE1(0));
                    }
                }
                else
                {
                    ResultadosE1[2].Res = "-1";
                    Dispatcher.InvokeAsync(() => NutBI1.OK(false));
                    Fail[0] = true;
                    Error1 = Error1 + " Nut " + "D ";
                    Dispatcher.InvokeAsync(() => EstadoE1(0));
                }
                #endregion

                _ctsTaskE1.Token.ThrowIfCancellationRequested();

                //PilotBracket
                #region

                ResultadosE1[5].Programa = Com.PilotBracketN1();
                ResultadosE1[5].Res = DM.PilotBracketNombre(ResultadosE1[5].Programa);



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
                    Error1 = Error1 + " PB " + ResultadosE1[5].Programa + " ";
                    Dispatcher.InvokeAsync(() => EstadoE1(0));
                }
                #endregion

                _ctsTaskE1.Token.ThrowIfCancellationRequested();

                await Orifice1;

                serial1 = GenerarSerial(modelo, E1, Contador1);

                DM.Guardar(serial1, modelo, DateTime.Now, false, Fail[0],
                    /*Rosca*/ ResultadosOrifice1.OKNG, ResultadosOrifice1.Calificacion, /*Crack*/ false, -1, -1,
                    /*Resorte*/ false,
                    /*PilotBracket*/ ResultadosE1[5].OKNG, ResultadosE1[5].Programa,
                    /*Largo*/  ResultadosE1[0].OKNG, ResultadosE1[0].Calificacion,
                    /*Sentido*/ ResultadosE1[1].OKNG, ResultadosE1[1].Calificacion, int.Parse(ResultadosE1[1].Res),
                    /*NUT*/ ResultadosE1[2].OKNG, ResultadosE1[2].Calificacion, int.Parse(ResultadosE1[2].Res));

                if (Fail[0])
                {
                    Dispatcher.InvokeAsync(() => EstadoE1(2));
                    _ctsTaskE1.Cancel();
                }
                else
                {
                    Com.E1_3Pass(true);
                    EsperarTaponE1.WaitOne();
                }

                _ctsTaskE1.Token.ThrowIfCancellationRequested();
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
                    Error1 = Error1 + "Tapon ";
                    Dispatcher.InvokeAsync(() => EstadoE1(0));
                }

                DM.Guardar(serial1, DateTime.Now, false, Fail[0], ResultadosE1[3].OKNG, ResultadosE1[3].Calificacion, false, -1);

                #endregion

                _ctsTaskE1.Token.ThrowIfCancellationRequested();

                if (Fail[0])
                {
                    Dispatcher.InvokeAsync(() => EstadoE1(2));
                    _ctsTaskE1.Cancel();
                }
                else
                {
                    EsperarEtiquetaE1.WaitOne();
                }

                _ctsTaskE1.Token.ThrowIfCancellationRequested();
                await Corrugado1.CambioProgramaAsync(4);

                //Etiqueta
                #region
                ResultadosE1[4] = await Corrugado1.PruebaAsync(ResultadosE1[4]);
                if (ResultadosE1[4].OKNG)
                {
                    Dispatcher.InvokeAsync(() => EtiquetaBI1.OK(true));
                    Pass[0] = true;
                    Dispatcher.InvokeAsync(() => EstadoE1(1));
                }
                else
                {
                    Dispatcher.InvokeAsync(() => EtiquetaBI1.OK(false));
                    Fail[0] = true;
                    Pass[0] = false;
                    Error1 = Error1 + "Etiqueta ";
                    Dispatcher.InvokeAsync(() => EstadoE1(2));
                }
                #endregion
                DM.Guardar(serial1, DateTime.Now, Pass[0], !Pass[0], ResultadosE1[3].OKNG, ResultadosE1[3].Calificacion, ResultadosE1[4].OKNG, ResultadosE1[4].Calificacion);
            }
            catch (OperationCanceledException)
            {

            }
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
        private async Task TaskE2()
        {
            try
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

                _ctsTaskE2.Token.ThrowIfCancellationRequested();
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
                    ResultadosE2[1].Res = "-1";
                    Dispatcher.InvokeAsync(() => SentidoBI2.OK(false));
                    Fail[1] = true;
                }
                #endregion

                _ctsTaskE2.Token.ThrowIfCancellationRequested();
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
                        ResultadosE2[2].Res = "-1";
                        Dispatcher.InvokeAsync(() => NutBI2.OK(false));
                        Fail[1] = true;
                        ResultadosE2[2].OKNG = false;
                    }
                }
                else
                {
                    ResultadosE2[2].Res = "-1";
                    Dispatcher.InvokeAsync(() => NutBI2.OK(false));
                    Fail[1] = true;
                }
                #endregion

                _ctsTaskE2.Token.ThrowIfCancellationRequested();

                //PilotBracket
                #region


                ResultadosE2[5].Programa = Com.PilotBracketN2();
                ResultadosE2[5].Res = DM.PilotBracketNombre(ResultadosE2[5].Programa);

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

                _ctsTaskE2.Token.ThrowIfCancellationRequested();

                await Orifice2;

                serial2 = GenerarSerial(modelo, E2, Contador2);

                DM.Guardar(serial2, modelo, DateTime.Now, false, Fail[1],
                    /*Rosca*/ ResultadosOrifice2.OKNG, ResultadosOrifice2.Calificacion, /*Crack*/ false, -1, -1,
                    /*Resorte*/ false,
                    /*PilotBracket*/ ResultadosE2[5].OKNG, ResultadosE2[5].Programa,
                    /*Largo*/  ResultadosE2[0].OKNG, ResultadosE2[0].Calificacion,
                    /*Sentido*/ ResultadosE2[1].OKNG, ResultadosE2[1].Calificacion, int.Parse(ResultadosE2[1].Res),
                    /*NUT*/ ResultadosE2[2].OKNG, ResultadosE2[2].Calificacion, int.Parse(ResultadosE2[2].Res));

                if (Fail[1])
                {
                    _ctsTaskE2.Cancel();
                }
                else
                {
                    Com.E2_3Pass(true);
                    EsperarTaponE2.WaitOne();
                }

                _ctsTaskE2.Token.ThrowIfCancellationRequested();
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

                DM.Guardar(serial2, DateTime.Now, false, Fail[1], ResultadosE2[3].OKNG, ResultadosE2[3].Calificacion, false, -1);

                #endregion

                _ctsTaskE2.Token.ThrowIfCancellationRequested();

                if (Fail[1])
                {
                    _ctsTaskE2.Cancel();
                }
                else
                {
                    EsperarEtiquetaE2.WaitOne();
                }

                _ctsTaskE2.Token.ThrowIfCancellationRequested();
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
                    Dispatcher.InvokeAsync(() => EtiquetaBI2.OK(false));
                    Fail[1] = true;
                    Pass[1] = false;
                }
                #endregion
                DM.Guardar(serial2, DateTime.Now, Pass[1], !Pass[1], ResultadosE2[3].OKNG, ResultadosE2[3].Calificacion, ResultadosE2[4].OKNG, ResultadosE2[4].Calificacion);

            }
            catch (OperationCanceledException)
            {

            }
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
