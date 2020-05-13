using ho1a.applicationCore.Data.Enum;

namespace ho1a.applicationCore.Data.Interfaces
{
    public interface IObjectWithState
    {
        EObjectState ObjectState { get; set; }
    }
}
