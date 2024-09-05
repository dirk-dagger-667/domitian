﻿using dominitian_ui.Models.Responses.Login;
using dominitian_ui.Models.Results;
using Microsoft.AspNetCore.Identity;

namespace dominitian.Tests.Infrastructure.DataSources.UserAdmin.Controllers
{
    public class LoginControllerTestData
    {
        public static IEnumerable<object[]> FailingSignInData
        {
            get
            {
                yield return new object[] { SignInResult.Failed };
                yield return new object[] { SignInResult.NotAllowed };
            }
        }

        public static IEnumerable<object[]> FailingResultsBadRequest
        {
            get
            {
                yield return new object[] { Result<LoginResponse>.Failure(ResultTypes.BadRequest, LoginErrors.WrongPassword) };
                yield return new object[] { Result<LoginResponse>.Failure(ResultTypes.BadRequest, LoginErrors.LoginLockedOut) };
                yield return new object[] { Result<LoginResponse>.Failure(ResultTypes.BadRequest, LoginErrors.FailedAttempt) };
                yield return new object[] { Result<LoginResponse>.Failure(new Exception()) };
            }
        }
    }
}

