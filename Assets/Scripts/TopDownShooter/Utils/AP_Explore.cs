using DG.Tweening;
using System.Collections.Generic;
using TDS;
using UnityEngine;

namespace TDS
{
    public class AP_Explore : AbstractPickup
    {
        [Header("Camera Transition")]
        public Vector3 Offset;
        public float Duration;

        [Header("Auto Handling")]
        public bool AutoNext = false;
        public bool AutoTimer = true;
        public List<Transform> NewSpawnPoints = new();


        private void OnEnable()
        {
            Debug.Log("Explore area activated");
            transform.DOMove(new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), 1f).SetEase(Ease.InOutBack);
        }

        public override void PickUp(GameObject obj, Item item)
        {
            base.PickUp(obj, item);

            Camera camera = Camera.main;
            camera.transform.DOMove(new Vector3(camera.transform.position.x + Offset.x, camera.transform.position.y + Offset.y, camera.transform.position.z + Offset.z), Duration);

            CanvasManager.Instance.RemoveExploreArea(this);

            if (AutoTimer)
            {
                CanvasManager.Instance.ResetTimer();
            }

            if (NewSpawnPoints.Count > 0)
            {
                SpawnManager.Instance.SetNewSpawnPoints(NewSpawnPoints);
                SpawnManager.Instance.StartGame();
            }
        }
    }

}
