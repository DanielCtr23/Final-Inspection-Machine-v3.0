using K_IV3;
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


namespace Final_Inspection_Machine_v3._0
{
    public partial class InspeccionMicro800
    {

        IV3.Resultado[] ResE1 = new IV3.Resultado[10];
        IV3.Resultado[] ResE2 = new IV3.Resultado[10];
        bool[] Fail = new bool[2];
        bool[] Pass = new bool[2];
        ManualResetEvent EsperarTaponE1, EsperarTaponE2, EsperarEtiquetaE1, EsperarEtiquetaE2;
        DataManager DM = new DataManager();
        //Etiquetadora etiquetadora;
        Etiquetadora2 etiquetadora2;
        bool nutrojo, sinsentido, pilotbracket, resorte;
        string serial1, serial2;
        string modelo;
        int Contador1, Contador2;
        private Thread HiloPrincipal;
        private Thread Estacion1;
        private Thread Estacion2;

        int[] TriggerControl = new int[] { 0, 0, 0, 0, 0, 0 };

        private void Ejecucion()
        {
            string etiquetadoraStatus = etiquetadora2.Status();
            if (etiquetadoraStatus != "Ready To Print")
            {
                MessageBox.Show(etiquetadoraStatus);
                return;
            }

            Abortar = false;
            // Inicializar resultados y estados
            ResE1 = new IV3.Resultado[10];
            ResE2 = new IV3.Resultado[10];
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

            Estacion1 = new Thread(TaskE1);
            Estacion2 = new Thread(TaskE2);
            Estacion1.IsBackground = true;
            Estacion2.IsBackground = true;
            Estacion1.Start();
            Estacion2.Start();

            Estacion1.Join();
            Estacion2.Join();

            //Esperar Para Limpiar Pantalla
            Com.Terminar();
            Thread.Sleep(1750);

            // Actualizar UI después de completar las tareas
            Dispatcher.Invoke(() =>
            {
                Error1 = "";
                EstadoE1(3);
                Error2 = "";
                EstadoE2(3);
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
        private async void TaskE1()
        {
            try { 
            Dispatcher.InvokeAsync(() => EstadoE1(0));
            await Corrugado1.Program(0);
            serial1 = E1.ToString();
            Task Orifice1 = TaskO1();

            //Largo de Corrugado
            #region
            ResE1[5] = await Corrugado1.Trigger();
            if (ResE1[5].OKNG && (ResE1[5].Programa == 0))
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

                if (ResE1[5].TriggerNo > TriggerControl[2])
                {
                    TriggerControl[2] = ResE1[5].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 1: Contactar a Ingeniería o Mantenimiento");
                    return;
                }

                await Corrugado1.Program(1);

            //Sentido de Corrugado
            #region
            ResE1[6] = await Corrugado1.Trigger();
            if (ResE1[6].OKNG && (ResE1[6].Programa == 1))
            {
                if (sinsentido)
                {
                    Dispatcher.InvokeAsync(() => SentidoBI1.OK(true));
                }
                else
                {
                    if (ResE1[6].Tipo == 0)
                    {
                        Dispatcher.InvokeAsync(() => SentidoBI1.OK(true));
                    }
                    else if (ResE1[6].Tipo == 1)
                    {
                        Dispatcher.InvokeAsync(() => SentidoBI1.OK(false));
                        ResE1[6].OKNG = false;
                        Fail[0] = true;
                        Error1 = Error1 + " Sentido I ";
                        Dispatcher.InvokeAsync(() => EstadoE1(0));
                    }
                }
            }
            else
            {
                ResE1[6].Tipo = -1;
                Dispatcher.InvokeAsync(() => SentidoBI1.OK(false));
                Fail[0] = true;
                Error1 = Error1 + " Sentido -  ";
                Dispatcher.InvokeAsync(() => EstadoE1(0));
            }
                #endregion

                if (ResE1[6].TriggerNo > TriggerControl[2])
                {
                    TriggerControl[2] = ResE1[6].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 1: Contactar a Ingeniería o Mantenimiento");
                    return;
                }

                await Corrugado1.Program(2);

            //Nut
            #region
            ResE1[7] = await Corrugado1.Trigger();
            string n = "";
            if (ResE1[7].OKNG)
            {
                if (ResE1[7].Tipo == 0)
                {
                    ResE1[7].Tipo = 0;
                    n = "A ";
                }
                else if (ResE1[7].Tipo == 1)
                {
                    ResE1[7].Tipo = 1;
                    n = "R ";
                }

                if (!nutrojo && (ResE1[7].Tipo == 0))
                {
                    Dispatcher.InvokeAsync(() => NutBI1.OK(true));
                }
                else if (nutrojo && (ResE1[7].Tipo == 1))
                {
                    Dispatcher.InvokeAsync(() => NutBI1.OK(true));
                }
                else
                {
                    Dispatcher.InvokeAsync(() => NutBI1.OK(false));
                    Fail[0] = true;
                    ResE1[7].OKNG = false;
                    Error1 = Error1 + " Nut " + n;
                    Dispatcher.InvokeAsync(() => EstadoE1(0));
                }
            }
            else
            {
                ResE1[2].Tipo = -1;
                Dispatcher.InvokeAsync(() => NutBI1.OK(false));
                Fail[0] = true;
                Error1 = Error1 + " Nut " + "D ";
                Dispatcher.InvokeAsync(() => EstadoE1(0));
            }
                #endregion

                if (ResE1[7].TriggerNo > TriggerControl[2])
                {
                    TriggerControl[2] = ResE1[7].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 1: Contactar a Ingeniería o Mantenimiento");
                    return;
                }

                //PilotBracket
                #region

                ResE1[4].Programa = Com.PilotBracketN1();
            ResE1[4].Tipo = ResE1[4].Programa;


            if (Com.PilotBracket1())
            {
                Dispatcher.InvokeAsync(() => PilotBracketBI1.OK(true));
                ResE1[4].OKNG = true;
            }
            else
            {
                Dispatcher.InvokeAsync(() => PilotBracketBI1.OK(false));
                ResE1[4].OKNG = false;
                //Fail[0] = true;
                Error1 = Error1 + " PB " + DM.PilotBracketNombre(ResE1[4].Tipo) + " ";
                Dispatcher.InvokeAsync(() => EstadoE1(0));
            }
            #endregion

            await Orifice1;

            serial1 = GenerarSerial(modelo, E1, Contador1);
            try
            {
                DM.Guardar(serial1, modelo, DateTime.Now, false, Fail[0],
                   /*Rosca*/ ResE1[0].OKNG, ResE1[0].Calificacion, /*Crack*/ false, -1, -1,
                   /*Resorte*/ false,
                   /*PilotBracket*/ ResE1[4].OKNG, ResE1[4].Programa,
                   /*Largo*/  ResE1[5].OKNG, ResE1[5].Calificacion,
                   /*Sentido*/ ResE1[6].OKNG, ResE1[6].Calificacion, ResE1[6].Tipo,
                   /*NUT*/ ResE1[7].OKNG, ResE1[7].Calificacion, ResE1[7].Tipo);

            }
            catch (Exception)
            {

            }

            if (Abortar)
            {
                return;
            }

            if (Fail[0])
            {
                Dispatcher.InvokeAsync(() => EstadoE1(2));
                return;
            }
            else
            {
                Com.E1_3Pass(true);
                EsperarTaponE1.WaitOne();
            }

            if (Abortar)
            {
                return;
            }

            await Corrugado1.Program(3);

            //Tapon
            #region
            ResE1[8] = await Corrugado1.Trigger();
            if (ResE1[8].OKNG && ResE1[8].Programa == 3)
            {
                etiquetadora2.GenerarEtiqueta(serial1);
                Dispatcher.InvokeAsync(() => TaponBI1.OK(true));
                Com.E1_TAPON_COLOCADO(true);
            }
            else
            {
                Dispatcher.InvokeAsync(() => TaponBI1.OK(false));
                Fail[0] = true;
                //ResE1[3].Calificacion = 0;
                Error1 = Error1 + "Tapon ";
                Dispatcher.InvokeAsync(() => EstadoE1(0));
            }

            DM.Guardar(serial1, DateTime.Now, false, Fail[0], ResE1[8].OKNG, ResE1[8].Calificacion, false, -1);
                #endregion

                if (ResE1[8].TriggerNo > TriggerControl[2])
                {
                    TriggerControl[2] = ResE1[8].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 1: Contactar a Ingeniería o Mantenimiento");
                    return;
                }

                if (Abortar)
            {
                return;
            }

            if (Fail[0])
            {
                Dispatcher.InvokeAsync(() => EstadoE1(2));
                return;
            }
            else
            {
                EsperarEtiquetaE1.WaitOne();
            }

            if (Abortar)
            {
                return;
            }

            await Corrugado1.Program(4);


            //Etiqueta
            #region
            ResE1[9] = await Corrugado1.Trigger();
            if (ResE1[9].OKNG && ResE1[9].Programa == 4)
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
            Com.E1_TAPON_COLOCADO(false);
                #endregion
                if (ResE1[9].TriggerNo > TriggerControl[2])
                {
                    TriggerControl[2] = ResE1[9].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 1: Contactar a Ingeniería o Mantenimiento");
                    return;
                }
                DM.Guardar(serial1, DateTime.Now, Pass[0], !Pass[0], ResE1[8].OKNG, ResE1[8].Calificacion, ResE1[9].OKNG, ResE1[9].Calificacion);
        }
            catch(Exception)
            {

            }
        }
        private async Task TaskO1()
        {
            try
            {
                await Orifice11.Program(0);
                ResE1[0] = await Orifice11.Trigger();
                if (ResE1[0].OKNG)
                {
                    await Dispatcher.InvokeAsync(() => OrificeBI1.OK(true));
                }
                else
                {
                    await Dispatcher.InvokeAsync(() => OrificeBI1.OK(false));
                    Fail[0] = true;
                    Error1 = Error1 + " Rosca ";
                }

                if (ResE1[0].TriggerNo > TriggerControl[0])
                {
                    TriggerControl[0] = ResE1[0].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara Trasera de Orifice 1: Contactar a Ingeniería o Mantenimiento");
                    return;
                }
            }
            catch (Exception e)
            {
                Fail[0] = true;
                Error1 = Error1 + " *Rosca* ";
            }
        }
        private async void TaskE2()
        {
            try
            {
                Dispatcher.InvokeAsync(() => EstadoE2(0));
                await Corrugado2.Program(0);
                serial2 = E2.ToString();
                Task Orifice2 = TaskO2();

                //Largo de Corrugado
                #region
                ResE2[5] = await Corrugado2.Trigger();
                if (ResE2[5].OKNG && (ResE2[5].Programa == 0))
                {
                    Dispatcher.InvokeAsync(() => LargoBI2.OK(true));

                }
                else
                {
                    Dispatcher.InvokeAsync(() => LargoBI2.OK(false));
                    Fail[1] = true;
                    Error2 = Error2 + " Largo ";
                    Dispatcher.InvokeAsync(() => EstadoE2(0));
                }
                #endregion
                if (ResE2[5].TriggerNo > TriggerControl[5])
                {
                    TriggerControl[5] = ResE2[5].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 2: Contactar a Ingeniería o Mantenimiento");
                    return;
                }

                await Corrugado2.Program(1);

                //Sentido de Corrugado
                #region
                ResE2[6] = await Corrugado2.Trigger();
                if (ResE2[6].OKNG && (ResE2[6].Programa == 1))
                {
                    if (sinsentido)
                    {
                        Dispatcher.InvokeAsync(() => SentidoBI2.OK(true));
                    }
                    else
                    {
                        if (ResE2[6].Tipo == 0)
                        {
                            Dispatcher.InvokeAsync(() => SentidoBI2.OK(true));
                        }
                        else if (ResE2[6].Tipo == 1)
                        {
                            Dispatcher.InvokeAsync(() => SentidoBI2.OK(false));
                            ResE2[1].OKNG = false;
                            Fail[0] = true;
                            Error2 = Error2 + " Sentido I ";
                            Dispatcher.InvokeAsync(() => EstadoE2(0));
                        }
                    }
                }
                else
                {
                    ResE2[6].Tipo = -1;
                    Dispatcher.InvokeAsync(() => SentidoBI2.OK(false));
                    Fail[1] = true;
                    Error2 = Error2 + " Sentido -  ";
                    Dispatcher.InvokeAsync(() => EstadoE2(0));
                }
                #endregion
                if (ResE2[6].TriggerNo > TriggerControl[5])
                {
                    TriggerControl[5] = ResE2[6].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 2: Contactar a Ingeniería o Mantenimiento");
                    return;
                }

                await Corrugado2.Program(2);

                //Nut
                #region
                ResE2[7] = await Corrugado2.Trigger();
                string n = "";
                if (ResE2[7].OKNG)
                {
                    if (ResE2[7].Tipo == 0)
                    {
                        ResE2[7].Tipo = 0;
                        n = "A ";
                    }
                    else if (ResE2[7].Tipo == 1)
                    {
                        n = "R ";
                    }
                    if (!nutrojo && (ResE2[7].Tipo == 0))
                    {
                        Dispatcher.InvokeAsync(() => NutBI2.OK(true));
                    }
                    else if (nutrojo && (ResE2[7].Tipo == 1))
                    {
                        Dispatcher.InvokeAsync(() => NutBI2.OK(true));
                    }
                    else
                    {
                        Dispatcher.InvokeAsync(() => NutBI2.OK(false));
                        Fail[1] = true;
                        ResE2[7].OKNG = false;
                        Error2 = Error2 + " Nut " + n;
                        Dispatcher.InvokeAsync(() => EstadoE2(0));
                    }
                }
                else
                {
                    ResE2[7].Tipo = 1;
                    Dispatcher.InvokeAsync(() => NutBI2.OK(false));
                    Fail[1] = true;
                    Error2 = Error2 + " Nut " + "D ";
                    Dispatcher.InvokeAsync(() => EstadoE2(0));
                }
                #endregion
                if (ResE2[7].TriggerNo > TriggerControl[5])
                {
                    TriggerControl[5] = ResE2[7].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 2: Contactar a Ingeniería o Mantenimiento");
                    return;
                }


                //PilotBracket
                #region


                ResE2[4].Programa = Com.PilotBracketN2();
                ResE2[4].Tipo = ResE2[4].Programa;

                if (Com.PilotBracket2())
                {
                    Dispatcher.InvokeAsync(() => PilotBracketBI2.OK(true));
                    ResE2[4].OKNG = true;
                }
                else
                {
                    Dispatcher.InvokeAsync(() => PilotBracketBI2.OK(false));
                    ResE2[4].OKNG = false;
                    //Fail[1] = true;
                    Error2 = Error2 + " PB " + DM.PilotBracketNombre(ResE2[4].Programa) + " ";
                    Dispatcher.InvokeAsync(() => EstadoE2(0));
                }
                #endregion

                await Orifice2;

                serial2 = GenerarSerial(modelo, E2, Contador2);
                try { 
                DM.Guardar(serial2, modelo, DateTime.Now, false, Fail[1],
                    /*Rosca*/ ResE2[0].OKNG, ResE2[0].Calificacion, /*Crack*/ false, -1, -1,
                    /*Resorte*/ false,
                    /*PilotBracket*/ ResE2[4].OKNG, ResE2[4].Programa,
                    /*Largo*/  ResE2[5].OKNG, ResE2[5].Calificacion,
                    /*Sentido*/ ResE2[6].OKNG, ResE2[6].Calificacion, ResE2[6].Tipo,
                    /*NUT*/ ResE2[7].OKNG, ResE2[7].Calificacion, ResE2[7].Tipo);

                }
                catch(Exception)
                {

                }

                if (Abortar)
                {
                    return;
                }

                if (Fail[1])
                {
                    Dispatcher.InvokeAsync(() => EstadoE2(2));
                    return;
                }
                else
                {
                    Com.E2_3Pass(true);
                    EsperarTaponE2.WaitOne();
                }

                if (Abortar)
                {
                    return;
                }

                await Corrugado2.Program(3);

                //Tapon
                #region
                ResE2[8] = await Corrugado2.Trigger();
                if (ResE2[8].OKNG && ResE2[8].Programa == 3)
                {
                    etiquetadora2.GenerarEtiqueta(serial2);
                    Dispatcher.InvokeAsync(() => TaponBI2.OK(true));
                    Com.E2_TAPON_COLOCADO(true);
                }
                else
                {
                    Dispatcher.InvokeAsync(() => TaponBI2.OK(false));
                    Fail[1] = true;
                    //ResE2[3].Calificacion = 0;
                    Error2 = Error2 + "Tapon ";
                    Dispatcher.InvokeAsync(() => EstadoE2(0));
                }

                DM.Guardar(serial2, DateTime.Now, false, Fail[1], ResE2[8].OKNG, ResE2[8].Calificacion, false, -1);
                #endregion
                if (ResE2[8].TriggerNo > TriggerControl[5])
                {
                    TriggerControl[5] = ResE2[8].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 2: Contactar a Ingeniería o Mantenimiento");
                    return;
                }

                if (Abortar)
                {
                    return;
                }

                if (Fail[1])
                {
                    Dispatcher.InvokeAsync(() => EstadoE2(2));
                    return;
                }
                else
                {
                    EsperarEtiquetaE2.WaitOne();
                }

                if (Abortar)
                {
                    return;
                }

                await Corrugado2.Program(4);

                //Etiqueta
                #region
                ResE2[9] = await Corrugado2.Trigger();
                if (ResE2[9].OKNG && ResE2[9].Programa == 4)
                {
                    Dispatcher.InvokeAsync(() => EtiquetaBI2.OK(true));
                    Pass[1] = true;
                    Dispatcher.InvokeAsync(() => EstadoE2(1));

                }
                else
                {
                    Dispatcher.InvokeAsync(() => EtiquetaBI2.OK(false));
                    Fail[1] = true;
                    Pass[1] = false;
                    Error2 = Error2 + "Etiqueta ";
                    Dispatcher.InvokeAsync(() => EstadoE2(2));
                }
                Com.E2_TAPON_COLOCADO(false);
                #endregion
                if (ResE2[9].TriggerNo > TriggerControl[5])
                {
                    TriggerControl[5] = ResE2[9].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara de Corrugado 2: Contactar a Ingeniería o Mantenimiento");
                    return;
                }
                DM.Guardar(serial2, DateTime.Now, Pass[1], !Pass[1], ResE2[8].OKNG, ResE2[8].Calificacion, ResE2[9].OKNG, ResE2[9].Calificacion);


            }
            catch (Exception e)
            {

            }

        }
        private async Task TaskO2()
        {
            try
            {
                await Orifice21.Program(0);
                ResE2[0] = await Orifice21.Trigger();
                if (ResE2[0].OKNG)
                {
                    await Dispatcher.InvokeAsync(() => OrificeBI2.OK(true));
                }
                else
                {
                    await Dispatcher.InvokeAsync(() => OrificeBI2.OK(false));
                    Fail[1] = true;
                    Error2 = Error2 + " Rosca ";
                }
                if (ResE2[0].TriggerNo > TriggerControl[3])
                {
                    TriggerControl[3] = ResE2[0].TriggerNo;
                }
                else
                {
                    MessageBox.Show("No se realizo Triggger en Camara Trasera de Orifice 2: Contactar a Ingeniería o Mantenimiento");
                    return;
                }
            }
            catch (Exception e)
            {
                Fail[1] = true;
                Error2 = Error2 + " *Rosca* ";
            }
        }
        private string GenerarSerial(string Modelo, int Estacion, int Contador)
        {
            string serial = modelo + DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")
                + DateTime.Now.ToString("HH") + Estacion.ToString() +(Contador%1000).ToString("D3");
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
