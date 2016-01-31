using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DrumKit
{
    class DrumViewModel : INotifyPropertyChanged
    {
        private Recording _rec = new Recording();

        private ObservableCollection<Instrument> _instruments = new ObservableCollection<Instrument>();

        private int _bpm = 120;

        private Dispatcher _dispatcher;

        private Metronome _metronome;

        public ObservableCollection<Instrument> Instruments
        {
            get
            {
                return _instruments;
            }
            set
            {
                if (value != _instruments)
                {
                    _instruments = value;
                    OnPropertyChanged("Instruments");
                }
            }
        }

        public int Bpm
        {
            get
            {
                return _bpm;
            }
            set
            {
                if (_bpm != value)
                {
                    _bpm = value;
                    OnPropertyChanged("Bpm");
                }
            }
        }

        #region Methods

        public void SetDispatcher(Dispatcher dispatcher)
        {
            _metronome = new Metronome();
            _dispatcher = dispatcher;
        }

        public void ToggleMetronome()
        {
            if (_metronome.IsRunning)
            {
                _metronome.Stop();
            }
            else
            {
                this._dispatcher.Invoke((Action) (() => {
                    _metronome.Start(Bpm, this._dispatcher);
                }));
            }
        }

        public void LoadFiles(string path)
        {
            ObservableCollection<WavSample> samples = new ObservableCollection<WavSample>();

            foreach (string f_name in Directory.EnumerateFiles(path, "*.wav"))
            {
                samples.Add(new WavSample(f_name));
            }

            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.Kick, _dispatcher));
            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.Snare, _dispatcher));
            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.Hihat, _dispatcher));
            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.Tom1, _dispatcher));
            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.Tom2, _dispatcher));
            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.FloorTom, _dispatcher));
            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.Crash, _dispatcher));
            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.Ride, _dispatcher));
            Instruments.Add(new Instrument(samples, Instrument.InstrumentType.Splash, _dispatcher));
        }

        public void Play(Instrument.InstrumentType type, bool green = false)
        {
            this._dispatcher.BeginInvoke((Action) (() =>
            {
                Instruments.First(i => i.Type == type).Play(green);
                if (_rec.IsRecording)
                {
                    _rec.Add(Instruments.First(i => i.Type == type).SoundA);
                }
            }));
        }

        public void Save(bool custom = false)
        {
            string path = @"save.sv";
            if (custom)
            {
                SaveFileDialog fd = new SaveFileDialog();
                Nullable<bool> res = fd.ShowDialog();

                if (res == true)
                {
                    path = fd.FileName;
                }
                else
                {
                    return;
                }
            }
            using (StreamWriter s = new StreamWriter(path, false))
            {
                foreach (Instrument i in Instruments)
                {
                    s.WriteLine(i.SoundA.Name);
                    s.WriteLine(i.SoundB.Name);
                }
            }
        }

        public void Load(bool custom = false)
        {
            string path = @"save.sv";
            if (custom)
            {
                OpenFileDialog fd = new OpenFileDialog();
                Nullable<bool> res = fd.ShowDialog();

                if (res == true) 
                {
                    path = fd.FileName;
                }
                else
                {
                    return;
                }
            }
            using (StreamReader reader = new StreamReader(path))
            {
                foreach (Instrument i in Instruments)
                {
                    this._dispatcher.Invoke((Action)(() =>
                        {
                            i.Load(reader.ReadLine());
                            i.Load(reader.ReadLine(), false);
                        }));
                }
            }
        }

        public void StartRecording()
        {
            //_rec.Start();
            _rec.StartRecord();
        }

        public void StopRecording()
        {
            //_rec.Stop();
            _rec.StopRecord();
        }

        WaveOut waveOut;
        WaveFileReader reader;

        public void PlayRecording()
        {
            //_rec.Play();
            waveOut = new WaveOut();
            waveOut.PlaybackStopped += waveOut_PlaybackStopped;
            if (File.Exists(@"fileout.wav"))
            {
                reader = new WaveFileReader(@"fileout.wav");
            }
            else 
            {
                return;
            }

            waveOut.Init(reader);
            waveOut.Play();
        }

        void waveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            waveOut.Dispose();
            reader.Dispose();
        }

        public void ExportRecording(string path)
        {
            _rec.Export(path);
        }

        #endregion

        #region INotifyPropertyChanged

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
