public interface IObjectPoolable<T>
{
    public T GetObject();
    public void ReturnToPool(T obj);
}
