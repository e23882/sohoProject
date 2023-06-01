using NAudio.Wave;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // 建立一個 WaveInEvent 物件，用於設定麥克風的相關屬性
            NAudio.Wave.WaveInEvent waveIn = new NAudio.Wave.WaveInEvent();

            // 設定音訊的格式，這裡使用預設的格式
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat();

            // 設定錄音的事件處理函式
            waveIn.DataAvailable += WaveIn_DataAvailable;

            // 建立一個 WaveOut 物件，用於撥放聲音
            WaveOut waveOut = new WaveOut();

            // 開始錄音
            waveIn.StartRecording();

            Console.WriteLine("正在錄音，按下任意鍵停止...");
            Console.ReadKey();

            // 停止錄音
            waveIn.StopRecording();

            // 建立一個 BufferedWaveProvider 物件，用於將錄音資料緩衝
            BufferedWaveProvider bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);
            bufferedWaveProvider.AddSamples(buffer, 0, buffer.Length);

            // 將 BufferedWaveProvider 設定給 WaveOut 物件，開始播放聲音
            waveOut.Init(bufferedWaveProvider);
            waveOut.Play();

            Console.WriteLine("正在播放錄音，按下任意鍵停止...");
            Console.ReadKey();

            // 停止播放聲音
            waveOut.Stop();
            waveOut.Dispose();
        }
        static byte[] buffer;

        // 錄音資料可用時的事件處理函式
        static void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // 記錄錄音資料到 buffer
            buffer = new byte[e.BytesRecorded];
            Array.Copy(e.Buffer, buffer, e.BytesRecorded);

            // 輸出錄音資料
            Console.WriteLine("錄音中...");
            for (int i = 0; i < e.BytesRecorded; i += 2)
            {
                short sample = (short)((e.Buffer[i + 1] << 8) | e.Buffer[i]);
                float sample32 = sample / 32768f;

                // 在這裡你可以對聲音資料進行處理或存儲
                // 這裡只是示範將聲音值輸出到控制台
                Console.WriteLine(sample32);
            }
        }
    }
}
