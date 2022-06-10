using FakeItEasy;
using LightBoard.DataAccess.Abstractions;
using LightBoard.DataAccess.Abstractions.Repositories;

namespace LightBoard.Tests.Common;

public class UnitOfWorkFakeWrapper
{
    public Fake<IUnitOfWork> Fake { get; }
    public IUnitOfWork FakedObject { get; }

    public Fake<IUsersRepository> Users { get; }

    public UnitOfWorkFakeWrapper()
    {
        Fake = new Fake<IUnitOfWork>();
        Users = new Fake<IUsersRepository>();

        Fake.CallsTo(unitOfWork => unitOfWork.Users)
            .Returns(Users.FakedObject);

        FakedObject = Fake.FakedObject;
    }
}