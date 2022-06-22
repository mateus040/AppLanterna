using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.Battery;
using Xamarin.Essentials;

namespace AppLanterna
{
    public partial class MainPage : ContentPage
    {
        bool lanterna_ligada = false;

        public MainPage()
        {
            InitializeComponent();

            btnOnOff.Source = ImageSource.FromResource("AppLanterna.Img.botao-desligado.jpg");

            Carrega_info_Bateria();
        }

        private void BtnOnOff_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (lanterna_ligada)
                {
                    // se estiver ligada, vou desligar.
                    btnOnOff.Source = ImageSource.FromResource("AppLanterna.Img.botao-desligado.jpg");
                    lanterna_ligada = false;

                    Flashlight.TurnOffAsync();

                }
                else
                {
                    // senão, vou ligar.
                    btnOnOff.Source = ImageSource.FromResource("AppLanterna.Img.botao-ligado.jpg");
                    lanterna_ligada = true;

                    Flashlight.TurnOnAsync();
                }

                Vibration.Vibrate(TimeSpan.FromMilliseconds(300));

            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }


        private void Carrega_info_Bateria()
        {
            try
            {
                if (CrossBattery.IsSupported)
                {
                    CrossBattery.Current.BatteryChanged -= Mudanca_Status_Bateria;
                    CrossBattery.Current.BatteryChanged += Mudanca_Status_Bateria;
                }
                else throw new Exception("Dados da bateria indisponíveis");

            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private void Mudanca_Status_Bateria(object sender, Plugin.Battery.Abstractions.BatteryChangedEventArgs e)
        {
            try
            {
                lbl_porcentagem_restante.Text = e.RemainingChargePercent.ToString() + "%";

                if (e.IsLow)
                {
                    lbl_bateria_fraca.IsVisible = true;

                }
                else
                {
                    lbl_bateria_fraca.IsVisible = false;
                }

                switch (e.Status)
                {
                    case Plugin.Battery.Abstractions.BatteryStatus.Charging:
                        lbl_status.Text = "Carregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Discharging:
                        lbl_status.Text = "Descarregando";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Full:
                        lbl_status.Text = "Carregado";
                        break;
                    case Plugin.Battery.Abstractions.BatteryStatus.NotCharging:
                        lbl_status.Text = "Sem Carregar";
                        break;

                    case Plugin.Battery.Abstractions.BatteryStatus.Unknown:
                        lbl_status.Text = "Desconhecido";
                        break;
                }


                switch (e.PowerSource)
                {
                    case Plugin.Battery.Abstractions.PowerSource.Ac:
                        lbl_fonte_carregamento.Text = "Carregador";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Battery:
                        lbl_fonte_carregamento.Text = "Bateria";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Usb:
                        lbl_fonte_carregamento.Text = "USB";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Wireless:
                        lbl_fonte_carregamento.Text = "Sem fio";
                        break;

                    case Plugin.Battery.Abstractions.PowerSource.Other:
                        lbl_fonte_carregamento.Text = "Desconhecido";
                        break;
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
