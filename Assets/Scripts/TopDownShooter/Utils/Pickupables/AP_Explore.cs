using DG.Tweening;
using System.Collections.Generic;
using TDS;
using UnityEngine;

namespace TDS
{
    public class AP_Explore : Collectable
    {
        [Header("Camera Transition")]
        public Vector3 Offset;
        public float Duration;

        [Header("Auto Handling")]
        public bool AutoTimer = true;
        public List<Transform> NewSpawnPoints = new();

        [Header("Next Explore Area")]
        public AP_Explore nextExploreArea;

        private void Awake()
        {
            SpawnManager.Instance.AddExploreItem(this);
        }

        private void Start()
        {
            //SpawnManager.Instance.AddExploreItem(this);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            //transform.DOMove(new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), 1f).SetEase(Ease.InOutBack);
        }

        public override void PickUp(GameObject obj, Item item)
        {
            base.PickUp(obj, item);

            Camera camera = Camera.main;
            camera.transform.DOMove(new Vector3(camera.transform.position.x + Offset.x, camera.transform.position.y + Offset.y, camera.transform.position.z + Offset.z), Duration);

            if (NewSpawnPoints.Count > 0)
            {
                SpawnManager.Instance.SetNewSpawnPoints(NewSpawnPoints);
            }

            //CanvasManager.Instance.ResetTimer();
            //SpawnManager.Instance.AddExploreItem(this);
            //CanvasManager.Instance.AddExploreArea(nextExploreArea);

            if (AutoTimer)
            {
                TDSCanvasManager.Instance.ResetTimer();
                SpawnManager.Instance.StartGame();
            }

            SpawnManager.Instance.DisableAllExploreAreas();

        }
    }

}
