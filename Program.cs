using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BrainfuckCompiler
{
	class Program
	{
		static void Main(string[] args)
		{
			BuildProperties buildProperties = new BuildProperties(args);

			BuildProcess buildProcess = new BuildProcess(buildProperties);
			buildProcess.Build();

			

		}

		
	}


}
