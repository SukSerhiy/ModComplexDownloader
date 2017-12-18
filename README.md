# ModComplexDownloader
My dyploma work, part 2

The WPF-project which makes some settings on computer before using ModComplexProject.
It downloads some programms from Google Drive and makes changes in Windows Registry.

Download.dll downloads files and folders from Google Drive using Google Drive Api.
Class GoogleDriveDataDownloader.cs uses for downloading files, class Saver.cs - for saving them to computer.
ProgressConfiger.cs is used for tracing the progress of downloading.

RegistryWorker.dll is used for making changes to Windows registry and making reserve copy of registry befire it.
