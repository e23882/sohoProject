using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static int samplesPlayed = 0;
        static Stopwatch stRecord = new Stopwatch();
        static Stopwatch stPlay = new Stopwatch();
        static void Main(string[] args)
        {
            // 建立一個 WaveInEvent 物件，用於設定麥克風的相關屬性
            WaveInEvent waveIn = new WaveInEvent();

            // 設定音訊的格式，這裡使用預設的格式
            waveIn.WaveFormat = new WaveFormat();

            // 建立一個 WaveOutEvent 物件，用於撥放聲音
            WaveOutEvent waveOut = new WaveOutEvent();

            // 建立一個 BufferedWaveProvider 物件，用於將錄音資料緩衝
            bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);

            // 計數器變數

            // 設定錄音的事件處理函式
            waveIn.DataAvailable += (sender, e) =>
            {
                // 將錄音資料添加到 BufferedWaveProvider 中
                bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);

                // 更新計數器
                samplesPlayed += e.BytesRecorded / 2; // 一個樣本佔 2 個位元組
            };
            stRecord.Start();
            // 開始錄音
            waveIn.StartRecording();
            
            Console.WriteLine("正在錄音，按下任意鍵停止...");
            Console.ReadKey();
            stRecord.Stop();

            // 停止錄音
            waveIn.StopRecording();

            // 將 BufferedWaveProvider 設定給 WaveOutEvent 物件，開始播放聲音
            waveOut.Init(bufferedWaveProvider);
            stPlay.Start();
            waveOut.Play();
            while(stPlay.ElapsedMilliseconds< stRecord.ElapsedMilliseconds) 
            {
                Thread.Sleep(1000);
            }

          
            Console.WriteLine("正在播放錄音，按下任意鍵停止...");
            Console.ReadKey();

            //// 停止播放聲音
            //waveOut.Stop();
            //waveOut.Dispose();

           

            //Console.WriteLine("播放結束");
        }

        private static void WaveOut_PlaybackStopped1(object sender, StoppedEventArgs e)
        {
            throw new NotImplementedException();
        }

        static BufferedWaveProvider bufferedWaveProvider;

        // 錄音資料可用時的事件處理函式
        static void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            // 將錄音資料添加到 BufferedWaveProvider 中
            bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            samplesPlayed += e.BytesRecorded / 2; // 一個樣本佔 2 個位元組
        }

        // 播放停止時的事件處理函式
        static void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            Console.WriteLine("播放結束");
        }
    }
}
