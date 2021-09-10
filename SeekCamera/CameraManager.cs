using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

namespace SeekCamera
{
    public partial class CameraManager : IDisposable
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EventCallback(IntPtr camera, CameraEventEnum cameraEvent, int status, IntPtr userData);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void FrameAvailableCallback(IntPtr camera, IntPtr frame, IntPtr userData);
        
        private const int USB = 1;
        private EventCallback _eventCallback;
        private FrameAvailableCallback _frameAvailableCallback;
        private IntPtr _cameraManager = IntPtr.Zero;
        private IntPtr _camera;
        private int _framesReceived;
        private int _numberOfFramesToCapture;
        private List<Bitmap> _bitmaps;
        private bool _stillReceivingFrames = false;

        public PaletteEnum Palette { get; set; } = PaletteEnum.WHITE_HOT;

        public event EventHandler<CameraEventEventArgs> CameraEvent;
        public event EventHandler<CameraFrameEventArgs> CameraFrameEvent;

        public CameraManager()
        {
            _eventCallback = OnCameraEvent;
            _frameAvailableCallback = OnFrameAvailableEvent;
        }

        public void Dispose()
        {
            if (_cameraManager != IntPtr.Zero)
                Destroy();
        }

        protected void OnCameraEvent(CameraEventEventArgs e)
        {
            CameraEvent?.Invoke(this, e);
        }

        protected void OnCameraFrameEvent(CameraFrameEventArgs e)
        {
            CameraFrameEvent?.Invoke(this, e);
        }

        public void OnCameraEvent(IntPtr camera, CameraEventEnum cameraEvent, int status, IntPtr userData)
        {
            switch (cameraEvent)
            {
                case CameraEventEnum.Connect:
                    
                    var res = SeekCameraManagerRegisterFrameAvailableCallback(camera, _frameAvailableCallback, IntPtr.Zero);

                    if (res != CameraErrorEnum.SUCCESS)
                        raiseEvent(res);

                    _camera = camera;
                    break;
                case CameraEventEnum.Disconnect:
                    raiseEvent(CameraErrorEnum.ERROR_DISCONNECTED);

                    break;
                case CameraEventEnum.ReadyToPair:
                    raiseEvent(CameraErrorEnum.ERROR_READY_TO_PAIR);

                    break;
                case CameraEventEnum.Error:
                    raiseEvent((CameraErrorEnum)status);
                    Destroy();
                    break;
                default:
                    break;
            }
        }

        private void raiseEvent(CameraErrorEnum res)
        {
            var args = new CameraEventEventArgs() { ErrorCode = (int)res, ErrorText = GetErrorText((int)res) };
            OnCameraEvent(args);
        }

        public void OnFrameAvailableEvent(IntPtr camera, IntPtr cameraFrame, IntPtr userData)
        {
            if (_framesReceived++ >= _numberOfFramesToCapture)
            {
                _stillReceivingFrames = false;
                return;
            }

            IntPtr frame = IntPtr.Zero;

            var res = SeekCameraGetFrameByFormat(cameraFrame, FrameFormatEnum.COLOR_ARGB8888, ref frame);

            if (res != CameraErrorEnum.SUCCESS)
                raiseEvent(res);

            var width = SeekFrameGetWidth(frame);
            var height = SeekFrameGetHeight(frame);
            var lineStride = SeekFrameGetLineStride(frame);

            var raw = SeekFrameGetData(frame);

            var bitmap = new Bitmap((int)width, (int)height, (int)lineStride, PixelFormat.Format32bppArgb, raw);

            // This is fishy, need to do something else with raw. Passing the bitmap
            // outside this method causes Access violation. Creating copy for now.
            // If I stopwatch the entire event it takes between 0-1 ms, so fairly efficient.
            var bitmapCopy = new Bitmap(bitmap); 

            _bitmaps.Add(bitmapCopy);

            OnCameraFrameEvent(new CameraFrameEventArgs() { Frame = _framesReceived });
        }

        public List<Bitmap> Bitmaps()
        {
            return _bitmaps;
        }

        public CameraErrorEnum StartCapture(int numberOfFrames = 1)
        {
            if (_camera == IntPtr.Zero)
                return CameraErrorEnum.SUCCESS;

            _bitmaps = new List<Bitmap>();

            _numberOfFramesToCapture = numberOfFrames;
            _stillReceivingFrames = true;
            _framesReceived = 0;

            setColorPalette(Palette);

            var res = SeekCameraCaptureSessionStart(_camera, FrameFormatEnum.COLOR_ARGB8888);

            if (res != CameraErrorEnum.SUCCESS)
            {
                raiseEvent(res);
                return res;
            }

            while (_stillReceivingFrames)
            {
                Thread.Sleep(250);
            }

            return 0;
        }

        public CameraErrorEnum StopCapture()
        {
            if (_camera == IntPtr.Zero)
                return CameraErrorEnum.SUCCESS;

            var res = SeekCameraCaptureSessionStop(_camera);

            if (res != CameraErrorEnum.SUCCESS)
                return res;

            _stillReceivingFrames = false;

            return CameraErrorEnum.SUCCESS;
        }

        public CameraErrorEnum Initialize()
        {
            IntPtr userData = IntPtr.Zero;

            var res = SeekCameraManagerCreate(ref _cameraManager, USB);

            if (res != CameraErrorEnum.SUCCESS)
            {
                raiseEvent(res);
                return res;
            }

            res = SeekCameraManagerRegisterEventCallback(_cameraManager, _eventCallback, userData);

            if (res != CameraErrorEnum.SUCCESS)
                raiseEvent(res);

            return res;
        }

        public CameraErrorEnum Destroy()
        {
            if (_cameraManager == IntPtr.Zero)
                return CameraErrorEnum.SUCCESS;

            var res = SeekCameraManagerDestroy(ref _cameraManager);

            if (res != CameraErrorEnum.SUCCESS)
                raiseEvent(res);

            _bitmaps = new List<Bitmap>();

            return CameraErrorEnum.SUCCESS;
        }

        private void setColorPalette(PaletteEnum palette)
        {
            if (_camera == IntPtr.Zero)
                return;

            var res = SeekSetColorPalette(_camera, palette);

            if (res != CameraErrorEnum.SUCCESS)
                raiseEvent(res);
        }

        public string GetVersion()
        {
            return $"{SeekCameraGetMajorVersion()}.{SeekCameraGetMinorVersion()}.{SeekCameraGetPatchVersion()}";
        }

        public string GetErrorText(int status)
        {
            return ((CameraErrorEnum)status).ToString();
        }
    }
}
