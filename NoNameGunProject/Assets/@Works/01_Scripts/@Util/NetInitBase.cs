using Unity.Netcode;

public class NetInitBase : NetworkBehaviour
{
    protected bool _init = false;

    public virtual bool Init()
    {
        if (_init)
        {
            return false;
        }

        _init = true;
        return true;
    }

    private void Awake()
    {
        Init();
    }
}
