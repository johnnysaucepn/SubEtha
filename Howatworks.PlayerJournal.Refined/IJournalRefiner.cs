namespace Howatworks.PlayerJournal.Refined
{
    public interface IJournalRefiner<in T, out T1>
    {
        T1 Refine(T raw);
    }
}
