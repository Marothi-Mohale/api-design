using TutoringPlatform.Infrastructure.Auth;

namespace TutoringPlatform.Tests;

public class Pbkdf2PasswordHasherTests
{
    private readonly Pbkdf2PasswordHasher _passwordHasher = new();

    [Fact]
    public void Hash_ShouldCreateVerifiableHash()
    {
        const string password = "StrongerPassword123!";

        var hash = _passwordHasher.Hash(password);

        Assert.NotNull(hash);
        Assert.NotEqual(password, hash);
        Assert.True(_passwordHasher.Verify(password, hash));
    }

    [Fact]
    public void Verify_ShouldReturnFalse_ForIncorrectPassword()
    {
        var hash = _passwordHasher.Hash("CorrectPassword123!");

        var result = _passwordHasher.Verify("WrongPassword456!", hash);

        Assert.False(result);
    }
}
