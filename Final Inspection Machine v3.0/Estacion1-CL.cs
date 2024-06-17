using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Final_Inspection_Machine_v3._0
{
    public partial class Inspeccion_CompactLogix
    {
        private async void TaskE1()
        {
            serial1 = 1.ToString();
            //TaskO1();

            PilotBracketBI1.Color2 = System.Drawing.Color.Yellow;

            //Largo de Corrugado
            #region
            ResultadosE1[0] = await Corrugado1.PruebaAsync(ResultadosE1[0]);
            if (ResultadosE1[0].OKNG && (ResultadosE1[0].Programa == 0))
            {
                LargoBI1.SelectColor2 = true;

            }
            else
            {
                LargoBI1.SelectColor3 = true;
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
                    SentidoBI1.SelectColor2 = true;
                }
                else
                {
                    if (ResultadosE1[1].Res == "00")
                    {
                        SentidoBI1.SelectColor2 = true;
                    }
                    else if (ResultadosE1[1].Res == "01")
                    {
                        SentidoBI1.SelectColor3 = true;
                        ResultadosE1[1].OKNG = false;
                    }
                }
            }
            else
            {
                SentidoBI1.SelectColor3 = true;
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
                    NutBI1.SelectColor2 = true;
                }
                else if (nutrojo && (ResultadosE1[2].Res == "01"))
                {
                    NutBI1.SelectColor2 = true;
                }
                else
                {
                    NutBI1.SelectColor3 = true;
                    Fail[0] = true;
                    ResultadosE1[2].OKNG = false;
                }
            }
            else
            {
                NutBI1.SelectColor3 = true;
                Fail[0] = true;
            }
            #endregion

            //PilotBracket
            #region
            if (pilotbracket)
            {
                PilotBracketBI1.Color2 = System.Drawing.Color.Green;
                if (bool.Parse(Com.Read("PB_E1_OK")))
                {

                }
                else
                {
                    PilotBracketBI1.SelectColor3 = true;
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
                Com.Write("E1_3PASS", 1);
                EsperarTaponE1.WaitOne();
            }

            await Corrugado1.CambioProgramaAsync(3);

            //Tapon
            #region
            ResultadosE1[3] = await Corrugado1.PruebaAsync(ResultadosE1[3]);
            if (ResultadosE1[3].OKNG)
            {
                TaponBI1.SelectColor2 = true;
                Com.Write("E1_TAPON_COLOCADO", 1);
                serial1 = GenerarSerial(modelo, 1, Contador2);
                //Thread.Sleep(1000);
                etiquetadora.GenerarEtiqueta(serial1);
                Contador1++;
            }
            else
            {
                TaponBI1.SelectColor3 = true;
                Fail[0] = true;
            }

            #endregion

            if (Fail[0])
            {
                Com.Write("E1_TERMINADO", 1);
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
                EtiquetaBI1.SelectColor2 = true;
                Pass[0] = true;
                Com.Write("E1_TERMINADO", 1);
            }
            else
            {
                EtiquetaBI1.SelectColor3 = true;
                Fail[0] = true;
            }
            #endregion


        }

        private async void TaskO1()
        {
            if ((await Orifice11.PruebaOrifice()).OKNG)
            {
                OrificeBI1.SelectColor2 = true;
            }
            else
            {
                OrificeBI1.SelectColor3 = true;
                Fail[0] = true;
            }
        }

    }
}
