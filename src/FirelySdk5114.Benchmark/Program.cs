using System;
using BenchmarkDotNet.Running;

namespace FirelySdk5114.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running Firely SDK 5.11.4 JSON Benchmarks");
            var summary = BenchmarkRunner.Run<FirelySdk5114JsonBenchmark>();
            
            Console.WriteLine("Running Firely SDK 5.11.4 FHIRPath Benchmarks");
            summary = BenchmarkRunner.Run<FirelySdk5114FhirPathBenchmark>();
        }
    }
}
