using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaton
{
    /// <summary>
    /// A state of the automaton.
    /// </summary>
    public class State {
        /// <summary>
        /// The name of the state.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Create a new state with a given name.
        /// </summary>
        /// <param name="name">The name to setup for the state.</param>
        public State(string name)
        {
            Name = name;
            StateTransitions = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// Stores the input key as key.
        /// The value maps to the following.
        /// </summary>
        public Dictionary<string, List<string>> StateTransitions {get; set;}
    }

    /// <summary>
    /// A NEA. (Non-deterministic finite automaton)
    /// </summary>
    public class NFA
    {
        /// <summary>
        /// Create a new nfa.
        /// </summary>
        public NFA()
        {
            Transitions = new Dictionary<string,State>();
        }

        /// <summary>
        /// The states of the automaton.
        /// </summary>
        public Dictionary<string, State> Transitions { get; set;}
        public string start { get; set; }
        public List<string> end { get; set; }

        /// <summary>
        /// Create a string representing the automaton.
        /// </summary>
        /// <returns>The string representing the automaton.</returns>
        override public string ToString()
        {
            string res = "";

            res += "start = " + start + "\n";
            res += "end = {" + Flatten(end) + "}\n";
            res += "transitions = {" + Flatten(Transitions) + "}\n";

            return res;
        }

        /// <summary>
        /// Flatten the dict of states given.
        /// </summary>
        /// <param name="dict">The dict of states to flatten.</param>
        /// <returns>A string representing the transitions.</returns>
        private string Flatten(Dictionary<string, State> dict)
        {
            string result = "";
            bool first = true;

            foreach(var state in dict.Values)
            {
                foreach(var transition in state.StateTransitions)
                {
                    if (first)
                    {
                        result += "(" + state.Name + "," + transition.Key + ")->{" + Flatten(transition.Value) + "}";
                        first = false;
                    }
                    else
                    {
                        result += ", (" + state.Name + "," + transition.Key + ")->{" + Flatten(transition.Value) + "}";
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Flatten the list of strings.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
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
