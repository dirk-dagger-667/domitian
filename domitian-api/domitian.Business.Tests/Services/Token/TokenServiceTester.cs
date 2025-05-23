﻿using domitian.Business.Contracts;
using domitian.Tests.Infrastructure.DataSources.UserAdmin.Services;
using domitian_api.Data.Identity;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace domitian.Business.Tests.Services.Token
{
    [Collection("DIServiceProviderCollection")]
    public class TokenServiceTester
    {
        private readonly ITokenService _tokenService;

        public TokenServiceTester(ServiceProviderFixture _serviceProvider)
        {
            _tokenService = _serviceProvider.ServiceProvider.GetService<ITokenService>()!;
        }

        [Theory]
        [MemberData(memberName: nameof(TokenServiceTestData.GenerateTokenTestDataNonNull), MemberType = typeof(TokenServiceTestData))]
        public void GenerateToken_Should_Return_Token(DomitianIDUser user)
        {
            var token = _tokenService.GenerateJwt(user);
            var claimsPrincipal = _tokenService.GetPrincipalFromExpiredToken(token);

            var name = claimsPrincipal?.FindFirst(c => c.Type == "name")?.Value;
            var mail = claimsPrincipal?.FindFirst(c => c.Type.Contains("email"))?.Value;

            name.Should().NotBeNull().And.Be(user.UserName);
            mail.Should().NotBeNull().And.Be(user.Email);
        }

        [Theory]
        [MemberData(memberName: nameof(TokenServiceTestData.GenerateTokenTestDataNull), MemberType = typeof(TokenServiceTestData))]
        public void GenerateToken_Should_Throw_ArgumentNullReference(DomitianIDUser user, Type expectedEx)
            => FluentActions.Invoking(() => _tokenService.GenerateJwt(user))
                .Should().Throw<Exception>()
                .Where(e => e.GetType() == expectedEx);
    }
}

