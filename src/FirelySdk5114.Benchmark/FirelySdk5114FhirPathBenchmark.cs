using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Common;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.FhirPath;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.FhirPath;

namespace FirelySdk5114.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class FirelySdk5114FhirPathBenchmark
    {
        private readonly FhirJsonParser _jsonParser;
        private Patient _patient;
        private Bundle _bundle;

        // Cache for FHIRPath expressions
        private FhirPathCompiler _compiler;
        private readonly ITypedElement _patientElement;

        public FirelySdk5114FhirPathBenchmark()
        {
            _jsonParser = new FhirJsonParser();
            _patient = _jsonParser.Parse<Patient>(TestData.GetPatientJson());
            _bundle = _jsonParser.Parse<Bundle>(TestData.GetLargePatientBundle());
            _patientElement = _patient.ToTypedElement();

            // Initialize FHIRPath compiler
            _compiler = new FhirPathCompiler();
        }

        [Benchmark]
        public object EvaluateSimplePatientExpression()
        {
            // A simple expression to get the patient's name
            return _patient.Select("name.given");
        }

        [Benchmark]
        public object EvaluateComplexPatientExpression()
        {
            // A more complex expression that filters telecom entries
            return _patient.Select("telecom.where(system = 'phone' and use = 'mobile')");
        }

        [Benchmark]
        public object EvaluateBundleExpression()
        {
            // Query to extract all patient names from the bundle
            return _bundle.Select("entry.resource.ofType(Patient).name.given");
        }

        [Benchmark]
        public object EvaluateWithPreCompiledExpression()
        {
            // Using pre-compiled expression which is how the SDK caches FHIRPath expressions
            var expression = _compiler.Compile("name.where(use = 'official').given");
            return expression.Invoke(_patientElement, new EvaluationContext());
        }
    }
}
