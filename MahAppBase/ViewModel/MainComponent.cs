using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using MahAppBase.Command;
using NAudio.Wave;
using RestSharp;

namespace MahAppBase.ViewModel
{
    public class MainComponent : ViewModelBase
    {
        #region Declarations
        /// <summary>
        /// WebAPI URL
        /// </summary>
        public const string BaseURL = "https://0883-114-35-31-137.ngrok-free.app";

        /// <summary>
        /// 錄音、撥放相關物件實例變數
        /// </summary>
        private WaveInEvent waveIn = new WaveInEvent();
        private WaveOut _PlayRecordingInstance;
        private WaveFileWriter waveWriter;

        private string _MainReadingContent;
        private string _RecordButtonContent = "開始錄音";
        private string _PlayRecordContent = "播放錄音";
        private string _AccurateResult = "文章會在這　按下開始診斷以繼續治療 ";
        private string _KnowledgeResult = "辨識的結果會在這 請按下開始診斷以繼續治療";
        private string _StartDiagnosisContent = "開始診斷";
        
        private bool _RecordIsNotPlaying = false;
        private bool _Step1Selected = true;
        private bool _Step2Selected = false;
        private bool _ButtonEnable = true;
        private bool _DiagnosisRecordingIsStart = false;

        private int _CurrentStep = 1;
        private int _TotalStep = -1;
        #endregion

        #region Property
        /// <summary>
        /// 開始錄音
        /// </summary>
        public NoParameterCommand StartRecordCommand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public NoParameterCommand StartDiagnosisRecordCommand { get; set; }

        /// <summary>
        /// 播放錄音
        /// </summary>
        public NoParameterCommand PlayRecordCommand { get; set; }
        
        /// <summary>
        /// 上傳錄音
        /// </summary>
        public NoParameterCommand SendRecordCommand { get; set; }

        /// <summary>
        /// 查看診斷報告
        /// </summary>
        public NoParameterCommand CheckReportCommand { get; set; }

        /// <summary>
        /// 前往治療階段
        /// </summary>
        public NoParameterCommand GoNextCommand { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public NoParameterCommand StartDiagnosisCommand { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public NoParameterCommand ThreeTimesCorrectCommand { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public NoParameterCommand ChangeTextCommand { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public WaveOut PlayRecordingInstance
        {
            get
            {
                return _PlayRecordingInstance;
            }
            set
            {
                _PlayRecordingInstance = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string RecordButtonContent
        {
            get
            {
                return _RecordButtonContent;
            }
            set
            {
                _RecordButtonContent = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string PlayRecordContent
        {
            get
            {
                return _PlayRecordContent;
            }
            set
            {
                _PlayRecordContent = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string MainReadingContent
        {
            get
            {
                return _MainReadingContent;
            }
            set
            {
                _MainReadingContent = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 專案需要用到的重要參數Book
        /// </summary>
        public string Book { get; set; }

        /// <summary>
        /// 專案需要用到的重要參數Lesson
        /// </summary>
        public string Lesson { get; set; }

        /// <summary>
        /// 專案需要用到的重要參數StudentID
        /// </summary>
        public string StudentID { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public bool ButtonEnable
        {
            get
            {
                return _ButtonEnable;
            }
            set
            {
                _ButtonEnable = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool RecordIsNotPlaying
        {
            get
            {
                return _RecordIsNotPlaying;
            }
            set
            {
                _RecordIsNotPlaying = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool Step2Selected
        {
            get
            {
                return _Step2Selected;
            }
            set
            {
                _Step2Selected = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 是否在開始診斷的狀態
        /// </summary>
        public bool IsDiagnosising { get; set; } = false;
        
        /// <summary>
        /// 
        /// </summary>
        public bool Step1Selected
        {
            get
            {
                return _Step1Selected;
            }
            set
            {
                _Step1Selected = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int CurrentStep
        {
            get
            {
                return _CurrentStep;
            }
            set
            {
                _CurrentStep = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int TotalStep
        {
            get
            {
                return _TotalStep;
            }
            set
            {
                _TotalStep = value;
                OnPropertyChanged();
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        public string AccurateResult
        {
            get
            {
                return _AccurateResult;
            }
            set
            {
                _AccurateResult = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string KnowledgeResult
        {
            get
            {
                return _KnowledgeResult;
            }
            set
            {
                _KnowledgeResult = value;
                OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public string StartDiagnosisContent
        {
            get
            {
                return _StartDiagnosisContent;
            }
            set
            {
                _StartDiagnosisContent = value;
                OnPropertyChanged();
            }
        }
        
        #endregion

        #region MemberFunction

        /// <summary>
        /// 取得目前文章有多少個段落
        /// </summary>
        public void GetBookParagraphCount()
        {
            for (int i = 0; i < 100; i++)
            {
                var client = new RestClient($"{BaseURL}/getPartOfLessonText?book={Book}&lesson={Lesson}&studentID={StudentID}&part={i}");
                var request = new RestRequest();
                var response = client.Post(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TotalStep = i + 1;
                }
                else
                {
                    break;
                }
            }
        }
        
        /// <summary>
        /// ViewModel建構子
        /// </summary>
        /// <param name="book"></param>
        /// <param name="lesson"></param>
        /// <param name="studentID"></param>
        public MainComponent(string book, string lesson, string studentID)
        {
            Book = book;
            Lesson = lesson;
            StudentID = studentID;
            StartRecordCommand = new NoParameterCommand(StartRecordCommandAction);
            PlayRecordCommand = new NoParameterCommand(PlayRecordCommandAction);
            SendRecordCommand = new NoParameterCommand(SendRecordCommandAction);
            CheckReportCommand = new NoParameterCommand(CheckReportCommandAction);
            GoNextCommand = new NoParameterCommand(GoNextCommandAction);
            StartDiagnosisCommand = new NoParameterCommand(StartDiagnosisCommandAction);
            ThreeTimesCorrectCommand = new NoParameterCommand(ThreeTimesCorrectCommandAction);
            ChangeTextCommand = new NoParameterCommand(ChangeTextCommandAction);
            StartDiagnosisRecordCommand = new NoParameterCommand(StartDiagnosisRecordCommandAction);
            Step1Selected = true;
            Step2Selected = false;
        }
        public bool DiagnosisRecordingIsStart
        {
            get
            {
                return _DiagnosisRecordingIsStart;
            }
            set
            {
                _DiagnosisRecordingIsStart = value;
                OnPropertyChanged();
            }
        }

        private void StartDiagnosisRecordCommandAction()
        {
            if (RecordButtonContent == "開始錄音")
            {
                Stop();
                DiagnosisRecordingIsStart = true;
                RecordIsNotPlaying = false;
                waveIn = new WaveInEvent();
                waveIn.DataAvailable += WaveIn_DataAvailable;
                waveIn.DeviceNumber = 0;
                waveIn.WaveFormat = new WaveFormat(44100, 2);

                waveWriter = new WaveFileWriter("current.wav", waveIn.WaveFormat);
                waveIn.StartRecording();
                RecordButtonContent = "停止錄音";

            }
            else
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
                waveWriter.Close();
                waveWriter = null;
                RecordButtonContent = "開始錄音";
                RecordIsNotPlaying = true;

            }
        }

        /// <summary>
        /// 取得目前段落文章
        /// </summary>
        private void ChangeTextCommandAction()
        {
           
            var client = new RestClient($"{BaseURL}/getPartOfLessonText?book={Book}&lesson={Lesson}&studentID={StudentID}&part={CurrentStep-1}");
            var request = new RestRequest();
            var response = client.Post(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                AccurateResult = response.Content;
            }
            else 
            {
                AccurateResult = "取得目前段落文章失敗。";
            }
        }

        /// <summary>
        /// 三次全對後跳過按鈕Action
        /// </summary>
        private void ThreeTimesCorrectCommandAction()
        {
            //每次按就要加一，但是Cuurent不能大於Total
            if (CurrentStep < TotalStep)
            {
                CurrentStep++;
            }
            else 
            {
                KnowledgeResult = "辛苦啦~治療結束，請通知助教";
                AccurateResult = "辛苦啦~治療結束，請通知助教";
            }
        }

        /// <summary>
        /// 開始診斷
        /// </summary>
        private void StartDiagnosisCommandAction()
        {
            //正在開始診斷中
            if (IsDiagnosising == true)
            {
                if (DiagnosisRecordingIsStart != true) 
                {
                    MessageBox.Show("尚未完成治療階段的錄音，請完成後再上傳");
                    
                }
                else
                {
                    StartDiagnosisContent = "開始診斷";
                    ButtonEnable = false;
                    RecordIsNotPlaying = false;
                    IsDiagnosising = false;
                    Thread th = new Thread(() =>
                    {
                        LockAllButton();
                        KnowledgeResult = "正在辨識你的語音，請稍後。。。。。";
                        var client = new RestClient($"{BaseURL}/uploadAudioToCloudAndSTTThenSaveResult");
                        var request = new RestRequest();
                        request.AddParameter("book", Book);
                        request.AddParameter("lesson", Lesson);
                        request.AddParameter("studentID", StudentID);
                        if (waveWriter != null)
                        {
                            waveWriter.Flush();
                            waveWriter.Close();
                            waveWriter = null;
                        }
                        request.AddFile("file", "current.wav");

                        var response = client.Post(request);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //取得目前段落辨識結果
                            GetCurrentParagraphResult();
                            UnlockAllButton();
                        }
                        else
                        {
                            AccurateResult = "雲端語音辨識發生錯誤。";
                            UnlockAllButton();
                        }
                        IsDiagnosising = false;
                    });
                    th.Start();
                }
                
            }
            //不是正在開始診斷中
            else
            {
                ButtonEnable = true;
                RecordIsNotPlaying = true;
                IsDiagnosising = true;
                StartDiagnosisContent = "錄音完成後請按我";
            }
        }

        private void GetCurrentParagraphResult()
        {
            var client = new RestClient($"{BaseURL}/getPartOfWERLessonText");
            var request = new RestRequest();
            request.AddParameter("book", Book);
            request.AddParameter("lesson", Lesson);
            request.AddParameter("studentID", StudentID);
            request.AddParameter("part", CurrentStep-1);
            var response = client.Post(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                KnowledgeResult = response.Content;
            }
            else
            {
                AccurateResult = "取得目前段落辨識結果發生錯誤。";
            }
        }

        /// <summary>
        /// 取得整篇文章
        /// </summary>
        public void GetBookText()
        {
            var client = new RestClient($"{BaseURL}/getFirstLessonText?book={Book}&lesson={Lesson}&studentID={StudentID}");
            var request = new RestRequest();
            var response = client.Post(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                MainReadingContent = response.Content;
            }
            else
            {
                MainReadingContent = "取得文章發生錯誤。";
            }
        }

        /// <summary>
        /// 前往治療階段
        /// </summary>
        private void GoNextCommandAction()
        {
            Step1Selected = false;
            Step2Selected = true;
        }

        /// <summary>
        /// 查看診斷報告
        /// </summary>
        private void CheckReportCommandAction()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = $"{BaseURL}/showDiagnosisResult?book={Book}&lesson={Lesson}&studentID={StudentID}",
                UseShellExecute = true
            });
        }

        /// <summary>
        /// Disable所有按鈕
        /// </summary>
        public void LockAllButton()
        {
            ButtonEnable = false;
            RecordIsNotPlaying = false;
        }

        /// <summary>
        /// Enable所有按鈕
        /// </summary>
        public void UnlockAllButton()
        {
            ButtonEnable = true;
            RecordIsNotPlaying = true;
        }

        /// <summary>
        /// 上傳錄音檔
        /// </summary>
        private void SendRecordCommandAction()
        {
            Thread th = new Thread(() =>
            {
                MessageBoxResult result = MessageBox.Show("確認視窗", "是否要上傳?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    LockAllButton();
                    MainReadingContent = "正在辨識你的語音，請稍後。。。。。";
                    var client = new RestClient($"{BaseURL}/uploadAudioToCloudAndSTTThenSaveResult");
                    var request = new RestRequest();
                    request.AddParameter("book", Book);
                    request.AddParameter("lesson", Lesson);
                    request.AddParameter("studentID", StudentID);
                    if (waveWriter != null)
                    {
                        waveWriter.Flush();
                        waveWriter.Close();
                        waveWriter = null;
                    }
                    request.AddFile("file", "current.wav");

                    var response = client.Post(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        MainReadingContent = "診斷完成，接著可以查看結果或是前往下個步驟。";
                        UnlockAllButton();
                    }
                    else
                    {
                        MainReadingContent = "雲端語音辨識發生錯誤。";
                        UnlockAllButton();
                    }
                }
            });
            th.Start();


        }

        public void Stop()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }

            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }
        }

        public void Play(string filePath)
        {
            Stop();
            waveOut = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);
            waveOut.Init(audioFile);
            waveOut.Play();
        }
        private WaveOutEvent waveOut;
        private AudioFileReader audioFile;
     
        /// <summary>
        /// 播放錄音
        /// </summary>
        private void PlayRecordCommandAction()
        {
            if (PlayRecordContent == "播放錄音")
            {
                Play("current.wav");
            }
        }


        /// <summary>
        /// 開始錄音按鈕Click
        /// </summary>
        private void StartRecordCommandAction()
        {
            if (RecordButtonContent == "開始錄音")
            {
                Stop();
                DiagnosisRecordingIsStart = true;
                RecordIsNotPlaying = false;
                waveIn = new WaveInEvent();
                waveIn.DataAvailable += WaveIn_DataAvailable;
                waveIn.DeviceNumber = 0;
                waveIn.WaveFormat = new WaveFormat(44100, 2);
              
                waveWriter = new WaveFileWriter("current.wav", waveIn.WaveFormat);
                waveIn.StartRecording();
                RecordButtonContent = "停止錄音";
            }
            else
            {
                waveIn.StopRecording();
                waveIn.Dispose();
                waveIn = null;
                waveWriter.Close();
                waveWriter = null;

                RecordButtonContent = "開始錄音";
                RecordIsNotPlaying = true;
            }
        }

        /// <summary>
        /// 錄音物件實例收到可以用的錄音事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if(waveWriter != null)
                    waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
            }
            catch
            {
                //TODO : 優化事件，不要用try catch處理，影響效能
            }
        }
        #endregion
    }
}