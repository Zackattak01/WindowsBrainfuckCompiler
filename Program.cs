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

			List<char> commands = GetCommandsFromSource(buildProperties.FileName);


			

			StreamWriter sw = new StreamWriter(File.Create("temp.c"));

			//#include <stdio.h> and <stdlib.h>
			sw.WriteLine("#include <stdio.h>");
			sw.WriteLine("#include <stdlib.h>");
			sw.WriteLine("#include <conio.h>");

			//creates the tape in c
			sw.WriteLine("unsigned char tape[30000];");

			//creates the pointer in c
			sw.WriteLine("unsigned char *i;");

			//creates the int main() boilerplate code
			sw.WriteLine("int main(){");

			//get the input and output files if applicable
			if (buildProperties.IOMode == IOMode.File)
			{
				sw.WriteLine($"FILE *out = fopen(\"{buildProperties.FileName.Split('.')[0]}_output.txt\", \"w\");");
				sw.WriteLine($"FILE *in = fopen(\"{buildProperties.FileName.Split('.')[0]}_input.txt\", \"r\");");
			}
				

			sw.WriteLine("i = tape;");

			foreach (var command in commands)
			{
				switch (command)
				{
					case '>':
						sw.WriteLine("i++;");
						sw.WriteLine("if(i < tape){i=tape;}");
						break;
					case '<':
						sw.WriteLine("i--;");
						sw.WriteLine("if(i > tape + 30000){i=tape + 30000;}");
						break;
					case '+':
						sw.WriteLine("(*i)++;");
						break;
					case '-':
						sw.WriteLine("(*i)--;");
						break;
					case '.':
						if (buildProperties.IOMode == IOMode.Console)
							sw.WriteLine("printf(\"%c\",(*i));");
						else
							sw.WriteLine("fwrite(i, 1, 1, out);");
						break;
					case ',':
						if (buildProperties.IOMode == IOMode.Console)
							sw.WriteLine("(*i)=getch();");
						else
							sw.WriteLine("fread(i, 1, 1, in);");
						break;
					case '[':
						sw.WriteLine("while((*i) != 0){");
						break;
					case ']':
						sw.WriteLine("}");
						break;
					default:
						break;
				}
			}

			//more boilerplate
			if (buildProperties.IOMode == IOMode.File)
			{
				sw.WriteLine("fclose(out);");
				sw.WriteLine("fclose(in);");
			}

			sw.WriteLine("return 0;");
			sw.WriteLine("}");

			sw.Close();

			//Compile the file using a built in batch script
			string ExecutablePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			var buildScript = new ProcessStartInfo("cmd.exe", $"/C {ExecutablePath}/build.bat {buildProperties.FileName.Split('.')[0]} {buildProperties.LeaveCSource}")
			{
				CreateNoWindow = false
			};

			Process.Start(buildScript);

		}

		static bool IsCommand(char c)
		{
			switch (c)
			{
				case '>':
					return true;
				case '<':
					return true;
				case '+':
					return true;
				case '-':
					return true;
				case '.':
					return true;
				case ',':
					return true;
				case '[':
					return true;
				case ']':
					return true;
				default:
					return false;
			}
		}

		static List<char> GetCommandsFromSource(string fileName)
		{
			List<char> commands = new List<char>();

			int openBracketCounter = 0;
			int closeBracketCounter = 0;


			using (FileStream fs = File.OpenRead(fileName))
			{
				byte[] buf = new byte[fs.Length];
				int c;




				while ((c = fs.Read(buf, 0, buf.Length)) > 0)
				{
					foreach (var command in Encoding.ASCII.GetChars(buf, 0, c))
					{
						if (IsCommand(command))
						{
							commands.Add(command);

							if (command == '[')
								openBracketCounter++;
							else if (command == ']')
								closeBracketCounter++;
						}


					}

				}

				if (openBracketCounter != closeBracketCounter)
				{
					Console.WriteLine("Error:  [ count != ] count.  Make sure that every '[' has a corresponding ']'");
					Environment.Exit(-1);
				}

				return commands;
			}
		}
	}


}
