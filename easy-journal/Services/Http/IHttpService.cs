namespace easy_journal.Servicess.Http
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string url, Dictionary<string, string> headers = null);
        Task<T> PostAsync<T>(string url, object body, Dictionary<string, string> headers = null);
        Task<string> GetStringAsync(string url, Dictionary<string, string> headers = null);
    }
}
