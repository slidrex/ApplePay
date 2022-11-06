///<summary> Needs to be added to inventory system repository handler list to be activated </summary>
public interface IRepositoryHandler {}
///<summary> Needs to be added to inventory system repository handler list to be activated </summary>
public interface IRepositoryCallbackHandler : IRepositoryHandler
{
    public void OnRepositoryUpdated(Item item, byte index, RepositoryChangeFeedback feedback);
}
///<summary> Needs to be added to inventory system repository handler list to be activated </summary>
public interface IRepositoryPreUpdateHandler : IRepositoryHandler
{
    public void OnBeforeRepositoryUpdate();
}
public enum RepositoryChangeFeedback
{
    Added,
    Removed,
    Replaced
}