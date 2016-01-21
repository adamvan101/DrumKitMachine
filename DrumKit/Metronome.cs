using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace DrumKit
{
    class Metronome
    {
        private WavSample _sample;

        private Timer _timer;

        public bool IsRunning = false;

        public Metronome()
        {
            _sample = new WavSample(Path.GetFullPath(@"Resources\blip.wav"));
            _sample.Load();
        }

        public void Start(int bpm, Dispatcher dispatcher)
        {
            IsRunning = true;
            _timer = new Timer();
            // bpm to ms delay
            _timer.Interval = 60000.0 / bpm;
            _timer.Elapsed += delegate
            {
                dispatcher.Invoke((Action)(() =>
                {
                    _sample.Play(false);
                }));
            };

            _timer.Start();
        }

        public void Stop()
        {
            IsRunning = false;

            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
        }
    }
}
