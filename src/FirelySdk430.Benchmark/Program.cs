using System;
using BenchmarkDotNet.Running;

namespace FirelySdk430.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running Firely SDK 4.3.0 JSON Benchmarks");
            var summary = BenchmarkRunner.Run<FirelySdk430JsonBenchmark>();
            
            Console.WriteLine("Running Firely SDK 4.3.0 FHIRPath Benchmarks");
            summary = BenchmarkRunner.Run<FirelySdk430FhirPathBenchmark>();
        }
    }
}
