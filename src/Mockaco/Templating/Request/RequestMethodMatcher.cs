﻿using Microsoft.AspNetCore.Http;
using System;

namespace Mockaco
{
    public class RequestMethodMatcher : IRequestMatcher
    {
        public bool IsMatch(HttpRequest httpRequest, Mock mock)
        {
            if (string.IsNullOrWhiteSpace(mock?.Method))
            {
                return false;
            }

            var isMatch = httpRequest.Method.Equals(mock.Method, StringComparison.InvariantCultureIgnoreCase);

            return isMatch;
        }
    }
}
