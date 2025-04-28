using EntityAxis.Abstractions;

namespace EntityAxis.Registration.Tests.MockAssembly;

public interface ITestQueryService : IQueryService<TestEntity, int>;
