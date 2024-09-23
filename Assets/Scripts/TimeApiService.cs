using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Clock
{
    public class TimeApiService : MonoBehaviour
    {
        private readonly string[] _timeApis = {
            "https://worldtimeapi.org/api/timezone/Europe/Moscow",
            "https://worldtimeapi.org/api/timezone/Europe/London"
        };

        private const float RequestTimeout = 5f;

        public IEnumerator GetTimeFromApi(Action<DateTime> onSuccess, Action onFailure)
        {
            foreach (var apiUrl in _timeApis)
            {
                // Debug.Log($"Отправка запроса на {apiUrl}");

                UnityWebRequest request = UnityWebRequest.Get(apiUrl);
                request.timeout = (int)RequestTimeout;
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    TimeApiResponse timeResponse = JsonUtility.FromJson<TimeApiResponse>(request.downloadHandler.text);
                    onSuccess(DateTime.Parse(timeResponse.datetime));
                    yield break;
                }
                else
                {
                    // Debug.LogError($"Ошибка запроса: {request.error}");
                    onFailure?.Invoke();
                    yield break;
                }
            }

            onFailure?.Invoke();
        }

        [Serializable]
        private class TimeApiResponse
        {
            public string datetime;
        }
    }
}