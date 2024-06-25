using IV3_Keyence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static IV3_Keyence.Estructuras;

namespace Final_Inspection_Machine_v3._0
{
    public partial class InspeccionCL2
    {
        Thread HiloPrincipal;
        Thread Estacion1, Estacion2;

        Estructuras.ResultadosCorrugado[] ResultadosE1 = new ResultadosCorrugado[8];
        Estructuras.ResultadosCorrugado[] ResultadosE2 = new ResultadosCorrugado[8];
        IV3 Corrugado1, Corrugado2, Orifice11, Orifice12, Orifice21, Orifice22;
        bool[] Fail = new bool[2];
        bool[] Pass = new bool[2];

        bool nutrojo, sinsentido, pilotbracket;

        private async void Ejecucion()
        {
            Estacion1 = new Thread(TaskE1);
            Estacion2 = new Thread(TaskE2);
            Estacion1.Start();
            Estacion2.Start();
            Estacion1.Join();
            Estacion2.Join();
        }

        private async void TaskE1()
        {
            await Corrugado1.CambioProgramaAsync(0);
            serial1 = 1.ToString();
            //TaskO1();

            //PilotBracketBI1.Color2 = System.Drawing.Color.Yellow;

            //Largo de Corrugado
            #region
            ResultadosE1[0] = await Corrugado1.PruebaAsync(ResultadosE1[0]);
            if (ResultadosE1[0].OKNG && (ResultadosE1[0].Programa == 0))
            {
                //LargoBI1.SelectColor2 = true;

            }
            else
            {
                //LargoBI1.SelectColor3 = true;
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
                    //SentidoBI1.SelectColor2 = true;
                }
                else
                {
                    if (ResultadosE1[1].Res == "00")
                    {
                        //SentidoBI1.SelectColor2 = true;
                    }
                    else if (ResultadosE1[1].Res == "01")
                    {
                        //SentidoBI1.SelectColor3 = true;
                        ResultadosE1[1].OKNG = false;
                    }
                }
            }
            else
            {
                //SentidoBI1.SelectColor3 = true;
                Fail[0] = true;
            }
            #endregion

            await Corrugado1.CambioProgramaAsync(2);

            //Nut
            #region
            ResultadosE1[2] = await Corrugado1.PruebaAsync(ResultadosE1[2]);
            if (ResultadosE1[2].OKNG)
            {
                if (!nutrojo && (ResultadosE1[2].Res == "00"))
                {
                    //NutBI1.SelectColor2 = true;
                }
                else if (nutrojo && (ResultadosE1[2].Res == "01"))
                {
                    //NutBI1.SelectColor2 = true;
                }
                else
                {
                    //NutBI1.SelectColor3 = true;
                    Fail[0] = true;
                    ResultadosE1[2].OKNG = false;
                }
            }
            else
            {
                //NutBI1.SelectColor3 = true;
                Fail[0] = true;
            }
            #endregion

            //PilotBracket
            #region
            if (Com.PilotBracket())
            {
                //PilotBracketBI1.Color2 = System.Drawing.Color.Green;
                if (Com.PilotBracket1())
                {

                }
                else
                {
                    //PilotBracketBI1.SelectColor3 = true;
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

            if (Fail[0])
            {
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
                //TaponBI1.SelectColor2 = true;
                Com.E1_TAPON_COLOCADO(true);
                serial1 = GenerarSerial(modelo, 1, Contador2);
                //Thread.Sleep(1000);
                etiquetadora.GenerarEtiqueta(serial1);
                Contador1++;
            }
            else
            {
                //TaponBI1.SelectColor3 = true;
                Fail[0] = true;
            }

            #endregion

            if (Fail[0])
            {
                Com.Terminar();
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
                //EtiquetaBI1.SelectColor2 = true;
                Pass[0] = true;
                Com.Terminar();
            }
            else
            {
                //EtiquetaBI1.SelectColor3 = true;
                Fail[0] = true;
            }
            #endregion


        }

        private async void TaskO1()
        {
            if ((await Orifice11.PruebaOrifice()).OKNG)
            {
                //OrificeBI1.SelectColor2 = true;
            }
            else
            {
                //OrificeBI1.SelectColor3 = true;
                Fail[0] = true;
            }
        }


        private async void TaskE2()
        {
            await Corrugado2.CambioProgramaAsync(0);
            serial1 = 1.ToString();
            //TaskO1();

            //PilotBracketBI1.Color2 = System.Drawing.Color.Yellow;

            //Largo de Corrugado
            #region
            ResultadosE2[0] = await Corrugado2.PruebaAsync(ResultadosE1[0]);
            if (ResultadosE2[0].OKNG && (ResultadosE2[0].Programa == 0))
            {
                //LargoBI1.SelectColor2 = true;

            }
            else
            {
                //LargoBI1.SelectColor3 = true;
                Fail[1] = true;
            }
            #endregion

            await Corrugado2.CambioProgramaAsync(1);

            //Sentido de Corrugado
            #region
            ResultadosE2[1] = await Corrugado2.PruebaAsync(ResultadosE1[1]);
            if (ResultadosE2[1].OKNG && (ResultadosE1[1].Programa == 1))
            {
                if (sinsentido)
                {
                    //SentidoBI1.SelectColor2 = true;
                }
                else
                {
                    if (ResultadosE2[1].Res == "00")
                    {
                        //SentidoBI1.SelectColor2 = true;
                    }
                    else if (ResultadosE2[1].Res == "01")
                    {
                        //SentidoBI1.SelectColor3 = true;
                        ResultadosE2[1].OKNG = false;
                    }
                }
            }
            else
            {
                //SentidoBI1.SelectColor3 = true;
                Fail[1] = true;
            }
            #endregion

            await Corrugado2.CambioProgramaAsync(2);

            //Nut
            #region
            ResultadosE2[2] = await Corrugado2.PruebaAsync(ResultadosE1[2]);
            if (ResultadosE2[2].OKNG)
            {
                if (!nutrojo && (ResultadosE2[2].Res == "00"))
                {
                    //NutBI1.SelectColor2 = true;
                }
                else if (nutrojo && (ResultadosE2[2].Res == "01"))
                {
                    //NutBI1.SelectColor2 = true;
                }
                else
                {
                    //NutBI1.SelectColor3 = true;
                    Fail[1] = true;
                    ResultadosE2[2].OKNG = false;
                }
            }
            else
            {
                //NutBI1.SelectColor3 = true;
                Fail[1] = true;
            }
            #endregion

            //PilotBracket
            #region
            if (Com.PilotBracket())
            {
                //PilotBracketBI1.Color2 = System.Drawing.Color.Green;
                if (Com.PilotBracket2())
                {

                }
                else
                {
                    //PilotBracketBI1.SelectColor3 = true;
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
                Estacion2.Abort();

            }
            else
            {
                Com.E2_3Pass(true);
                EsperarTaponE1.WaitOne();
            }

            await Corrugado1.CambioProgramaAsync(3);

            //Tapon
            #region
            ResultadosE2[3] = await Corrugado2.PruebaAsync(ResultadosE1[3]);
            if (ResultadosE2[3].OKNG)
            {
                //TaponBI1.SelectColor2 = true;
                Com.E2_TAPON_COLOCADO(true);
                serial1 = GenerarSerial(modelo, 1, Contador2);
                //Thread.Sleep(1000);
                etiquetadora.GenerarEtiqueta(serial1);
                Contador1++;
            }
            else
            {
                //TaponBI1.SelectColor3 = true;
                Fail[0] = true;
            }

            #endregion

            if (Fail[1])
            {
                Estacion2.Abort();
            }
            else
            {
                EsperarEtiquetaE1.WaitOne();
            }

            await Corrugado2.CambioProgramaAsync(4);

            //Etiqueta
            #region
            ResultadosE2[4] = await Corrugado2.PruebaAsync(ResultadosE1[4]);
            if (ResultadosE2[4].OKNG)
            {
                //EtiquetaBI1.SelectColor2 = true;
                Pass[1] = true;
            }
            else
            {
                //EtiquetaBI1.SelectColor3 = true;
                Fail[1] = true;
            }
            #endregion


        }
    }
}
