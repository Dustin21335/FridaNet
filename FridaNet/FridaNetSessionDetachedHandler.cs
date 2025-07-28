namespace FridaNet
{
    public class FridaNetSessionDetachedEventArgs : EventArgs
    {
        public FridaNetSessionDetachReason Reason { get; }

        public FridaNetSessionDetachedEventArgs(FridaNetSessionDetachReason reason)
        {
            Reason = reason;
        }
    }

    public enum FridaNetSessionDetachReason
    {
        ApplicationRequested = 1,
        ProcessReplaced,
        ProcessTerminated,
        ConnectionTerminated,
        DeviceLost
    }

    public delegate void FridaNetSessionDetachedHandler(object sender, FridaNetSessionDetachedEventArgs e);
}
