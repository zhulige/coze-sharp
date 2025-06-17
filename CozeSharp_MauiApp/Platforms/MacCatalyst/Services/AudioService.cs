using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CozeSharp.Services;

namespace CozeSharp_MauiApp.Services
{
    public class AudioService : IDisposable, IAudioService
    {
        public bool IsPlaying => throw new NotImplementedException();

        public bool IsRecording => throw new NotImplementedException();

        public event IAudioService.PcmAudioEventHandler? OnPcmAudioEvent;

        public void AddOutSamples(byte[] pcmData)
        {
            throw new NotImplementedException();
        }

        public void AddOutSamples(float[] pcmData)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void StartPlaying()
        {
            throw new NotImplementedException();
        }

        public void StartRecording()
        {
            throw new NotImplementedException();
        }

        public void StopPlaying()
        {
            throw new NotImplementedException();
        }

        public void StopRecording()
        {
            throw new NotImplementedException();
        }
    }
}
