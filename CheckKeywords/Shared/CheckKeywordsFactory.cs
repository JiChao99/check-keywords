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
            EnumCheckType.Word => new WordsHandler(),
            EnumCheckType.Json => new JsonHandler(),
            EnumCheckType.SwaggerUrl => new SwaggerHandler(),
            _ => throw new NotImplementedException()
        };
    }
}
