using Shiny.BluetoothLE;
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
    public partial class PrintPage : ContentPage
    {
        public PrintPage(IPeripheral device)
        {
            InitializeComponent();
            BindingContext = new PrintViewModel(device);

        }
    }
}