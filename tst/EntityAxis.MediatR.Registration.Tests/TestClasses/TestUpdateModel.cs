using EntityAxis.MediatR.Commands;

namespace EntityAxis.MediatR.Registration.Tests.TestClasses;

public class TestUpdateModel : IUpdateCommandModel<TestEntity, int>
{
    public int Id { get; set; }
}