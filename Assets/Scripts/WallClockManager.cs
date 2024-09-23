using System;
using UnityEngine;
using DG.Tweening;

namespace Clock
{
    public class WallClockManager : MonoBehaviour
    {
        [SerializeField] private Transform _hourHand;
        [SerializeField] private Transform _minuteHand;
        [SerializeField] private Transform _secondHand;
        [SerializeField] private float animationDuration = 0.5f;

        public void UpdateWallClock(DateTime time)
        {
            float hourDegrees = (time.Hour % 12) * 30 + (time.Minute / 2f);
            float minuteDegrees = time.Minute * 6 + (time.Second / 10f);
            float secondDegrees = time.Second * 6 + (time.Millisecond / 1000f) * 6;
            
            _hourHand.DORotate(new Vector3(0, 0, -hourDegrees + 90), animationDuration, RotateMode.FastBeyond360);
            _minuteHand.DORotate(new Vector3(0, 0, -minuteDegrees + 90), animationDuration, RotateMode.FastBeyond360);
            _secondHand.DORotate(new Vector3(0, 0, -secondDegrees + 90), animationDuration, RotateMode.FastBeyond360);
        }
    }
}