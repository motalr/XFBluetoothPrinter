using Shiny.BluetoothLE.Central;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFBluetoothPrinter.ViewModels;

namespace XFBluetoothPrinter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel();
        }

        private void Device_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = BindingContext as HomePageViewModel;

            var device = e.Item as IPeripheral;
            vm.OnSelectedPeripheral(device);
        }
    }
}