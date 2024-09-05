﻿using domition_api.Infrastructure.Constants;

namespace dominitian.Infrastructure.Configuration
{
    public class ApiUrlOptions
    {
        public const string SectionName = "ApiUrls";

        public string? ApiUrlBase { get; set; }

        #region Register controller

        public string RegisterConfirmEmail(string? userId, string? code)
            => $"{ApiUrlBase}/api/{ControllerEndpPoints.RegisterController}/{ControllerEndpPoints.ConfirmEmail}?userId={userId}&code={code}";

        #endregion
    }
}
