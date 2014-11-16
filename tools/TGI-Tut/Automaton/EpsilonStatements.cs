using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automaton;

namespace Automaton
{
    /// <summary>
    /// Models epsilon statements.
    /// </summary>
    public class EpsilonStatements
    {
        /// <summary>
        /// The statements for a given state.
        /// </summary>
        public Dictionary<string, List<string>> Statements {get;set;}

        /// <summary>
        /// Calculate the epsilon statements of an automaton.
        /// </summary>
        /// <param name="nfa">The nfa to operate on.</param>
        public EpsilonStatements(NFA nfa)
        {
            Statements = new Dictionary<string, List<string>>();

            // Initialize the dictionary.
            InitializeDictionary(Statements, nfa.Transitions);

            // Calculate epsilon statements.
            foreach(var pair in nfa.Transitions)
            {
                Statements[pair.Key].Add(nfa.Transitions[pair.Key].Name);
                FollowAllEpsilonRecursive(pair.Key, pair.Value, nfa);
            }
        }

        /// <summary>
        /// Recursively trace the epsilon statements of a state.
        /// </summary>
        /// <param name="origin">The origin is used to determine wich states epsilon statements are calculated.</param>
        /// <param name="currentState">The state currently visited.</param>
        /// <param name="nfa">The nfa to operate on.</param>
        private void FollowAllEpsilonRecursive(string origin, State currentState, NFA nfa)
        {
            // Follow all edges of the automaton graph.
            foreach (var transition in currentState.StateTransitions)
            {
                foreach (var destination in transition.Value)
                {
                    // Only look at those with an epsilon and avoid looping recursion.
                    if (transition.Key == "\\e" && !Statements[origin].Contains(destination))
                    {
                        Statements[origin].Add(destination);
                        FollowAllEpsilonRecursive(origin, nfa.Transitions[destination], nfa);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize a dictionary with the keys of another.
        /// </summary>
        /// <param name="dict1"></param>
        /// <param name="dict2"></param>
        private void InitializeDictionary<K, S, T>(Dictionary<K, List<S>> dict1, Dictionary<K, T> dict2)
        {
            foreach (var pair in dict2)
            {
                dict1[pair.Key] = new List<S>();
            }
        }

        /// <summary>
        /// Convert the object into a string.
        /// </summary>
        /// <returns>The object as a string.</returns>
        override public string ToString()
        {
            string res = "";
            foreach (var pair in Statements)
            {
                res += ("E(" + pair.Key + ") = {" + Flatten(pair.Value) + "}") + "\n";
            }
            return res;
        }

        /// <summary>
        /// Flatten a list of strings to a single string.
        /// </summary>
        /// <param name="list">The list to flatten.</param>
        /// <returns>A string representing the list.</returns>
        private string Flatten(List<string> list)
        {
            string result = "";
            bool first = true;

            foreach (string s in list)
            {
                if (first)
                {
                    result += s;
                    first = false;
                }
                else
                {
                    result += " " + s;
                }
            }
            return result;
        }
    }
}
