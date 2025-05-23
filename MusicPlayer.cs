using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.SDL;

namespace TheAdventure.Audio
{
    using NAudio.Wave;

    public class MusicPlayer
    {
        private IWavePlayer? outputDevice;
        private AudioFileReader? audioFile;
        public bool IsPlaying { get; private set; }
        public void PlayLoop(string path)
        {
            if (IsPlaying) return;
            outputDevice = new WaveOutEvent();
            audioFile = new AudioFileReader(path);
            audioFile.Volume = 0.5f;

            var loop = new LoopStream(audioFile);
            outputDevice.Init(loop);
            outputDevice.Play();
        }

        public void Stop()
        {
            outputDevice?.Stop();
            outputDevice?.Dispose();
            audioFile?.Dispose();
        }
    }

    // Pentru looping:
    public class LoopStream : WaveStream
    {
        private readonly WaveStream sourceStream;

        public LoopStream(WaveStream sourceStream)
        {
            this.sourceStream = sourceStream;
        }

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;
        public override long Length => long.MaxValue;
        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = sourceStream.Read(buffer, offset, count);
            if (read == 0)
            {
                sourceStream.Position = 0;
                read = sourceStream.Read(buffer, offset, count);
            }
            return read;
        }
    }

}
