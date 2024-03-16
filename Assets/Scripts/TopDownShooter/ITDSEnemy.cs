using TDS.Variables;

namespace TDS.Elements
{
    public interface ITDSEnemy
    {
        TDSStats GetStats();
        void Initialize(TDSStats stats);
    }
}