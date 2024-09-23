using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace Clock
{
    public class NtpService : MonoBehaviour
    {
        private const string NtpServer = "pool.ntp.org";
        private const int NtpPort = 123;

        public async Task<DateTime> GetNetworkTimeAsync()
        {
            try
            {
                // Debug.Log("Запрос к NTP-серверу начат...");

                using (var udpClient = new UdpClient())
                {
                    udpClient.Connect(NtpServer, NtpPort);

                    var ntpData = new byte[48];
                    ntpData[0] = 0x1B;

                    // Debug.Log("Отправка запроса...");
                    await udpClient.SendAsync(ntpData, ntpData.Length);

                    // Debug.Log("Ожидание ответа...");

                    Task<UdpReceiveResult> receiveTask = udpClient.ReceiveAsync();

                    Task timeoutTask = Task.Delay(2000);

                    var completedTask = await Task.WhenAny(receiveTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        // Debug.LogWarning("Тайм-аут при ожидании ответа от NTP-сервера.");
                        return DateTime.MinValue;
                    }

                    var response = await receiveTask;
                    // Debug.Log("Ответ получен");

                    byte[] ntpResponseData = response.Buffer;

                    if (ntpResponseData.Length < 48)
                    {
                        // Debug.LogError("Некорректный ответ от NTP-сервера");
                        return DateTime.MinValue;
                    }

                    ulong intPart = BitConverter.ToUInt32(ntpResponseData, 40);
                    ulong fractPart = BitConverter.ToUInt32(ntpResponseData, 44);

                    intPart = SwapEndianness(intPart);
                    fractPart = SwapEndianness(fractPart);

                    var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                    var networkDateTime = new DateTime(1900, 1, 1).AddMilliseconds((long)milliseconds);

                    // Debug.Log($"Полученное время: {networkDateTime.ToLocalTime()}");
                    return networkDateTime.ToLocalTime();
                }
            }
            catch (Exception ex)
            {
                // Debug.LogError($"Ошибка получения времени через NTP: {ex.Message}");
                return DateTime.MinValue;
            }
        }

        private static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) + ((x & 0x0000ff00) << 8) +
                          ((x & 0x00ff0000) >> 8) + ((x & 0xff000000) >> 24));
        }
    }
}