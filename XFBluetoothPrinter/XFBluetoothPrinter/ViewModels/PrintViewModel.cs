using Shiny.BluetoothLE;
using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XFBluetoothPrinter.Services;

namespace XFBluetoothPrinter.ViewModels
{
    public class PrintViewModel : BaseViewModel
    {
        IDisposable _perifDisposable;
        IGattCharacteristic _savedCharacteristic;

        private IPeripheral peripheral { get; set; }
        public IPeripheral Peripheral
        {
            get { return peripheral; }
            set
            {
                peripheral = value;
                OnPropertyChanged();
            }
        }

        private bool isReady { get; set; } = false;
        public bool IsReady
        {
            get { return isReady; }
            set
            {
                isReady = value;
                OnPropertyChanged();
            }
        }

        public ICommand PrintCommand { get; set; }
        public ICommand ConnectDeviceCommand { get; set; }
        public ICommand DisconnectDeviceCommand { get; set; }

        public PrintViewModel(IPeripheral device)
        {
            PrintCommand = new Command(() => Print());
            ConnectDeviceCommand = new Command<IPeripheral>(ConnectPrinter);
            DisconnectDeviceCommand = new Command(DisconnectPrinter);

            if(device != null)
            {
                ConnectDeviceCommand.Execute(device);
            }
        }

        private void DisconnectPrinter()
        {
            if (Peripheral.IsConnected())
                Peripheral.CancelConnection();
        }

        public void ConnectPrinter(IPeripheral selectedPeripheral)
        {
            if (!selectedPeripheral.IsConnected())
                selectedPeripheral.Connect();

            _perifDisposable = selectedPeripheral.WhenAnyCharacteristicDiscovered().Subscribe((characteristic) =>
            {
                if (characteristic.CanWrite() && !characteristic.CanRead() && !characteristic.CanNotify())
                {
                    _savedCharacteristic = characteristic;

                    IsReady = true;

                    _perifDisposable.Dispose();
                }
            });

            
        }
        public void Print()
        {
            var Logo = DependencyService.Get<IImageToByteArray>().DrawableByNameToByteArray("poslogo.png");

            //var logoByte = Encoding.UTF8.GetBytes(Logo);

            _savedCharacteristic?.Write(Logo).Subscribe(
                result =>
                {
                    ShowMessage("Success!", "Receipt Printed");
                },
                exception =>
                {
                    ShowMessage("Failed!", "Unable to print");
                });
        }

        public void ShowMessage(string title, string message)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert(title, message, "Ok");
            });
        }
    }
}
