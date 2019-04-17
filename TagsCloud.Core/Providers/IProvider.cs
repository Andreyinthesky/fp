using TagsCloud.ErrorHandling;

namespace TagsCloud.Core.Providers
{
    public interface IProvider<T>
    {
        Result<T> Get();
    }
}