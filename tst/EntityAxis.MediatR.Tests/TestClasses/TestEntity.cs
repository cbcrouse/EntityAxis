using EntityAxis.Abstractions;

namespace EntityAxis.MediatR.Tests.TestClasses;

public class TestEntity : IEntityId<int>
{
    public int Id { get; set; }
}