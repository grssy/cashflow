using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using Shouldly;

namespace UseCases.Test.Users.Register;
public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Count().ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Count().ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public async Task Error_Email_Already_Exists()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrors().Count().ShouldBe(1);
        result.GetErrors().ShouldContain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var passwordEncripter = new PasswordEncripterBuilder();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (string.IsNullOrWhiteSpace(email) == false)
        {
            userReadOnlyRepository.ExisActiveUserWithEmail(email);
        }

        return new RegisterUserUseCase(mapper, passwordEncripter.Build(), userReadOnlyRepository.Build(), userWriteOnlyRepository, unitOfWork, accessTokenGenerator);
    }
}
