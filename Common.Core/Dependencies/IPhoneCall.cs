using System;
namespace Common.Core
{
    public class PhoneCallback
    {
        public event EventHandler Finished;
        public bool IsListening { get; set; }
        public static PhoneCallback Instance = new PhoneCallback();
        public void Complete(bool success)
        {
            //if (Finished != null)
            //    Finished(null, new PhoneActionCallBack() { Success = success });

        }
    }
    public interface IPhoneCall
    {
        void PlaceCall(string phoneNumber);
        void PlaceCallWithCallBack(string phoneNumber);
    }
}
