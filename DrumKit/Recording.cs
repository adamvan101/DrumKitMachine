using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrumKit
{
    class Recording
    {
        private List<WavSample> _samples = new List<WavSample>();
        private Stopwatch _timer = new Stopwatch();
        private long _lastTime = 0;

        private WasapiLoopbackCapture _capture;
        private WaveFileWriter _writer;

        public bool IsRecording { get; set; }

        private void Init()
        {
            _capture = new WasapiLoopbackCapture();
            _capture.DataAvailable += _capture_DataAvailable;
            _capture.RecordingStopped += _capture_RecordingStopped;
        }

        public Recording()
        {
            Init();
        }

        void _capture_RecordingStopped(object sender, StoppedEventArgs e)
        {
            _writer.Dispose();
            _capture.Dispose();
        }

        void _capture_DataAvailable(object sender, WaveInEventArgs e)
        {
            _writer.Write(e.Buffer, 0, e.BytesRecorded);
        }

        public void Add(WavSample s)
        {
            s.SleepInMs = _timer.ElapsedMilliseconds - _lastTime;
            _lastTime = _timer.ElapsedMilliseconds;
            _samples.Add(s);
        }

        public void StartRecord()
        {
            Init();
            _writer = new WaveFileWriter(@"fileout.wav", _capture.WaveFormat);
            _capture.StartRecording();
        }

        public void StopRecord()
        {
            _capture.StopRecording();
        }

        public void Start()
        {
            IsRecording = true;
            _timer.Start();
        }

        public void Stop()
        {
            IsRecording = false;
            _timer.Stop();
            _timer.Reset();
        }

        public void Export(string path)
        {
            var buffer = new byte[3000];
            WaveFileWriter writer = null;

            try
            {
                foreach (WavSample s in _samples)
                {
                    using (var reader = new WaveFileReader(s.GetPath()))
                    {
                        if (writer == null)
                        {
                            writer = new WaveFileWriter(path, reader.WaveFormat);
                        }
                        else
                        {
                            if (!reader.WaveFormat.Equals(writer.WaveFormat))
                            {
                                throw new Exception("Whoops!");
                            }

                            var oneMsSilence = new byte[writer.WaveFormat.AverageBytesPerSecond / 1000];

                            for (int i = 0; i < s.SleepInMs; i++)
                            {
                                writer.Write(oneMsSilence, 0, oneMsSilence.Length);
                            }
                        }

                        int read;
                        while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            writer.Write(buffer, 0, read);
                        }
                    }
                }
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
        }

        public void Play()
        {
            if (IsRecording)
            {
                return;
            }

            Task.Factory.StartNew(() =>
            {
                List<WavSample> samples = _samples.ConvertAll(s => new WavSample(s.GetPath(), s.SleepInMs));

                foreach (WavSample s in samples)
                {
                    s.Load();
                }

                foreach (WavSample s in samples)
                {
                    Thread.Sleep((int)s.SleepInMs);
                    s.Play(false);
                }
            });
        }
    }
}
