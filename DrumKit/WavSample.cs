using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DrumKit
{
    class WavSample
    {
        private MediaPlayer _player;
        private string _path;
        private bool _loaded;
        private object _loadLock;

        private int _delayInMs = 35;
        private double _lastTimeInMs = 0;

        public string Name { get; set; }

        public long SleepInMs { get; set; }

        public WavSample(string path)
        {
            _player = new MediaPlayer();
            _path = path;
            _loaded = false;
            _loadLock = new object();
            Name = Path.GetFileNameWithoutExtension(path);
        }

        public WavSample(string path, long sleep) : this(path)
        {
            SleepInMs = sleep;
        }

        public string GetPath()
        {
            return _path;
        }

        public WavSample Load()
        {
            lock (_loadLock)
            {
                if (!_loaded)
                {
                    _loaded = true;
                    _player.Open(new Uri(_path));
                }
            }

            return this;
        }

        public void SetVolume(double v)
        {
            _player.Volume = v;
        }

        public MediaPlayer GetPlayer()
        {
            return _player;
        }

        public void Play(bool green)
        {
            if (DateTime.Now.TimeOfDay.TotalMilliseconds - _lastTimeInMs >= (green ? 10 + _delayInMs : _delayInMs))
            {
                _player.Position = new TimeSpan(0);
                _player.Play();
                _lastTimeInMs = DateTime.Now.TimeOfDay.TotalMilliseconds;
            }
        }
    }
}
