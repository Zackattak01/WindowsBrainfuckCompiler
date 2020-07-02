using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BrainfuckCompiler
{
	public class OptimizeProcess
	{
		private List<char> commands; 

		public OptimizeProcess(List<char> commands)
		{
			this.commands = commands;
		}

		public List<string> Optimize()
		{
			ReplaceKnownConstructs();
			RemoveDeadCode();

			return CondenseCommands();
		}

		private void ReplaceKnownConstructs()
		{
			//we convert to string to easily use the replace functionality to convert known constructs
			string commandsAsString = new string(commands.ToArray());

			//remove all empty loops
			while (commandsAsString.Contains("[]"))
				commandsAsString = commandsAsString.Replace("[]", "");

			commandsAsString = commandsAsString.Replace("[-]", "0");
			commandsAsString = commandsAsString.Replace("[+]", "0");




			commands = commandsAsString.ToList();
			
		}

		//TODO: Emulate loop functionality
		private void RemoveDeadCode()
		{
			List<char> optimizedCommands = new List<char>();

			char previousCommand = '\0';

			bool InDeadCode = false;
			bool LoopsAreDeadCode = true;

			int scopeLevel = 0;
			int scopeLevelOfDeadCode = 0;

			foreach (var command in commands)
			{
				if ((command == '+' || command == '-') && InDeadCode && scopeLevel == 0)
					LoopsAreDeadCode = false;

				if(command == '[')
				{
					scopeLevel++;

					if(!InDeadCode && !LoopsAreDeadCode && previousCommand == '0' || previousCommand == ']')
					{
						scopeLevelOfDeadCode = scopeLevel;
						InDeadCode = true;
					}

				}
				

				if (!InDeadCode)
					optimizedCommands.Add(command);

				if (command == ']')
				{
					if (scopeLevel == scopeLevelOfDeadCode)
					{
						InDeadCode = false;
						scopeLevelOfDeadCode = 0;
					}

					scopeLevel--;
				} 

				previousCommand = command;
	

			}
			commands = optimizedCommands;

		}		

		private List<string> CondenseCommands()
		{
			List<string> CodensedCommands = new List<string>();

			char previousCommand = '\0';

			int consecutiveStatements = 1;

			//add a null character so that the foreach loop can access all the acutal commands
			commands.Add('\0');

			foreach (var command in commands)
			{
				bool breakScope = false;

				if (command == '[' || command == ']' || command == '.' || command == ',')
					breakScope = true;

				if (command == previousCommand && !breakScope)
					consecutiveStatements++;
				else if (command != previousCommand && consecutiveStatements > 1)
				{
					switch (previousCommand)
					{
						case '>':
							CodensedCommands.Add(">" + consecutiveStatements);
							break;
						case '<':
							CodensedCommands.Add("<" + consecutiveStatements);
							break;
						case '+':
							CodensedCommands.Add("+" + consecutiveStatements);
							break;
						case '-':
							CodensedCommands.Add("-" + consecutiveStatements);
							break;
						default:
							break;
					}
					consecutiveStatements = 1;
				}
				else
					CodensedCommands.Add(previousCommand.ToString());

				previousCommand = command;
			}

			

			return CodensedCommands;
		}

		class EmulationData
		{
			public bool CellIsRuntimeDependent { get; set; } = false;
			public byte Value { get; set; } = 0;

			/*public EmulationData()
			{
				CellIsRuntimeDependent = false;
				Value = 0;
			}*/
		}

	
	}
}
