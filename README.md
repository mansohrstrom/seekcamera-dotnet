# seekcamera-dotnet

A simple .NET 5 wrapper for Seek thermal camera. Only tested with Seek MosaicCore. 

This sample is thrown together for a prototype and focusing on a narrow use case. We wanted to capture *n1* frames every *n2* seconds. 

The repo consists if two projects:

- **SeekCamera** - wraps the function calls and callbacks from Seek function library.
- **SeekCameraConsole** - console example usage. Will also stitch received images to mp4 at the end.

This has been tested on Windows 10 and Raspberry PI 4. In order to make it work you need to copy seekcamera.dll to the SeekCamera folder and rename it libseekcamera.dll. If you want to run on Raspberry PI, copy libseekcamera.so.* to the same location. The project file copies these to the output folder.

Work in progress.