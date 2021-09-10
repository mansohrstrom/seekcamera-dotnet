using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeekCamera
{
    public enum CameraEventEnum : uint
    {
        Connect = 0,
        Disconnect = 1,
        Error = 2,
        ReadyToPair = 3,
        StartSession = 4,
        StopSession = 5
    }

    public enum FrameFormatEnum : uint
    {
        CORRECTED = 0x04,
        PRE_AGC = 0x08,
        THERMOGRAPHY_FLOAT = 0x10,
        THERMOGRAPHY_FIXED_10_6 = 0x20,
        GRAYSCALE = 0x40,
        COLOR_ARGB8888 = 0x80,
        COLOR_RGB565 = 0x100,
        COLOR_AYUV = 0x200,
        COLOR_YUY2 = 0x400,
    }

    public enum PaletteEnum : uint
    {
        WHITE_HOT,
        BLACK_HOT,
        SPECTRA,
        PRISM,
        TYRIAN,
        IRON,
        AMBER,
        HI,
        GREEN,
        USER_0,
        USER_1,
        USER_2,
        USER_3,
        USER_4
    }

    public enum CameraErrorEnum : int
    {
        SUCCESS = 0,
        ERROR_DEVICE_COMMUNICATION = -1,
        ERROR_INVALID_PARAMETER = -2,
        ERROR_PERMISSIONS = -3,
        ERROR_NO_DEVICE = -4,
        ERROR_DEVICE_NOT_FOUND = -5,
        ERROR_DEVICE_BUSY = -6,
        ERROR_TIMEOUT = -7,
        ERROR_OVERFLOW = -8,
        ERROR_UNKNOWN_REQUEST = -9,
        ERROR_INTERRUPTED = -10,
        ERROR_OUT_OF_MEMORY = -11,
        ERROR_NOT_SUPPORTED = -12,
        ERROR_OTHER = -99,
        ERROR_CANNOT_PERFORM_REQUEST = -103,
        ERROR_FLASH_ACCESS_FAILURE = -104,
        ERROR_IMPLEMENTATION_ERROR = -105,
        ERROR_REQUEST_PENDING = -106,
        ERROR_INVALID_FIRMWARE_IMAGE = -107,
        ERROR_INVALID_KEY = -108,
        ERROR_SENSOR_COMMUNICATION = -109,
        ERROR_OUT_OF_RANGE = -301,
        ERROR_VERIFY_FAILED = -302,
        ERROR_SYSCALL_FAILED = -303,
        ERROR_FILE_DOES_NOT_EXIST = -400,
        ERROR_DIRECTORY_DOES_NOT_EXIST = -401,
        ERROR_FILE_READ_FAILED = -402,
        ERROR_FILE_WRITE_FAILED = -403,
        ERROR_NOT_IMPLEMENTED = -1000,
        ERROR_NOT_PAIRED = -1001,
        ERROR_DISCONNECTED = -10000,
        ERROR_READY_TO_PAIR = -10001
    }
}
