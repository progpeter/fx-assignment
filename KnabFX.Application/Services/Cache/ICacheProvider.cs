namespace KnabFX.Application.Services.Cache
{
    public interface ICacheProvider<T>
    {
        public T Get(string key);

        public bool Set(string key, T value, int ttl = 0);

        public bool Delete(string key);
    }
}
