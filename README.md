# seekcamera-dotnet

A simple .NET 5 wrapper for Seek thermal camera. Only tested with Seek MosaicCore. 

This sample is thrown together for a prototype and focusing on a narrow use case. We wanted to capture *n1* frames every *n2* seconds. 

The repo consists if two projects:

- **SeekCamera** - wraps the function calls and callbacks from Seek function library.
- **SeekCameraConsole** - console example usage. Will also stitch received images to mp4 at the end (requires ffmpeg in path).

This has been tested on Windows 10 and Raspberry PI 4. In order to make it work you need to copy seekcamera.dll to the SeekCamera folder and rename it libseekcamera.dll. If you want to run on Raspberry PI, copy libseekcamera.so.* to the same location. The project file copies these to the output folder.

Provides following narrow features required by our prototype:

- Initialize camera manager
- Get version information
- Select palette to use
- Start capturing frames and wait for result (synchronously)
- Stop capturing frames
- Each frame captured is stored in a bitmap list to be used after completed capture session.
- Raise events/errors from camera manager
- Destroy camera manager

Work in progress.