using System;

namespace DTMInfo
{
	internal sealed class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Usage: DTMInfo.exe [path to DTM movie file]");
				return;
			}

			DTM movieFile = new DTM(args[0]);

			Console.WriteLine("Game ID                        : {0}", movieFile.GameID);
			Console.WriteLine("Is Wii game                    : {0}", movieFile.IsWiiGame);
			Console.WriteLine("Number of connected controllers: {0}", movieFile.ConnectedControllers);
			Console.WriteLine("Is from a save state           : {0}", movieFile.IsFromSaveState);
			Console.WriteLine("Frame count                    : {0}", movieFile.FrameCount);
			Console.WriteLine("Input frames                   : {0}", movieFile.InputFrameCount);
			Console.WriteLine("Lag frames                     : {0}", movieFile.LagFrameCount);
			Console.WriteLine("Number of re-records           : {0}", movieFile.NumRerecords);
			Console.WriteLine("Author name                    : {0}", movieFile.Author);
			Console.WriteLine("Video backend                  : {0}", movieFile.VideoBackend);
			Console.WriteLine("Audio emulator                 : {0}", movieFile.AudioEmulator);
			Console.WriteLine("MD5                            : {0}", movieFile.MD5
				);
			Console.WriteLine("Recording start time           : {0} seconds", movieFile.RecordingStartTime);
			Console.WriteLine("Using a saved config           : {0}", movieFile.IsSavedConfig);
			Console.WriteLine("Using idle skipping            : {0}", movieFile.UsingIdleSkip);
			Console.WriteLine("Using dual core                : {0}", movieFile.UsingDualCore);
			Console.WriteLine("Using progressive scan         : {0}", movieFile.UsingProgressiveScan);
			Console.WriteLine("Using DSP HLE                  : {0}", movieFile.UsingHLEDSP);
			Console.WriteLine("Using fast disc speed          : {0}", movieFile.UsingFastDiscSpeed);
			Console.WriteLine("CPU Core                       : {0}", movieFile.CPUCore);
			Console.WriteLine("EFB access enabled             : {0}", movieFile.IsEFBAccessEnabled);
			Console.WriteLine("EFB copies enabled             : {0}", movieFile.IsEFBCopiesEnabled);
			Console.WriteLine("Using EFB to texture           : {0}", movieFile.UsingEFBToTexture);
			Console.WriteLine("Using EFB copy cache           : {0}", movieFile.IsEFBCopyCacheEnabled);
			Console.WriteLine("Emulate EFB format changes     : {0}", movieFile.IsEmulatingEFBFormatChanges);
			Console.WriteLine("Using XFB                      : {0}", movieFile.UsingXFB);
			Console.WriteLine("Using Real XFB                 : {0}", movieFile.UsingRealXFB);
			Console.WriteLine("Using memory card              : {0}", movieFile.UsingMemoryCard);
			Console.WriteLine("Create new memcard on playback : {0}", movieFile.UsingClearSaves);
			Console.WriteLine("Bongos                         : {0}", movieFile.Bongos);
			Console.WriteLine("Synchronize GPU                : {0}", movieFile.SyncGPU);
			Console.WriteLine("Using NetPlay                  : {0}", movieFile.UsingNetplay);
			Console.WriteLine("Second disc name               : {0}", movieFile.SecondDiscName);
			Console.WriteLine("Revision                       : {0}", movieFile.GitRevision); // TODO
			Console.WriteLine("DSP IROM hash                  : {0:X}", movieFile.DSPIROMHash);
			Console.WriteLine("DSP coef hash                  : {0:X}", movieFile.DSPCoefHash);
		}
	}
}
