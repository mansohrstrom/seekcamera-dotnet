using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SeekCamera
{
    public partial class CameraManager
    {
        [DllImport("libseekcamera", EntryPoint = "seekcamera_version_get_major", CharSet = CharSet.Auto)]
        public static extern int SeekCameraGetMajorVersion();
        [DllImport("libseekcamera", EntryPoint = "seekcamera_version_get_minor", CharSet = CharSet.Auto)]
        public static extern int SeekCameraGetMinorVersion();
        [DllImport("libseekcamera", EntryPoint = "seekcamera_version_get_patch", CharSet = CharSet.Auto)]
        public static extern int SeekCameraGetPatchVersion();
        [DllImport("libseekcamera", EntryPoint = "seekcamera_manager_create", CharSet = CharSet.Auto)]
        public static extern CameraErrorEnum SeekCameraManagerCreate(ref IntPtr cameraManager, uint discoveryMode);

        [DllImport("libseekcamera", EntryPoint = "seekcamera_manager_destroy", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraErrorEnum SeekCameraManagerDestroy(ref IntPtr cameraManager);

        [DllImport("libseekcamera", EntryPoint = "seekcamera_manager_register_event_callback", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraErrorEnum SeekCameraManagerRegisterEventCallback(IntPtr cameraManager, EventCallback callback, IntPtr userData);

        [DllImport("libseekcamera", EntryPoint = "seekcamera_register_frame_available_callback", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraErrorEnum SeekCameraManagerRegisterFrameAvailableCallback(IntPtr camera, FrameAvailableCallback callback, IntPtr userData);

        [DllImport("libseekcamera", EntryPoint = "seekcamera_capture_session_start", CharSet = CharSet.Auto)]
        public static extern CameraErrorEnum SeekCameraCaptureSessionStart(IntPtr camera, FrameFormatEnum format);

        [DllImport("libseekcamera", EntryPoint = "seekcamera_frame_get_frame_by_format", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraErrorEnum SeekCameraGetFrameByFormat(IntPtr cameraFrame, FrameFormatEnum format, ref IntPtr frame);

        [DllImport("libseekcamera", EntryPoint = "seekcamera_capture_session_stop", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraErrorEnum SeekCameraCaptureSessionStop(IntPtr camera);

        [DllImport("libseekcamera", EntryPoint = "seekframe_get_data", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SeekFrameGetData(IntPtr frame);

        [DllImport("libseekcamera", EntryPoint = "seekframe_get_line_stride", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SeekFrameGetLineStride(IntPtr frame);

        [DllImport("libseekcamera", EntryPoint = "seekframe_get_width", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SeekFrameGetWidth(IntPtr frame);

        [DllImport("libseekcamera", EntryPoint = "seekframe_get_height", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint SeekFrameGetHeight(IntPtr frame);

        [DllImport("libseekcamera", EntryPoint = "seekcamera_set_color_palette", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern CameraErrorEnum SeekSetColorPalette(IntPtr camera, PaletteEnum palette);
    }
}
