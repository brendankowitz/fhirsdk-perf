using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Common;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;

namespace FirelySdk430.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class FirelySdk430JsonBenchmark
    {
        private readonly FhirJsonParser _jsonParser;
        private readonly FhirJsonSerializer _jsonSerializer;
        private readonly string _patientJson;
        private readonly string _patientBundleJson;
        private Patient _patient;
        private Bundle _bundle;
        private readonly ITypedElement _patientEl;

        public FirelySdk430JsonBenchmark()
        {
            _jsonParser = new FhirJsonParser(new ParserSettings
            {
                AllowUnrecognizedEnums = true,
                PermissiveParsing = true,
            });
            _jsonSerializer = new FhirJsonSerializer();
            _patientJson = TestData.GetPatientJson();
            _patientBundleJson = TestData.GetLargePatientBundle();

            // Pre-parse for serialization benchmarks
            _patient = _jsonParser.Parse<Patient>(_patientJson);
            _bundle = _jsonParser.Parse<Bundle>(_patientBundleJson);

            _patientEl = _patient.ToTypedElement();
        }

        [Benchmark]
        public Patient DeserializePatient()
        {
            return _jsonParser.Parse<Patient>(_patientJson);
        }

        [Benchmark]
        public Bundle DeserializeBundle()
        {
            return _jsonParser.Parse<Bundle>(_patientBundleJson);
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

        [Benchmark]
        public object EvaluateToTypedElement()
        {
            // Query to extract all patient names from the bundle
            return _patient.ToTypedElement();
        }

        [Benchmark]
        public object EvaluateToPoco()
        {
            // Query to extract all patient names from the bundle
            return _patientEl.ToPoco();
        }
    }
}
