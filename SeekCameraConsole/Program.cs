using SeekCamera;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace SeekCameraConsole
{
    class Program
    {
        private static CameraManager _cameraManager;

        public static async Task Main(string[] args)
        {
            using (_cameraManager = new CameraManager());

            Console.WriteLine($"Seek Camera version {_cameraManager.GetVersion()}");

            _cameraManager.CameraEvent += OnCameraEvent;
            _cameraManager.CameraFrameEvent += (s, e) =>
                {
                    Console.CursorLeft = 0;
                    Console.Write($"Frame: {e.Frame}   ");
                };

            Console.WriteLine("Initializing...");
            var result = _cameraManager.Initialize();

            if (result != CameraErrorEnum.SUCCESS)
                return;

            _cameraManager.Palette = PaletteEnum.WHITE_HOT;
            
            string directory = ensureOutputDirectory();
            
            var counter = 0;

            for (int round = 1; round < 5; round++)
            {
                Console.WriteLine("Round " + round);

                counter = takePics(10, counter, directory);
            }

            bool generateMP4 = false;

            if (generateMP4)
            {
                Console.WriteLine("Done. Generating mp4.");

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg",
                        Arguments = $"-framerate 5 -i {directory}{Path.DirectorySeparatorChar}img-%04d.jpg {directory}{Path.DirectorySeparatorChar}out.mp4",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false,
                        RedirectStandardError = true
                    },
                    EnableRaisingEvents = true
                };

                process.Start();

                string processOutput = null;
                while ((processOutput = process.StandardError.ReadLine()) != null)
                {
                    Debug.WriteLine(processOutput);
                }
            }

            Console.WriteLine("Destroying...");

            _cameraManager.Destroy();

            _cameraManager = null;

            Console.WriteLine("Done.");

            Console.ReadLine();
        }

        private static string ensureOutputDirectory()
        {
            string directory = Directory.GetCurrentDirectory();
            directory = Path.Join(directory, "out");

            Directory.CreateDirectory(directory);

            foreach (var file in new DirectoryInfo(directory).EnumerateFiles("*"))
                file.Delete();

            return directory;
        }

        private static int takePics(int images, int counter, string directory)
        {
            var result = _cameraManager.StartCapture(images);

            if (result != CameraErrorEnum.SUCCESS)
                return 0;

            result = _cameraManager.StopCapture();

            if (result != CameraErrorEnum.SUCCESS)
                return 0;

            int i = 1;

            Console.WriteLine("Saving images...");

            foreach (var item in _cameraManager.Bitmaps())
            {
                var fileName = Path.Join(directory, $"img-{counter + i++:D4}.jpg");

                item.Save(fileName, ImageFormat.Jpeg);
            }

            return counter + images;
        }

        static void OnCameraEvent(object sender, CameraEventEventArgs e)
        {
            Console.WriteLine($"  [Event] Code {e.ErrorCode} Text '{e.ErrorText}'.");
        }
    }
}
