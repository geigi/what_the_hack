/// <summary>
/// This Interface is used by classes that contain a serializable data object.
/// Every class that contains data that will be saved/loaded needs to implement this interface.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDataObjectUser<T>
{
    void SetData(T data);
    T GetData();
}