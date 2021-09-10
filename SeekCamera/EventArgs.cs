using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
