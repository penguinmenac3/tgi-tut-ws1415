using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automaton
{
    public class EliminateEpsilon
    {
        /// <summary>
        /// Eliminate all epsilon transitions on a given nfa.
        /// </summary>
        /// <param name="nfa">The nfa to transform.</param>
        /// <param name="noSelfEpsilon">When true the transformation also removes epsilon pointing from a state to a state itself.</param>
        /// <returns>A nfa that has no epsilon transitions except maybe from a state back to itself.</returns>
        public static NFA EliminateEpsilonOn(NFA nfa, bool noSelfEpsilon)
        {
            EpsilonStatements epsilonStatements = new EpsilonStatements(nfa);
            NFA result = new NFA();

            // Q' = Q
            foreach (var state in nfa.Transitions)
            {
                result.Transitions[state.Key] = new State(state.Key);
            }

            // s' = s
            result.start = nfa.start;

            // F' = { q in Q | E(q) combined with F is not empty}
            result.end = new List<string>();
            foreach (var state in nfa.Transitions.Keys)
            {
                if (!EmptyCross(epsilonStatements.Statements[state], nfa.end))
                {
                    result.end.Add(state);
                }
            }

            // transitionFunction'(q, a) = { q } if a = \e
            // transitionFunction'(q, a) = transitionFunction(E(q), a) else
            foreach (var state in nfa.Transitions.Keys)
            {
                var newTransition = result.Transitions[state].StateTransitions;

                // Add the self epsilon like in the tgi lecture only when not explicitly forbidden.
                if (!noSelfEpsilon)
                {
                    newTransition["\\e"] = new List<string>();
                    newTransition["\\e"].Add(state);
                }

                // Look at all states in ea and add their transitions.
                foreach (var tmp in epsilonStatements.Statements[state])
                {
                    // We need the state object and not it's name...
                    State stateObject = nfa.Transitions[tmp];

                    // Now add all transitions except those with epsilon.
                    foreach (var transition in stateObject.StateTransitions)
                    {
                        if (transition.Key != "\\e")
                        {
                            AddToDictionary(newTransition, transition);
                        }
                    }
                }
            }

            // Done
            return result;
        }

        /// <summary>
        /// Calculate if list1 and list2 have no elements in common.
        /// </summary>
        /// <param name="list1">The first list.</param>
        /// <param name="list2">The other list.</param>
        /// <returns>True if the two lists have no elements in common.</returns>
        private static bool EmptyCross(List<string> list1, List<string> list2)
        {
            foreach (var tmp in list1)
            {
                if (list2.Contains(tmp))
                {
                    return false;
                }
            }
            return true;
        }
        
        /// <summary>
        /// Add all elements of a pair with list as value to a dict.
        /// </summary>
        /// <param name="dict">The dictionary where to add the pair.</param>
        /// <param name="pair">The pair that should be added.</param>
        private static void AddToDictionary <K, V>(Dictionary<K, List<V>> dict, KeyValuePair<K, List<V>> pair)
        {
            if (!dict.ContainsKey(pair.Key))
            {
                dict[pair.Key] = new List<V>();
            }
            dict[pair.Key].AddRange(pair.Value);
        }
    }
}
