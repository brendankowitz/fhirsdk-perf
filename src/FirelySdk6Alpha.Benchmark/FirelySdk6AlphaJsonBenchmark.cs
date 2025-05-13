using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Common;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using System.Text.Json;

namespace FirelySdk6Alpha.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class FirelySdk6AlphaJsonBenchmark
    {
        private readonly FhirJsonDeserializer _jsonDeserializer;
        private readonly FhirJsonSerializer _jsonSerializer;
        private readonly string _patientJson;
        private readonly string _patientBundleJson;
        private Patient _patient;
        private Bundle _bundle;

        public FirelySdk6AlphaJsonBenchmark()
        {
            _jsonDeserializer = new FhirJsonDeserializer(new DeserializerSettings
            {
                AllowUnrecognizedEnums = true,
                AnnotateResourceParseExceptions = true,
            });
            _jsonSerializer = new FhirJsonSerializer();
            _patientJson = TestData.GetPatientJson();
            _patientBundleJson = TestData.GetLargePatientBundle();

            // Pre-parse for serialization benchmarks
            _patient = DeserializePatient();
            _bundle = DeserializeBundle();
        }

        [Benchmark]
        public Patient DeserializePatient()
        {
            return _jsonDeserializer.Deserialize<Patient>(_patientJson);
        }

        [Benchmark]
        public Bundle DeserializeBundle()
        {
            return _jsonDeserializer.Deserialize<Bundle>(_patientBundleJson);
        }

        [Benchmark]
        public string SerializePatient()
        {
            return _jsonSerializer.SerializeToString(_patient);
        }

        [Benchmark]
        public string SerializeBundle()
        {
            return _jsonSerializer.SerializeToString(_bundle);
        }
    }
}
