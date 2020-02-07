using System;
using System.Collections.Generic;
using System.Net;

namespace WebApi.Owin
{
    public sealed class ExceptionHandlingOptions
    {
        public ExceptionHandlingOptions()
        {
            StatusCodes = new Dictionary<Type, HttpStatusCode>
            {
                {typeof(KeyNotFoundException), HttpStatusCode.NotFound},
                {typeof(ArgumentException), HttpStatusCode.BadRequest },
            };
        }

        public IDictionary<Type, HttpStatusCode> StatusCodes { get; set; }
    }
}