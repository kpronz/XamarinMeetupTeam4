#if __ANDROID__
using System;
using Android.App;
using Android.Content;
using Android.Telephony;
using Android.Widget;
using Common.Core;

[assembly: UsesPermission(Name = "android.permission.CALL_PHONE")]
[assembly: UsesPermission(Name = "android.permission.READ_PHONE_STATE")]
[assembly: Xamarin.Forms.Dependency(typeof(PhoneCall))]
namespace Common.Core
{
    public class PhoneCall : IPhoneCall
    {
        private TelephonyManager telephonyManager;
        private PhoneCallListener phoneListener;

        public void PlaceCall(string phoneNumber)
        {
            try
            {
                var ctx = Xamarin.Forms.Forms.Context;
                var intent = new Intent(Intent.ActionCall);
                var uri = global::Android.Net.Uri.Parse("tel:" + CoreExtensions.CleanPhoneNumber(phoneNumber));
                intent.SetData(uri);
                ctx.StartActivity(intent);
            }
            catch (Exception ex)
            {
                var toast = Toast.MakeText(Xamarin.Forms.Forms.Context, "This activity is not supported", ToastLength.Long);
                toast.Show();
            }

        }

        public void PlaceCallWithCallBack(string phoneNumber)
        {
            try
            {
                var ctx = Xamarin.Forms.Forms.Context;
                phoneListener = new PhoneCallListener();
                telephonyManager = (TelephonyManager)ctx.GetSystemService(Context.TelephonyService);
                phoneListener.CallEndedEvent += PhoneCallEnded;

                telephonyManager.Listen(phoneListener, PhoneStateListenerFlags.CallState);
                var intent = new Intent(Intent.ActionCall);
                var uri = global::Android.Net.Uri.Parse("tel:" + CoreExtensions.CleanPhoneNumber(phoneNumber));
                intent.SetData(uri);
                ctx.StartActivity(intent);
            }
            catch (Exception ex)
            {
                var toast = Toast.MakeText(Xamarin.Forms.Forms.Context, "This activity is not supported", ToastLength.Long);
                toast.Show();
            }
        }
        private void PhoneCallEnded(DateTime start, DateTime end)
        {
            if (phoneListener != null)
                phoneListener.CallEndedEvent -= PhoneCallEnded;
            telephonyManager.Listen(phoneListener, PhoneStateListenerFlags.None);
            phoneListener = null;
            PhoneCallback.Instance.Complete(true);
        }
    }

    public delegate void CallEndedEventHandle(DateTime startTime, DateTime endTime);
    public class PhoneCallListener : PhoneStateListener
    {
        private bool isPhoneCalling = false;
        public event CallEndedEventHandle CallEndedEvent;
        private DateTime startTime;

        public override void OnCallStateChanged(CallState state, string incomingNumber)
        {
            switch ((CallState)state)
            {
                case CallState.Ringing:
                    break;
                case CallState.Offhook:
                    isPhoneCalling = true;
                    startTime = DateTime.Now;
                    break;
                case CallState.Idle:
                    if (isPhoneCalling)
                    {
                        isPhoneCalling = false;
                        if (CallEndedEvent != null)
                        {
                            CallEndedEvent(startTime, DateTime.Now);
                        }
                    }
                    break;
            }
            base.OnCallStateChanged(state, incomingNumber);
        }
    }
}
#endif
