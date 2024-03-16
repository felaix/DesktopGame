using UnityEngine;

namespace TDS
{
    using Elements;
    using TDS.Variables;

    public class TDSController : MonoBehaviour
    {
        public static TDSController instance;

        TDSElements elements;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            Initialize();
            
            elements.Initialize();

        }
        private void Initialize()
        {
            CreateElements();
            //CreateEnemy(elements.defaultStats);
        }

        //private TDSEnemy CreateEnemy(TDSStats stats)
        //{
        //    TDSEnemy newEnemy = new TDSEnemy();
        //    newEnemy.Initialize(stats);
        //    return newEnemy;
        //}

        private void CreateElements() => elements = new TDSElements();

    }

}
