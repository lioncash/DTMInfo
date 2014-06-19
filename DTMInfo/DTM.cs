using System;
using System.IO;
using System.Text;

namespace DTMInfo
{
	/// <summary>
	/// Dolphin Movie File.
	/// </summary>
	public sealed class DTM
	{
		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="filename">Path to the DTM movie file.</param>
		public DTM(string filename)
		{
			if (filename == null)
				throw new ArgumentNullException("filename");

			if (!File.Exists(filename))
				throw new IOException("Specified file does not exist.");

			BinaryReader reader = new BinaryReader(File.OpenRead(filename));

			byte[] fileMagic = reader.ReadBytes(4);
			if (!IsValidHeaderID(fileMagic))
				throw new IOException("Invalid file: Header magic is incorrect.");

			GameID                      = Encoding.UTF8.GetString(reader.ReadBytes(6));
			IsWiiGame                   = reader.ReadBoolean();
			ConnectedControllers        = reader.ReadByte();
			IsFromSaveState             = reader.ReadBoolean();
			FrameCount                  = reader.ReadUInt64();
			InputFrameCount             = reader.ReadUInt64();
			LagFrameCount               = reader.ReadUInt64();
			UniqueID                    = reader.ReadUInt64();
			NumRerecords                = reader.ReadUInt32();
			Author                      = Encoding.UTF8.GetString(reader.ReadBytes(32));
			VideoBackend                = Encoding.UTF8.GetString(reader.ReadBytes(16));
			AudioEmulator               = Encoding.UTF8.GetString(reader.ReadBytes(16));
			MD5                         = ExtractRevisionOrMD5(reader.ReadBytes(16));
			RecordingStartTime          = reader.ReadUInt64();
			IsSavedConfig               = reader.ReadBoolean();
			UsingIdleSkip               = reader.ReadBoolean();
			UsingDualCore               = reader.ReadBoolean();
			UsingProgressiveScan        = reader.ReadBoolean();
			UsingHLEDSP                 = reader.ReadBoolean();
			UsingFastDiscSpeed          = reader.ReadBoolean();
			CPUCore                     = GetCPUCoreName(reader.ReadByte());
			IsEFBAccessEnabled          = reader.ReadBoolean();
			IsEFBCopiesEnabled          = reader.ReadBoolean();
			UsingEFBToTexture           = reader.ReadBoolean();
			IsEFBCopyCacheEnabled       = reader.ReadBoolean();
			IsEmulatingEFBFormatChanges = reader.ReadBoolean();
			UsingXFB                    = reader.ReadBoolean();
			UsingRealXFB                = reader.ReadBoolean();
			UsingMemoryCard             = reader.ReadBoolean();
			UsingClearSaves             = reader.ReadBoolean();
			Bongos                      = reader.ReadByte();
			SyncGPU                     = reader.ReadBoolean();
			UsingNetplay                = reader.ReadBoolean();

			// Skip reserved bytes
			reader.BaseStream.Position += 13;

			SecondDiscName              = Encoding.UTF8.GetString(reader.ReadBytes(40));
			GitRevision                 = ExtractRevisionOrMD5(reader.ReadBytes(20)); // TODO: This will likely need to be manually converted.
			DSPIROMHash                 = reader.ReadUInt32();
			DSPCoefHash                 = reader.ReadUInt32();

			// Skip reserved bytes.
			reader.BaseStream.Position += 19;
		}

		#endregion

		#region Properties

		/// <summary>
		/// ID of the game that this movie was made for.
		/// </summary>
		public string GameID { get; private set; }

		/// <summary>
		/// Whether or not this movie file is for a Wii game.
		/// </summary>
		public bool IsWiiGame { get; private set; }

		/// <summary>
		/// Number of connected controllers (1-4)
		/// </summary>
		public int ConnectedControllers { get; private set; }

		/// <summary>
		/// false indicates that the recording started from bootup, true for savestate
		/// </summary>
		public bool IsFromSaveState { get; private set; }

		/// <summary>
		/// Number of frames in the recording.
		/// </summary>
		public ulong FrameCount { get; private set; }

		/// <summary>
		/// Number of input frames in the recording.
		/// </summary>
		public ulong InputFrameCount { get; private set; }

		/// <summary>
		/// Number of lag frames in the recording.
		/// </summary>
		public ulong LagFrameCount { get; private set; }

		/// <summary>
		/// (not implemented) A Unique ID comprised of: md5(time + Game ID)
		/// </summary>
		public ulong UniqueID { get; private set; }

		/// <summary>
		/// Number of rerecords/'cuts' of this TAS
		/// </summary>
		public uint NumRerecords { get; private set; }

		/// <summary>
		/// Author's name
		/// </summary>
		public string Author { get; private set; }

		/// <summary>
		/// Name of the video backend used.
		/// </summary>
		public string VideoBackend { get; private set; }

		/// <summary>
		/// Name of the audio emulator used.
		/// </summary>
		public string AudioEmulator { get; private set; }

		/// <summary>
		/// MD5 of the game ISO.
		/// </summary>
		public string MD5 { get; private set; }

		/// <summary>
		/// Seconds since 1970 that recording started (used for RTC).
		/// </summary>
		public ulong RecordingStartTime { get; private set; }

		/// <summary>
		/// Whether or not certain settings are loaded.
		/// </summary>
		/// <remarks>
		/// All properties following this one are loaded during startup if true.
		/// </remarks>
		public bool IsSavedConfig { get; private set; }


		public bool UsingIdleSkip               { get; private set; }
		public bool UsingDualCore               { get; private set; }
		public bool UsingProgressiveScan        { get; private set; }
		public bool UsingHLEDSP                 { get; private set; }
		public bool UsingFastDiscSpeed          { get; private set; }

		/// <summary>CPU core type used. 0 = Interpreter, 1 = JIT, 2 = JITIL</summary>
		public string CPUCore                   { get; private set; }
		public bool IsEFBAccessEnabled          { get; private set; }
		public bool IsEFBCopiesEnabled          { get; private set; }
		public bool UsingEFBToTexture           { get; private set; }
		public bool IsEFBCopyCacheEnabled       { get; private set; }
		public bool IsEmulatingEFBFormatChanges { get; private set; }
		public bool UsingXFB                    { get; private set; }
		public bool UsingRealXFB                { get; private set; }
		public bool UsingMemoryCard             { get; private set; }

		/// <summary>Create a new memory card when playing back a movie if true</summary>
		public bool UsingClearSaves             { get; private set; }
		public byte Bongos                      { get; private set; }
		public bool SyncGPU                     { get; private set; }
		public bool UsingNetplay                { get; private set; }

		/// <summary>Name of ISO file to switch to for two disc games.</summary>
		public string SecondDiscName            { get; private set; }
		public string GitRevision               { get; private set; }
		public uint DSPIROMHash                 { get; private set; }
		public uint DSPCoefHash                 { get; private set; }
		public byte Reserved2                   { get; private set; }

		#endregion

		#region Utility Methods

		private static bool IsValidHeaderID(byte[] data)
		{
			return data[0] == 'D' &&
			       data[1] == 'T' &&
			       data[2] == 'M' &&
			       data[3] == 0x1A;
		}

		private static string GetCPUCoreName(byte value)
		{
			switch (value)
			{
				case 0:
					return "Interpreter";
				
				case 1:
					return "JIT";

				case 2:
					return "JITIL";

				default:
					return "Unknown";
			}
		}

		private static string ExtractRevisionOrMD5(byte[] data)
		{
			StringBuilder sb = new StringBuilder(256);

			foreach (byte b in data)
			{
				sb.Append(b.ToString("X"));
			}

			return sb.ToString();
		}

		#endregion
	}
}
