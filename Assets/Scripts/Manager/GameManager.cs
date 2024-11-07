public class GameManager : Singleton<GameManager>
{
    private ObjectPool _objectPool;
    public ObjectPool objectPool
    {
        get { return _objectPool; }
        set { _objectPool = value; }
    }
}