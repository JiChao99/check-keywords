using Shared.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class CheckKeywordsFactory
    {
        public static CheckKeywordsHandler CreateHandler(EnumCheckType type) => type switch
        {
            EnumCheckType.Words => new WordsHandler(),
            EnumCheckType.Json => new JsonHandler(),
            EnumCheckType.Swagger => new SwaggerHandler(),
            _ => throw new NotImplementedException()
        };
    }
}
