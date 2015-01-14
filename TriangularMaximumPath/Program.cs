using System;
using System.Diagnostics;

namespace TriangularMaximumPath
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var programType = typeof (Program);
            var assembly = programType.Assembly;

            foreach (var arg in args)
            {
                var sw = Stopwatch.StartNew();

                var path = string.Join(@".", programType.Namespace, @"Cases", arg);
                var triangle = new Triangle(() => assembly.GetManifestResourceStream(path));
                var summary = triangle.Summarize();

                Console.WriteLine(@"arg: {0} summary: {1} in {2} milliseconds",
                    arg, summary, sw.ElapsedMilliseconds);
            }
        }
    }
}
