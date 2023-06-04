using System;
using System.Diagnostics;
using System.Threading;
using MahAppBase.Command;
using MahAppBase.CustomerUserControl;
using NAudio.Wave;

namespace MahAppBase.ViewModel
{
    public class MainComponent:ViewModelBase
    {
        #region Declarations
        WaveInEvent waveIn = new WaveInEvent();
        ucDonate donate = new ucDonate();
        private string _MainReadingContent;
        private string _RecordButtonContent = "開始錄音";
        private string _PlayRecordContent = "播放錄音";


        BufferedWaveProvider bufferedWaveProvider;

        #endregion

        #region Property
        private bool _RecordIsNotPlaying = false;
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
        public NoParameterCommand ButtonDonateClick { get; set; }
        public NoParameterCommand StartRecordCommand { get; set; }
        public NoParameterCommand PlayRecordCommand { get; set; }
        public NoParameterCommand SendRecordCommand { get; set; }
        public NoParameterCommand CheckReportCommand { get; set; }
        public NoParameterCommand GoNextCommand { get; set; }
        public bool DonateIsOpen { get; set; }
        private WaveOut _PlayRecordingInstance;
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

        public Stopwatch StartRecord { get; set; }
        public Stopwatch PlayRecord { get; set; }
        #endregion

        #region MemberFunction
        public MainComponent()
        {
            ButtonDonateClick = new NoParameterCommand(ButtonDonateClickAction);
            StartRecordCommand = new NoParameterCommand(StartRecordCommandAction);
            PlayRecordCommand = new NoParameterCommand(PlayRecordCommandAction);
            SendRecordCommand = new NoParameterCommand(SendRecordCommandAction);
            CheckReportCommand = new NoParameterCommand(CheckReportCommandAction);
            GoNextCommand = new NoParameterCommand(GoNextCommandAction);
        }

        private void GoNextCommandAction()
        {
            throw new NotImplementedException();
        }

        private void CheckReportCommandAction()
        {
            throw new NotImplementedException();
        }

        private void SendRecordCommandAction()
        {
            MainReadingContent = "正在便是你的語音，請稍後。。。。。";
        }
        
        private void PlayRecordCommandAction()
        {
            if(PlayRecordContent == "播放錄音") 
            {
                RecordIsNotPlaying = false;
                PlayRecord = new Stopwatch();
                PlayRecordingInstance = new WaveOut();
                PlayRecordingInstance.PlaybackStopped += PlayRecordingInstance_PlaybackStopped;
                PlayRecordingInstance.Init(bufferedWaveProvider);
                PlayRecord.Start();
                PlayRecordingInstance.Play();
                Thread th = new Thread(() =>
                {
                    while (PlayRecord.ElapsedMilliseconds - 100 < StartRecord.ElapsedMilliseconds)
                    {
                        Thread.Sleep(100);
                    }
                    PlayRecordContent = "停止播放";
                    PlayRecordingInstance.Stop();
                });
                th.Start();
            }
        }

        private void SaveRecoreToFile()
        {
           
        }

        private void PlayRecordingInstance_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            PlayRecordContent = "播放錄音";
            PlayRecordingInstance.Stop();
            RecordIsNotPlaying = true;
        }

        private void StartRecordCommandAction()
        {
            if (RecordButtonContent == "開始錄音") 
            {
                RecordIsNotPlaying = false;
                StartRecord = new Stopwatch();
                StartRecord.Start();
                WaveInEvent waveIn = new WaveInEvent();
                waveIn.WaveFormat = new WaveFormat();
                waveIn.DataAvailable += WaveIn_DataAvailable;
                WaveOut waveOut = new WaveOut();
                bufferedWaveProvider = new BufferedWaveProvider(waveIn.WaveFormat);

                // 開始錄音
                waveIn.StartRecording();
                RecordButtonContent = "停止錄音";
                

            }
            else 
            {
                waveIn.StopRecording();
                StartRecord.Stop();
                RecordButtonContent = "開始錄音";
                RecordIsNotPlaying = true;
                SaveRecoreToFile();
            }
            
        }

        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        public void ButtonDonateClickAction()
        {
            if (!DonateIsOpen)
            {
                donate = new ucDonate
                {
                    Topmost = true
                };
                donate.Closed += Donate_Closed;
                DonateIsOpen = true;
                donate.Show();
            }
            else
            {

            }
        }

        private void Donate_Closed(object sender, EventArgs e)
        {
            DonateIsOpen = false;
        }

        #endregion
    }
}