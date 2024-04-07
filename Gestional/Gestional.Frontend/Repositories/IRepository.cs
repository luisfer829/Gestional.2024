namespace Gestional.Frontend.Repositories
{
    public interface IRepository
    {
        Task<httResponseWrapper<T>> GetAsync<T>(string url);
        Task<httResponseWrapper<object>> PostAsync<T>(string url, T model);
        Task<httResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model);
    }
}
