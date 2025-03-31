using EntityAxis.MediatR.Commands;

namespace EntityAxis.MediatR.Tests.TestClasses;

public class TestUpdateModel : IUpdateCommandModel<TestEntity, int>
{
    public int Id { get; set; }
}