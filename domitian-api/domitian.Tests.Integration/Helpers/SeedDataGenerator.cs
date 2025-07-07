using Bogus;
using domitian.Infrastructure.Validators;
using domitian.Models.Requests.Login;
using domitian.Models.Requests.Registration;
using domitian_api.Data.Identity;
using System.Text;
using System.Text.RegularExpressions;

namespace domitian.Tests.Integration.Helpers
{
  public class SeedDataGenerator
  {
    //public static IEnumerable<DomitianIDUser> GetUsers(int count, bool useNewSeed = false)
    //  => GetUsersFaker(useNewSeed).Generate(count);

    //public static DomitianIDUser GetUser(bool useNewSeed = false)
    //  => GetUsers(1, useNewSeed).First();

    public static IEnumerable<RegisterRequest> GetRegisterRequests(int count, bool useNewSeed = false)
      => GetRegisterRequestsFaker(useNewSeed).Generate(count);

    public static RegisterRequest GetRegisterRequest(bool useNewSeed = false)
      => GetRegisterRequests(1, useNewSeed).First();

    public static IEnumerable<LoginRequest> GetLoginRequests(int count, bool useNewSeed = false)
      => GetLoginRequestsFaker(useNewSeed).Generate(count);

    //private static Faker<DomitianIDUser> GetUsersFaker(bool useNewSeed = false)
    //=> new Faker<DomitianIDUser>()
    //    .RuleFor(user => user.Email, (faker, user) => faker.Internet.ExampleEmail(faker.Name.FirstName(), faker.Name.LastName()))
    //    .RuleFor(user => user.UserName, (faker, user) => user.Email)
    //    .RuleFor(user => user.RefreshToken, faker => "dummy AccesToken")
    //    .RuleFor(user => user.RefreshTokenExpiry, (faker, user) => DateTime.UtcNow.AddMinutes(20))
    //    .RuleFor(user => user.EmailConfirmed, faker => true)
    //    .RuleFor(user => user.PasswordHash, (faker, user) => faker.Random.String())
    //    .UseSeed(useNewSeed ? Random.Shared.Next(10, int.MaxValue) : 0);

    private static Faker<RegisterRequest> GetRegisterRequestsFaker(bool useNewSeed = false)
      => new Faker<RegisterRequest>()
        .RuleFor(regRequest => regRequest.Email, faker => faker.Internet.ExampleEmail(faker.Name.FirstName(), faker.Name.LastName()))
        .RuleFor(regRequest => regRequest.Password, faker => GenerateRandomValidPassword(faker, RegisterRequestValidator.PasswordRegextExpression, 8, 24))
        .RuleFor(regRequest => regRequest.ConfirmPassword, (faker, regRequest) => regRequest.Password)
        .UseSeed(useNewSeed ? Random.Shared.Next(10, int.MaxValue) : 0);

    private static Faker<LoginRequest> GetLoginRequestsFaker(bool useNewSeed = false)
      => new Faker<LoginRequest>()
        .RuleFor(loginRequest => loginRequest.Email, faker => faker.Internet.ExampleEmail(faker.Name.FirstName(), faker.Name.LastName()))
        .RuleFor(loginRequest => loginRequest.Password, faker => GenerateRandomValidPassword(faker, RegisterRequestValidator.PasswordRegextExpression, 8, 24))
        .UseSeed(useNewSeed ? Random.Shared.Next(10, int.MaxValue) : 0);



    private static string GenerateRandomValidPassword(Faker faker,
      string regexExpression,
      int minLength,
      int maxLength)
    {
      var randomizer = new Randomizer();
      var lowerCase = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
      var upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
      var digits = "0123456789".ToCharArray();
      var special = "!@#$%^&*".ToCharArray();
      var allChars = lowerCase.Concat(upperCase).Concat(digits).Concat(special).ToArray();

      var length = randomizer.Number(minLength, maxLength);
      var password = new StringBuilder();

      // Ensure at least one character from each set
      password.Append(randomizer.ArrayElement(lowerCase));
      password.Append(randomizer.ArrayElement(upperCase));
      password.Append(randomizer.ArrayElement(digits));
      password.Append(randomizer.ArrayElement(special));

      // Fill the rest with random characters
      for (int i = 4; i < length; i++)
      {
        password.Append(randomizer.ArrayElement(allChars));
      }

      // Shuffle the password
      var passwordArray = password.ToString().ToCharArray();
      randomizer.Shuffle(passwordArray);
      var finalPassword = new string(passwordArray);

      // Validate against regex
      if (!Regex.IsMatch(finalPassword, regexExpression))
      {
        // Recurse if validation fails (rare due to enforced characters)
        return GenerateRandomValidPassword(faker, regexExpression, minLength, maxLength);
      }

      return finalPassword;
    }
  }
}
