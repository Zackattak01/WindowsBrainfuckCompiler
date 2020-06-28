using System;
using System.Collections.Generic;
using System.Text;

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
			RemoveDeadCode();

			return CondenseCommands();
		}

		private void RemoveDeadCode()
		{
			bool DeadCode = true;
			bool CurrentCellIsRuntimeDependent = false;

			bool AbortOptimizationDueToOutOfBounds = false;

			int i = 0;

			EmulationData[] cells = new EmulationData[30000];


			List<char> optimizedCommands = new List<char>();
			foreach (var command in commands)
			{


				switch (command)
				{
					case '+':
						cells[i].Value++;
						break;
					case '-':
						cells[i].Value--;
						break;
					case '>':
						i++;
						if (i++ >= 30000)
							AbortOptimizationDueToOutOfBounds = true;
						break;
					case '<':
						i--;
						if (i--! <= 0)
							AbortOptimizationDueToOutOfBounds = true;
						break;
					case ',':
						CurrentCellIsRuntimeDependent = true;
						break;
					default:
						break;
				}
				if (AbortOptimizationDueToOutOfBounds)
				{
					//send ominous message so that the user knows they messed up
					Console.WriteLine("WARNING: Code optimization could not be completed because the program supplied goes out of bounds.  Compilation will continue but, the program will most likely act erratically.");
					break;
				}


				//if the cell is dependent on runtime input then there is no optimizaton to be done
				if (cells[i].CellIsRuntimeDependent)
					continue;


			}
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
		}

		/*private List<string> CondenseCStatements(IEnumerable<string> cCode)
		{
			int consecutiveStatements = 1;
			string previousOptimizableStatement = "";

			List<string> optimizedCode = new List<string>();

			foreach (var statement in cCode)
			{
				bool breakScope = false;

				//catch all for anything non-optimizable
				if (statement.Contains("f") || statement.Contains("get")
					|| statement.Contains("while") || statement.Contains("}"))
				{
					breakScope = true;
				}


				//now we can check for optimizability
				if (statement == previousOptimizableStatement && !breakScope)
					consecutiveStatements++;
				else if (statement != previousOptimizableStatement && consecutiveStatements > 1)
				{
					switch (previousOptimizableStatement)
					{
						case "i++;":
							optimizedCode.Add(Optimized.PointerIncrement + consecutiveStatements + ";");
							break;
						case "i--;":
							optimizedCode.Add(Optimized.PointerDecrement + consecutiveStatements + ";");
							break;
						case "(*i)++;":
							optimizedCode.Add(Optimized.CellIncrement + consecutiveStatements + ";");
							break;
						case "(*i)--;":
							optimizedCode.Add(Optimized.CellDecrement + consecutiveStatements + ";");
							break;
					}
					consecutiveStatements = 1;
				}
				else
				{
					optimizedCode.Add(previousOptimizableStatement);
				}



				previousOptimizableStatement = statement;
			}

			//the foreach wont catch the last statment in the list and since we know it always will be the ending '}' for the main function we can just add it here
			optimizedCode.Add("}");



			return optimizedCode;
		}*/

		/* This was the code for dead code detection
		 * if ((command == '+' || command == '-') && loopsAreDeadCode == true)
							loopsAreDeadCode = false;

						if (command == '[' && loopsAreDeadCode)
						{
							scopeIsInDeadCode = true;
							scopeLevel++;
							continue;
						}
						else if (command == ']' && loopsAreDeadCode)
						{
							scopeLevel--;

							if (scopeLevel == 0)
								scopeIsInDeadCode = false;

							continue;
						}
						else if (scopeIsInDeadCode)
							continue;
							*/
	}
}
