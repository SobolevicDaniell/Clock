using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace Clock
{
    public class GetTimeService : MonoBehaviour
    {
        private const string url = "https://yandex.com/time/sync.json";

        public IEnumerator FetchTime(Action<long> onSuccess, Action onError)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                        TimeResponse response = JsonUtility.FromJson<TimeResponse>(request.downloadHandler.text);
                        onSuccess?.Invoke(response.time);
                }
                else
                {
                    onError?.Invoke();
                }
            }
        }

        [Serializable]
        public class TimeResponse
        {
            public long time;
            // public Clocks clocks;
            //
            // [Serializable]
            // public class Clocks
            // {
            // }
        }
    }
}