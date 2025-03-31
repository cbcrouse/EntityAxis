using EntityAxis.Abstractions;

namespace EntityAxis.MediatR.Tests.TestClasses;

public class TestCreateModel : IEntityId<int>
{
    public int Id { get; set; }
}