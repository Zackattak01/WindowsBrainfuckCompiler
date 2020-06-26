using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BrainfuckCompiler
{
	public class BuildProcess
	{
		private BuildProperties buildProperties;

		public BuildProcess(BuildProperties buildProperties)
			=> this.buildProperties = buildProperties;

		public void Build()
		{
			List<char> commands = GetCommandsFromSource(buildProperties.FileName);

			List<string> cStatements = TranslateCommandsToC(commands);

			//write optimization code here

			WriteCToFile(cStatements);

			CompileC();
		}

		private bool IsCommand(char c)
		{
			return c switch
			{
				'>' => true,
				'<' => true,
				'+' => true,
				'-' => true,
				'.' => true,
				',' => true,
				'[' => true,
				']' => true,
				_ => false,
			};
		}

		private List<char> GetCommandsFromSource(string fileName)
		{
			List<char> commands = new List<char>();

			int openBracketCounter = 0;
			int closeBracketCounter = 0;


			using FileStream fs = File.OpenRead(fileName);
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

		private List<string> TranslateCommandsToC(IEnumerable<char> commands)
		{
			List<string> cCode = new List<string>();

			//#include <stdio.h> and <stdlib.h>
			cCode.Add("#include <stdio.h>");
			cCode.Add("#include <stdlib.h>");
			cCode.Add("#include <conio.h>");

			//creates the tape in c
			cCode.Add("unsigned char tape[30000];");

			//creates the pointer in c
			cCode.Add("unsigned char *i;");

			//creates the int main() boilerplate code
			cCode.Add("int main(){");

			//get the input and output files if applicable
			if (buildProperties.IOMode == IOMode.File)
			{
				cCode.Add($"FILE *out = fopen(\"{buildProperties.FileName.Split('.')[0]}_output.txt\", \"w\");");
				cCode.Add($"FILE *in = fopen(\"{buildProperties.FileName.Split('.')[0]}_input.txt\", \"r\");");
			}


			cCode.Add("i = tape;");

			foreach (var command in commands)
			{
				switch (command)
				{
					case '>':
						cCode.Add("i++;");
						cCode.Add("if(i > tape + 30000){i=tape + 30000;}");
						break;
					case '<':
						cCode.Add("i--;");
						cCode.Add("if(i < tape){i=tape;}");
						break;
					case '+':
						cCode.Add("(*i)++;");
						break;
					case '-':
						cCode.Add("(*i)--;");
						break;
					case '.':
						if (buildProperties.IOMode == IOMode.Console)
							cCode.Add("printf(\"%c\",(*i));");
						else
							cCode.Add("fwrite(i, 1, 1, out);");
						break;
					case ',':
						if (buildProperties.IOMode == IOMode.Console)
							cCode.Add("(*i)=getch();");
						else
							cCode.Add("fread(i, 1, 1, in);");
						break;
					case '[':
						cCode.Add("while((*i) != 0){");
						break;
					case ']':
						cCode.Add("}");
						break;
					default:
						break;
				}
			}

			//more boilerplate
			if (buildProperties.IOMode == IOMode.File)
			{
				cCode.Add("fclose(out);");
				cCode.Add("fclose(in);");
			}

			cCode.Add("return 0;");
			cCode.Add("}");

			return cCode;
		}

		private void WriteCToFile(IEnumerable<string> CStatements)
		{
			StreamWriter sw = new StreamWriter(File.Create("temp.c"));

			foreach (var statement in CStatements)
			{
				sw.WriteLine(statement);
			}

			sw.Close();
		}

		private void CompileC()
		{
			//Compile the file using a built in batch script
			string ExecutablePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			var buildScript = new ProcessStartInfo("cmd.exe", $"/C {ExecutablePath}/build.bat {buildProperties.FileName.Split('.')[0]} {buildProperties.LeaveCSource}");

			Process.Start(buildScript);
		}
	}
}
