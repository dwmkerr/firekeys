using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FireKeys
{
    /// <summary>
    /// The Arguments class handles arguments provided to a console application.
    /// </summary>
    public class Arguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments"/> class, from the selected arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public Arguments(IEnumerable<string> args)
        {
            //  Store the parameters. 
            parameters = new Dictionary<string, string>();

            var spliter = new Regex(@"^-{1,2}|^/|=|:",
                                    RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var remover = new Regex(@"^['""]?(.*?)['""]?$",
                                    RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string parameter = null;

            // Valid parameters forms:
            // {-,/,--}param{ ,=,:}((",')value(",'))
            // Examples: 
            // -param1 value1 --param2 /param3:"Test-:-work" 
            //   /param4=happy -param5 '--=nice=--'
            foreach (string txt in args)
            {
                // Look for new parameters (-,/ or --) and a
                // possible enclosed value (=,:)
                string[] parts = spliter.Split(txt, 3);

                switch (parts.Length)
                {
                    // Found a value (for the last parameter 
                    // found (space separator))
                    case 1:
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                            {
                                parts[0] =
                                    remover.Replace(parts[0], "$1");

                                parameters.Add(parameter, parts[0]);
                            }
                            parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)
                        break;

                    // Found just a parameter
                    case 2:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                                parameters.Add(parameter, "true");
                        }
                        parameter = parts[1];
                        break;

                    // Parameter with enclosed value
                    case 3:
                        // The last parameter is still waiting. 
                        // With no value, set it to true.
                        if (parameter != null)
                        {
                            if (!parameters.ContainsKey(parameter))
                                parameters.Add(parameter, "true");
                        }

                        parameter = parts[1];

                        // Remove possible enclosing characters (",')
                        if (!parameters.ContainsKey(parameter))
                        {
                            parts[2] = remover.Replace(parts[2], "$1");
                            parameters.Add(parameter, parts[2]);
                        }

                        parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting
            if (parameter != null)
            {
                if (!parameters.ContainsKey(parameter))
                    parameters.Add(parameter, "true");
            }
        }

        /// <summary>
        /// The internal collection of parameters.
        /// </summary>
        private readonly Dictionary<string, string> parameters;
        
        /// <summary>
        /// Gets the <see cref="System.String"/> argument with the specified name.
        /// </summary>
        public string this[string param]
        {
            get
            {
                return parameters.ContainsKey(param) ? parameters[param] : string.Empty;
            }
        }

        public void Dump()
        {
            foreach(var entry in parameters)
                Console.WriteLine(entry.Key + ": " + entry.Value);
        }

        /// <summary>
        /// Determines whether the arguments contains the specified argument.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>
        ///   <c>true</c> if the arguments contains the specified argument; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string param)
        {
            return parameters.ContainsKey(param);
        }

        /// <summary>
        /// Gets the count of the arguments.
        /// </summary>
        public int Count
        {
            get { return parameters != null ? parameters.Count : 0; }
        }
    }
}
