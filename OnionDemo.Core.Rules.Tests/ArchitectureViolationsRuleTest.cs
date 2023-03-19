using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;


using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.MSTestV2;
using Autofac;
using OnionDemo.Persistance;
using OnionDemo.Services;

namespace OnionDemo.Core.Rules.Tests
{
    public class ArchitectureViolationsRuleTest
    {
        private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(
               System.Reflection.Assembly.Load("OnionDemo.Domain"),
               System.Reflection.Assembly.Load("OnionDemo.Services"),
               System.Reflection.Assembly.Load("OnionDemo.Persistance"),
               System.Reflection.Assembly.Load("OnionDemo.Presentation")).Build();

        private readonly IObjectProvider<IType> DomainModelLayer = Types().That().ResideInAssembly("OnionDemo.Domain");

        private readonly IObjectProvider<IType> DomainServiceLayer = Types().That().ResideInNamespace("OnionDemo.Services");

        private readonly IObjectProvider<IType> PersistanceLayer = Types().That().ResideInNamespace("OnionDemo.Persistance");

        private readonly IObjectProvider<IType> PresentationLayer = Types().That().ResideInNamespace("OnionDemo.Presentation");

        [Fact]
        public void TestLayerShouldContainsPrerequisiteAbstractions()
        {
            var domainModelInterfaces = Architecture.Interfaces.Where(x =>
                            x.NameEndsWith("Repository") ||
                            x.NameEndsWith("Manager"));

            Assert.True(domainModelInterfaces.Any());
        }

        [Fact]
        public void TestLayerForViolations()
        {
            // Create a new container builder
            var builder = new ContainerBuilder();

            // Register the module to be tested
            builder.RegisterModule(new ServiceAutofacModule());
            builder.RegisterModule(new PersistanceAutofacModule());
            builder.Build();

            //The domain layer should not have reference to any other layer within the application core.
            IArchRule layerdRule = Types().That().Are(DomainModelLayer)
                .Should().NotDependOnAny(DomainServiceLayer)
                .Because("The domain model layer should not have reference to any other layer within the application core.");
            layerdRule.Check(Architecture);

            //Domain layer should not have references to the repositories layer.
            layerdRule = Types().That().Are(DomainModelLayer)
               .Should().NotDependOnAny(PersistanceLayer)
               .Because("The Domain layer should not have references to the repositories layer.");
            layerdRule.Check(Architecture);

            //Domain layer should not have references to the Presentation layer.
            layerdRule = Types().That().Are(DomainModelLayer)
               .Should().NotDependOnAny(PresentationLayer)
               .Because("The Domain layer should not have references to the Presentation layer.");
            layerdRule.Check(Architecture);

            //Domain layer should not have references to the Persistance layer.
            layerdRule = Types().That().Are(DomainServiceLayer)
                .Should().NotDependOnAny(PersistanceLayer)
                .Because("Domain layer should not have references to the Persistance layer.");

            layerdRule.Check(Architecture);


            //A domain Service layer reference must have depedent from domain layer
            layerdRule = Types().That().Are(PresentationLayer)
                .Should().DependOnAny(DomainModelLayer);
            
            layerdRule.Check(Architecture);
        }
    }
}