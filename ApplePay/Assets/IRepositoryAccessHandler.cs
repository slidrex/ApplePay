public interface IRepositoryUpdateHandler
{
    public void OnRepositoryUpdated(Item item, byte index, RepositoryChangeFeedback feedback);
}
public enum RepositoryChangeFeedback
{
    Removed,
    Added,
    Replaced
}