using NReco.VideoConverter;
using System;
using System.IO;
using System.Net;
using NAudio;
using NAudio.Wave;
using System.Threading;

namespace YoutubeExtractor
{
    public class AduioStreamer
    {
        public event EventHandler StreamStarted;
        public event EventHandler<ProgressEventArgs> StreamPositionChanged;
        public event EventHandler StreamFinished;

        public VideoInfo Video { get; private set; }

        public AduioStreamer(VideoInfo video)
        {
            this.Video = video;
        }

        public int DownloadSize { get; private set; }

        public void Execute()
        {
            if(this.StreamStarted != null)
                this.StreamStarted(this, EventArgs.Empty);

            MediaFoundationReader reader = new MediaFoundationReader(this.Video.DownloadUrl);
            WaveOut player = new WaveOut(); 
            player.Init(reader);
            player.Play();

            while(player.PlaybackState != PlaybackState.Stopped)
            {
                if (this.StreamPositionChanged != null)
                    this.StreamPositionChanged(this, new ProgressEventArgs(reader.CurrentTime.Seconds * reader.TotalTime.Seconds / 100));
                Thread.Sleep(1000);
            }

            if(this.StreamFinished != null)
                this.StreamFinished(this, EventArgs.Empty);
        }
    }
}