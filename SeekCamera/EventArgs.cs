using System;

namespace SeekCamera
{
    public class CameraEventEventArgs : EventArgs
    {
        public int ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }

    public class CameraFrameEventArgs : EventArgs
    {
        public int Frame { get; set; }
    }
}
