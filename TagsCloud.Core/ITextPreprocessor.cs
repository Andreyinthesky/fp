using System.Collections.Generic;
using TagsCloud.ErrorHandling;

namespace TagsCloud.Core
{
    public interface ITextPreprocessor
    {
        Result<IEnumerable<string>> Process(string text);
    }
}