using EntityAxis.Abstractions;

namespace EntityAxis.Registration.Tests.TestClasses;

public class TestEntity : IEntityId<int>
{
    public int Id { get; set; }
}