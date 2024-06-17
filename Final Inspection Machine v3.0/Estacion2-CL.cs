using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Final_Inspection_Machine_v3._0
{
    public partial class Inspeccion_CompactLogix
    {
        private async void TaskE2()
        {
            serial2 = 2.ToString();
            //TaskO2();

            PilotBracketBI2.Color2 = System.Drawing.Color.Yellow;

            //Largo de Corrugado
            #region
            ResultadosE2[0] = await Corrugado2.PruebaAsync(ResultadosE2[0]);
            if (ResultadosE2[0].OKNG && (ResultadosE2[0].Programa == 0))
            {
                LargoBI2.SelectColor2 = true;

            }
            else
            {
                LargoBI2.SelectColor3 = true;
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
                    SentidoBI2.SelectColor2 = true;
                }
                else
                {
                    if (ResultadosE2[1].Res == "00")
                    {
                        SentidoBI2.SelectColor2 = true;
                    }
                    else if (ResultadosE2[1].Res == "01")
                    {
                        SentidoBI2.SelectColor3 = true;
                        ResultadosE2[1].OKNG = false;
                    }
                }
            }
            else
            {
                SentidoBI2.SelectColor3 = true;
                Fail[1] = true;
            }
            #endregion

            await Corrugado1.CambioProgramaAsync(2);

            //Nut
            #region
            ResultadosE2[2] = await Corrugado2.PruebaAsync(ResultadosE2[2]);
            if (ResultadosE2[2].OKNG)
            {
                if (!nutrojo && (ResultadosE2[2].Res == "00"))
                {
                    NutBI2.SelectColor2 = true;
                }
                else if (nutrojo && (ResultadosE2[2].Res == "01"))
                {
                    NutBI2.SelectColor2 = true;
                }
                else
                {
                    NutBI2.SelectColor3 = true;
                    Fail[1] = true;
                    ResultadosE2[2].OKNG = false;
                }
            }
            else
            {
                NutBI2.SelectColor3 = true;
                Fail[1] = true;
            }
            #endregion

            //PilotBracket
            #region
            if (pilotbracket)
            {
                MessageBox.Show(pilotbracket.ToString());
                PilotBracketBI2.Color2 = System.Drawing.Color.Green;
                if (bool.Parse(Com.Read("PB_E2_OK")))
                {

                }
                else
                {
                    PilotBracketBI2.SelectColor3 = true;
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
                Com.Write("E2_3PASS", 1);
                EsperarTaponE2.WaitOne();
            }

            await Corrugado2.CambioProgramaAsync(3);

            //Tapon
            #region
            ResultadosE2[3] = await Corrugado2.PruebaAsync(ResultadosE2[3]);
            if (ResultadosE2[3].OKNG)
            {
                TaponBI2.SelectColor2 = true;
                Com.Write("E2_TAPON_COLOCADO", 1);
                serial2 = GenerarSerial(modelo, 2, Contador2);
                Thread.Sleep(1000);
                etiquetadora.GenerarEtiqueta(serial2);
                Contador2++;
            }
            else
            {
                TaponBI2.SelectColor3 = true;
                Fail[1] = true;
            }

            #endregion

            if (Fail[1])
            {
                Com.Write("E2_TERMINADO", 1);
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
                EtiquetaBI2.SelectColor2 = true;
                Pass[1] = true;
                Com.Write("E2_TERMINADO", 1);
            }
            else
            {
                EtiquetaBI2.SelectColor3 = true;
                Fail[1] = true;
            }
            #endregion


        }

        private async void TaskO2()
        {
            if ((await Orifice21.PruebaOrifice()).OKNG)
            {
                OrificeBI2.SelectColor2 = true;
            }
            else
            {
                OrificeBI2.SelectColor3 = true;
                Fail[1] = true;
            }
        }
    }
}
