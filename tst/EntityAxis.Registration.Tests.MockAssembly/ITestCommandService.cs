using EntityAxis.Abstractions;

namespace EntityAxis.Registration.Tests.MockAssembly;

public interface ITestCommandService : ICommandService<TestEntity, int>;
