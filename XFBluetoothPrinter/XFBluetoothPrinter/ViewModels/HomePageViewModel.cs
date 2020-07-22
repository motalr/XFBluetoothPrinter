using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Shiny.BluetoothLE.Central;
using Shiny;
using System.Reactive.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Shiny.BluetoothLE;
using XFBluetoothPrinter.Views;

namespace XFBluetoothPrinter.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        IDisposable _scanDisposable, _connectedDisposable;
        ICentralManager _centralManager = Shiny.ShinyHost.Resolve<ICentralManager>();

        public bool IsScanning { get; set; }
        private bool isSelected { get; set; } = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<IPeripheral> bluetoothDevices { get; set; } = new ObservableCollection<IPeripheral>();
        public ObservableCollection<IPeripheral> BluetoothDevices
        {
            get { return bluetoothDevices; }
            set
            {
                bluetoothDevices = value;
                OnPropertyChanged();
            }
        }
        private IPeripheral selectedDevice { get; set; }
        public IPeripheral SelectedDevice
        {
            get { return selectedDevice; }
            set
            {
                selectedDevice = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetDeviceListCommand { get; set; }
        public ICommand SetAdapterCommand { get; set; }
        public ICommand CheckPermissionsCommand { get; set; }
        public ICommand GoToPrintCommand { get; set; }

        public HomePageViewModel()
        {
            GetDeviceListCommand = new Command(() => GetDeviceList());
            SetAdapterCommand = new Command(async () => await SetAdapter());
            GoToPrintCommand = new Command(() => GoToPrintPage());
            CheckPermissionsCommand = new Command(async () => await CheckPermissions());
            CheckPermissionsCommand.Execute(null);
        }

        public async Task CheckPermissions()
        {
            var status = await _centralManager.RequestAccess();
            if (status == AccessState.Denied)
            {
                await App.Current.MainPage.DisplayAlert("Permissions", "You need to have your Bluetooth ON to use this feature", "Ok");
                Xamarin.Essentials.AppInfo.ShowSettingsUI();
            }
            else
            {
                SetAdapterCommand.Execute(null);
            }
        }


        public async Task SetAdapter()
        {
            var poweredOn = _centralManager.Status == AccessState.Available;
            if (!poweredOn && !_centralManager.TrySetAdapterState(true))
                await App.Current.MainPage.DisplayAlert("Cannot change bluetooth adapter state", "", "Ok");

            if (poweredOn)
            {
                GetConnectedDevices();
            }
        }

        public void GetConnectedDevices()
        {
            _connectedDisposable = _centralManager.GetConnectedPeripherals().Subscribe(scanResult =>
            {
                scanResult.ToList().ForEach(
                 item =>
                 {
                     if (!string.IsNullOrEmpty(item.Name))
                         BluetoothDevices.Add(item);
                 });

                _connectedDisposable?.Dispose();
            });

            if (_centralManager.IsScanning)
                _centralManager.StopScan();
        }

        public void OnSelectedPeripheral(IPeripheral peripheral)
        {
            SelectedDevice = peripheral;

            IsSelected = true;

            _scanDisposable?.Dispose();
            IsScanning = _centralManager.IsScanning;
        }

        private void GoToPrintPage()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new PrintPage(SelectedDevice));
                SelectedDevice = null;
                BluetoothDevices = new ObservableCollection<IPeripheral>();
            });
        }

        public void CancelScanning()
        {
            _scanDisposable?.Dispose();
            IsScanning = _centralManager.IsScanning;
        }

        private void GetDeviceList()
        {
            if (_centralManager.IsScanning)
            {
                _scanDisposable?.Dispose();
            }
            else
            {
                if (_centralManager.Status == Shiny.AccessState.Available && !_centralManager.IsScanning)
                {
                    _scanDisposable = _centralManager.ScanForUniquePeripherals().Subscribe(scanResult =>
                    {
                        IsSelected = false;

                        if (!string.IsNullOrEmpty(scanResult.Name) && !BluetoothDevices.Contains(scanResult))
                            BluetoothDevices.Add(scanResult);

                    });
                }
            }

            IsScanning = _centralManager.IsScanning;
        }


    }
}
