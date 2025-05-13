using System;
using BenchmarkDotNet.Running;

namespace FirelySdk6Alpha.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running Firely SDK 6.0.0-alpha2 JSON Benchmarks");
            var summary = BenchmarkRunner.Run<FirelySdk6AlphaJsonBenchmark>();
            
            Console.WriteLine("Running Firely SDK 6.0.0-alpha2 FHIRPath Benchmarks");
            summary = BenchmarkRunner.Run<FirelySdk6AlphaFhirPathBenchmark>();
        }
    }
}
