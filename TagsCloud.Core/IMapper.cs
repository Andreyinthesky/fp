namespace TagsCloud.Core
{
    public interface IMapper<in T, out TOut>
    {
        TOut Map(T item);
    }
}