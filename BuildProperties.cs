using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace BrainfuckCompiler
{
	public class BuildProperties
	{

		public string FileName { get; private set; }

		public IOMode IOMode { get; private set; } = IOMode.File;

		public bool LeaveCSource { get; private set; } = false;

		/*
		 * Flags:
		 * -cio: Specify CommandLineIO
		 * -fio: Specify FileIO (default)
		 * -s  : Leave the C source file (named temp.c) instead of deleting it
		 */

		public static BuildProperties Create(string[] args)
		{
			BuildProperties buildProperties = new BuildProperties();

			//Checks to make sure that at least a source file was provided
			if (args.Length == 0)
			{
				Console.WriteLine("Please provide a source file.");
				Environment.Exit(-1);
			}

			//Determine file name location
			int FileNameLocation = DetermineFileNameLocation(args);

			if(FileNameLocation == -1)
			{
				Console.WriteLine("Please provide a source file.");
				Environment.Exit(-1);
			}

			buildProperties.FileName = args[FileNameLocation];

			if (!File.Exists(buildProperties.FileName))
			{
				Console.WriteLine("Please provide a source file.");
				Environment.Exit(-1);
			}

			foreach (var arg in args)
			{
				switch (arg.ToLower())
				{
					case "-cio":
						buildProperties.IOMode = IOMode.Console;
						break;
					case "-fio":
						buildProperties.IOMode = IOMode.File;
						break;
					case "-s":
						buildProperties.LeaveCSource = true;
						break;

				}
			}

			return buildProperties;
		}

		//I understand this method isn't a good way to determine where the file is
		private static int DetermineFileNameLocation(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].Contains('.'))
					return i;
			}

			return -1;
		}
	}

	public enum IOMode
	{
		File,
		Console
	}
}
