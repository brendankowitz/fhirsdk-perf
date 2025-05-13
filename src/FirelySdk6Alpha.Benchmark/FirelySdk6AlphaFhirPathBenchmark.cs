using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Common;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.FhirPath;
using Hl7.Fhir.ElementModel;
using System.Text.Json;
using System.Diagnostics;

namespace FirelySdk6Alpha.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class FirelySdk6AlphaFhirPathBenchmark
    {
        private readonly FhirJsonDeserializer _jsonDeserializer;
        private Patient _patient;
        private Bundle _bundle;

        // Cache for FHIRPath expressions
        private FhirPathCompiler _compiler;
        private EvaluationContext _evaluationContext;

        public FirelySdk6AlphaFhirPathBenchmark()
        {
            _jsonDeserializer = new FhirJsonDeserializer();

            // Parse the patient and bundle
            _patient = _jsonDeserializer.Deserialize<Patient>(TestData.GetPatientJson());
            _bundle = _jsonDeserializer.Deserialize<Bundle>(TestData.GetLargePatientBundle());;

            // Initialize FHIRPath compiler and evaluation context
            _compiler = new FhirPathCompiler();
            _evaluationContext = new EvaluationContext();
        }

        [Benchmark]
        public object EvaluateSimplePatientExpression()
        {
            // A simple expression to get the patient's name
#pragma warning disable SDK0001 // API is for evaluation purposes only
            var patientNode = _patient.ToTypedElement();
#pragma warning restore SDK0001
            return patientNode.Select("name.given", _evaluationContext);
        }

        [Benchmark]
        public object EvaluateComplexPatientExpression()
        {
            // A more complex expression that filters telecom entries
#pragma warning disable SDK0001 // API is for evaluation purposes only
            var patientNode = _patient.ToTypedElement();
#pragma warning restore SDK0001
            return patientNode.Select("telecom.where(system = 'phone' and use = 'mobile')", _evaluationContext);
        }

        [Benchmark]
        public object EvaluateBundleExpression()
        {
            // Query to extract all patient names from the bundle
#pragma warning disable SDK0001 // API is for evaluation purposes only
            var bundleNode = _bundle.ToTypedElement();
#pragma warning restore SDK0001
            return bundleNode.Select("entry.resource.ofType(Patient).name.given", _evaluationContext);
        }

        [Benchmark]
        public object EvaluateWithPreCompiledExpression()
        {
            // Using pre-compiled expression which is how the SDK caches FHIRPath expressions
            var expression = _compiler.Compile("name.where(use = 'official').given");
            return expression.Invoke(_patient.ToPocoNode(), _evaluationContext);
        }

        [Benchmark]
        public object EvaluateWithMultiplePreCompiledExpressions()
        {
            // Multiple pre-compiled expressions to test caching behavior in v6
            var expr1 = _compiler.Compile("name.where(use = 'official').given");
            var expr2 = _compiler.Compile("telecom.where(system = 'phone')");
            var expr3 = _compiler.Compile("address.city");

            var patientNode = _patient.ToPocoNode();

            // Execute all expressions
            expr1.Invoke(patientNode, _evaluationContext);
            expr2.Invoke(patientNode, _evaluationContext);
            return expr3.Invoke(patientNode, _evaluationContext);
        }
    }
}
