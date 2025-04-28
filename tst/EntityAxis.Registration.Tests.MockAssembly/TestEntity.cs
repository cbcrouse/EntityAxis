using EntityAxis.Abstractions;

namespace EntityAxis.Registration.Tests.MockAssembly;

public class TestEntity : IEntityId<int>
{
    public int Id { get; set; }
}