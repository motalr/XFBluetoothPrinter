using System;
using System.Collections.Generic;
using System.Text;

namespace XFBluetoothPrinter.Services
{
    public interface IImageToByteArray
    {
        byte[] DrawableByNameToByteArray(string fileName);
    }
}
