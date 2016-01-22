using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace DrumKit
{
    class Instrument : INotifyPropertyChanged
    {
        public enum InstrumentType
        {
            Snare = 0,
            Hihat = 1,
            Tom1 = 2,
            Tom2 = 3,
            FloorTom = 4,
            Kick = 5,
            Crash = 6,
            Ride = 7,
            Splash = 8
        }

        private InstrumentType _type;

        private String _name = null;

        private WavSample _soundA = null;

        private WavSample _soundB = null;

        private ObservableCollection<WavSample> _soundList = new ObservableCollection<WavSample>();

        private bool _isSelected = false;

        private bool _playSoundA = true;

        private Dispatcher _dispatcher;

        public bool PlaySoundA
        {
            get { return _playSoundA; }

            set
            {
                _playSoundA = value;
                OnPropertyChanged("BrushA");
                OnPropertyChanged("BrushB");
                OnPropertyChanged("Volume");
            }
        }

        public InstrumentType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public double Volume
        {
            get
            {
                if (PlaySoundA)
                {
                    return SoundA.GetPlayer().Volume;
                }
                else
                {
                    return SoundB.GetPlayer().Volume;
                }
            }
            set
            {
                if (PlaySoundA && SoundA.GetPlayer().Volume != value)
                {
                    SoundA.SetVolume(value);
                    OnPropertyChanged("Volume");
                }
                else if (SoundB.GetPlayer().Volume != value)
                {
                    SoundB.SetVolume(value);
                    OnPropertyChanged("Volume");
                }
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public WavSample SoundA
        {
            get
            {
                return _soundA;
            }
            set
            {
                if (value != _soundA)
                {
                    _soundA = value;
                    this._dispatcher.Invoke((Action)(() =>
                        {
                            _soundA.Load();
                        }));
                    OnPropertyChanged("SoundA");
                }
            }
        }

        public WavSample SoundB
        {
            get
            {
                return _soundB;
            }
            set
            {
                if (value != _soundB)
                {
                    _soundB = value;
                    this._dispatcher.Invoke((Action)(() =>
                    {
                        _soundB.Load();
                    }));
                    OnPropertyChanged("SoundB");
                }
            }
        }

        public ObservableCollection<WavSample> SoundList
        {
            get
            {
                return _soundList;
            }
            set
            {
                if (value != _soundList)
                {
                    _soundList = value;
                    OnPropertyChanged("SoundList");
                }
            }
        }

        public Brush BrushA
        {
            get
            {
                if (_isSelected && PlaySoundA)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }
        }

        public Brush BrushB
        {
            get
            {
                if (_isSelected && !PlaySoundA)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    OnPropertyChanged("BrushA");
                    OnPropertyChanged("BrushB");
                }
            }
        }

        #region Methods

        public Instrument(ObservableCollection<WavSample> list, InstrumentType type, Dispatcher dispatcher)
        {

            _dispatcher = dispatcher;
            SoundList = list;
            SoundA = list.First();
            SoundA.Load();
            SoundB = list.First();
            SoundB.Load();
            Type = type;
            Name = type.ToString();
            SoundA.SetVolume(0.5);
            SoundB.SetVolume(0.5);
        }

        public Instrument(ObservableCollection<WavSample> list, InstrumentType type, Dispatcher dispatcher, String name)
            : this(list, type, dispatcher)
        {
            Name = name;
        }

        public void Play(bool green)
        {
            if (PlaySoundA)
            {
                if (SoundA != null)
                    SoundA.Play(green);
            }
            else
            {
                if (SoundB != null)
                    SoundB.Play(green);
            }
        }

        public void Load(string name, bool soundA = true)
        {
            if (soundA)
            {
                SoundA = SoundList.FirstOrDefault(w => w.Name == name);
            }
            else
            {
                SoundB = SoundList.FirstOrDefault(w => w.Name == name);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
