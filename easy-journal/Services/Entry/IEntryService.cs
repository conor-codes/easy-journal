namespace easy_journal.Services.Entry
{
    public interface IEntryService
    {
        Task SaveEntryAsync(Models.Entry entry);
        Task<Models.Entry> GetEntryAsync();
        Task DeleteEntryAsync(Models.Entry entry);
    }
}
