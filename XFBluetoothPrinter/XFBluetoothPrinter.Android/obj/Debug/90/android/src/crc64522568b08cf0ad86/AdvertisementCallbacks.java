package crc64522568b08cf0ad86;


public class AdvertisementCallbacks
	extends android.bluetooth.le.AdvertiseCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onStartSuccess:(Landroid/bluetooth/le/AdvertiseSettings;)V:GetOnStartSuccess_Landroid_bluetooth_le_AdvertiseSettings_Handler\n" +
			"n_onStartFailure:(I)V:GetOnStartFailure_IHandler\n" +
			"";
		mono.android.Runtime.register ("Shiny.BluetoothLE.Peripherals.Internals.AdvertisementCallbacks, Shiny.BluetoothLE", AdvertisementCallbacks.class, __md_methods);
	}


	public AdvertisementCallbacks ()
	{
		super ();
		if (getClass () == AdvertisementCallbacks.class)
			mono.android.TypeManager.Activate ("Shiny.BluetoothLE.Peripherals.Internals.AdvertisementCallbacks, Shiny.BluetoothLE", "", this, new java.lang.Object[] {  });
	}


	public void onStartSuccess (android.bluetooth.le.AdvertiseSettings p0)
	{
		n_onStartSuccess (p0);
	}

	private native void n_onStartSuccess (android.bluetooth.le.AdvertiseSettings p0);


	public void onStartFailure (int p0)
	{
		n_onStartFailure (p0);
	}

	private native void n_onStartFailure (int p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
