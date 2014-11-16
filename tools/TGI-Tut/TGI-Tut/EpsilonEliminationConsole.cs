using Automaton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGI_Tut
{
    class EpsilonEliminationConsole
    {
        /// <summary>
        /// The main function.
        /// </summary>
        /// <param name="args">The arguments are ignored.</param>
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                NFA nfa = ReadNfaFromUserinput();

                Console.WriteLine(nfa.ToString());

                EpsilonStatements abschluss = new EpsilonStatements(nfa);

                Console.WriteLine(abschluss.ToString());

                NFA result = EliminateEpsilon.EliminateEpsilonOn(nfa, false);

                Console.WriteLine(result.ToString());

                Console.WriteLine("Exit program? [y/n]");

                char input = Console.ReadKey().KeyChar;
                while (input != 'n' && input != 'y')
                {
                    input = Console.ReadKey().KeyChar;
                }
                if (input == 'y')
                {
                    exit = true;
                }
            }
        }

        /// <summary>
        /// Let the user insert states by using <q0> <input> -> <q1>
        /// </summary>
        /// <returns>The nfa constructed from the user input.</returns>
        static NFA ReadNfaFromUserinput()
        {
            NFA nfa = new NFA();
            bool exit = false;
            Console.WriteLine("States: implicitly defined.");
            Console.WriteLine("Alphabet: implicitly defined.");

            Console.Write("start state > ");
            nfa.start = Console.ReadLine();

            Console.Write("end states (separate by space) > ");
            nfa.end = new List<string>();
            nfa.end.AddRange(Console.ReadLine().Split(' '));

            Console.WriteLine("transition function:");
            Console.WriteLine("Write \"q0 a q1;q1 b q0;...;q2 \\e q3\" in one line. \\e represents epsilon.");
            Console.WriteLine("You can input multiple lines.");
            Console.WriteLine("writing \"exit\" in a line stops input mode.");
            while (!exit)
            {
                Console.Write("> ");
                string inputLine = Console.ReadLine();
                if (inputLine == "exit")
                {
                    exit = true;
                    continue;
                }
                if (inputLine == "")
                {
                    continue;
                }
                string[] stateTransitions = inputLine.Split(';');
                foreach (string s in stateTransitions)
                {
                    string[] split = s.Split(' ');
                    string first = split[0];
                    string input = split[1];
                    string second = split[2];

                    if (!nfa.Transitions.ContainsKey(first))
                    {
                        nfa.Transitions[first] = new State(first);
                    }

                    if (!nfa.Transitions.ContainsKey(second))
                    {
                        nfa.Transitions[second] = new State(second);
                    }
                    if (!nfa.Transitions[first].StateTransitions.ContainsKey(input))
                    {
                        nfa.Transitions[first].StateTransitions[input] = new List<string>();
                    }

                    nfa.Transitions[first].StateTransitions[input].Add(second);
                }
            }
            return nfa;
        }
    }
}
