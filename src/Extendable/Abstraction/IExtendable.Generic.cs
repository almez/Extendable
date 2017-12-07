namespace Extendable.Abstraction
{
    public interface IExtendable<T> : IExtendable
    {
        new T Id { set; get; }
    }
}