using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
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
        private readonly FhirJsonPocoDeserializer _jsonPocoParser;
        private readonly FhirJsonPocoDeserializer _jsonPocoParserExpress;

        public FirelySdk430JsonBenchmark()
        {
            // The API for the parser/serializer has changed slightly in the newer version
            _jsonParser = new FhirJsonParser(new ParserSettings
            {
                AllowUnrecognizedEnums = true,
                PermissiveParsing = true,
            });
            _jsonSerializer = new FhirJsonSerializer();

            _jsonPocoParser = new FhirJsonPocoDeserializer(typeof(Patient).Assembly);
            _jsonPocoParserExpress = new FhirJsonPocoDeserializer(
                typeof(Patient).Assembly,
                new FhirJsonPocoDeserializerSettings(){
                Validator = null, // don't perform any validation during parsing - express path
            });

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
        public Patient DeserializePatientPoco()
        {
            var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(_patientJson)));
            return _jsonPocoParser.DeserializeResource(ref reader) as Patient;
        }

        [Benchmark]
        public Bundle DeserializeBundlePoco()
        {
            var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(_patientBundleJson)));
            return _jsonPocoParser.DeserializeResource(ref reader) as Bundle;
        }

        [Benchmark]
        public Patient DeserializePatientPocoExpress()
        {
            var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(_patientJson)));
            return _jsonPocoParserExpress.DeserializeResource(ref reader) as Patient;
        }

        [Benchmark]
        public Bundle DeserializeBundlePocoExpress()
        {
            var reader = new Utf8JsonReader(new ReadOnlySpan<byte>(Encoding.UTF8.GetBytes(_patientBundleJson)));
            return _jsonPocoParserExpress.DeserializeResource(ref reader) as Bundle;
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
