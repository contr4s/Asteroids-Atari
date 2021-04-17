public interface IDestroyable
{
    void DestroyMe(eDestroyedBy destroyedBy = eDestroyedBy.none);

    bool CreatedByPlayer { get; }
}
