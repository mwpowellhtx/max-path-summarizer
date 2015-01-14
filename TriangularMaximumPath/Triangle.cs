using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TriangularMaximumPath
{
    internal class Triangle
    {
        /// <summary>
        /// Source functional getter backing field.
        /// </summary>
        private readonly Func<Stream> _source;

        /// <summary>
        /// Separator backing field.
        /// </summary>
        private readonly IEnumerable<char> _separator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source"></param>
        /// <param name="separator"></param>
        internal Triangle(Func<Stream> source, IEnumerable<char> separator = null)
        {
            _source = source;
            _separator = separator ?? new[] {' ', '\r', '\n'};
        }

        /// <summary>
        /// Data backing field.
        /// </summary>
        private long[,] _data;

        /// <summary>
        /// Gets the Data associated with the Triangle.
        /// </summary>
        public long[,] Data
        {
            get
            {
                /* There are not that many ways to represent this data set that
                 * efficiently without introducing collections, graphs, trees,
                 * etc; especially if the set of questions is broader than a
                 * 'max-path-sum', i.e. we wanted to know what the path was,
                 * and so on. */
                return _data ?? (_data = Parse(ReadLines().ToArray()));
            }
        }

        /// <summary>
        /// Reads the lines from the Stream.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> ReadLines()
        {
            using (var stream = _source())
            {
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLineAsync().Result) != null)
                        yield return line;
                }
            }
        }

        /// <summary>
        /// Parses the <see cref="lines"/> into a two-dimensional array.
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private long[,] Parse(params string[] lines)
        {
            var length = lines.Length;
            var parsed = new long[length, length];

            var sep = _separator.ToArray();

            for (var i = 0; i < lines.Length; i++)
            {
                var values = lines[i].Split(sep)
                    .Where(x => !string.IsNullOrEmpty(x)).Select(long.Parse).ToArray();

                if (values.Count() - 1 != i)
                    throw new InvalidOperationException(
                        string.Format(@"Unable to read line {0}", i + 1));

                values.Aggregate(0, (j, x) =>
                {
                    parsed[i, j] = x;
                    return ++j;
                });
            }

            return parsed;
        }

        /// <summary>
        /// Summarizes the Triangle solution.
        /// </summary>
        /// <returns></returns>
        /// <remarks>This algorithm summarizes from the bottom up, which is
        /// the most efficient. We really cannot go the other direction all
        /// that effectively without blowing up the scope much more than it
        /// needs to be. If the question was, rather, find the PATHS themselves,
        /// that would be a much different looking animal requiring graphs,
        /// trees, etc.</remarks>
        internal long Summarize()
        {
            var data = Data;
            var length = data.GetLength(0);

            //Decompose to and rebuild from smallest possible sub problems.
            for (var i = length - 2; i >= 0; i--)
            {
                for (var j = 0; j <= i; j++)
                    data[i, j] += Math.Max(data[i + 1, j], data[i + 1, j + 1]);
            }

            return data[0, 0];
        }
    }
}
