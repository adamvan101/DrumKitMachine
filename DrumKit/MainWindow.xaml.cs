using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrumKit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DrumViewModel _viewModel;
        private string WAV_PATH = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DrumSamples");

        private List<WavSample> _wavs = new List<WavSample>();
        private DrumKitController _controller1;
        private DrumKitController _controller2;

        private int _selectedIndex = 0;

        private int SelectedIndex
        {
            get 
            {
                return _selectedIndex;
            }
            set
            {
                try
                {
                    _viewModel.Instruments[(_selectedIndex + _viewModel.Instruments.Count) % _viewModel.Instruments.Count].IsSelected = false;
                }
                catch (Exception e) { /* suppress */ }
                _selectedIndex = (value + _viewModel.Instruments.Count) % _viewModel.Instruments.Count;
                _viewModel.Instruments[(_selectedIndex + _viewModel.Instruments.Count) % _viewModel.Instruments.Count].IsSelected = true;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = this.DataContext as DrumViewModel;
            _viewModel.SetDispatcher(this.Dispatcher);
            this.PreviewKeyDown += MainWindow_KeyDown;
            LoadWavs();
            InitControllers();

            SelectedIndex = 0;
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        public void LoadWavs()
        {
            _viewModel.LoadFiles(WAV_PATH);
        }

        public void InitControllers()
        {
            _controller1 = new DrumKitController(null);
            _controller1.OnButtonEvent += _controller1_OnButtonEvent;

            _controller2 = new DrumKitController(_controller1.GetDevice());
            _controller2.OnButtonEvent += _controller2_OnButtonEvent;
        }

        void _controller1_OnButtonEvent(IMessage m)
        {
            if (m.Depress)
            {
                _controller1.pedalUp = true;
                _controller1.leftUp = true;
                _controller1.rightUp = true;
                _controller1.downUp = true;
                _controller1.upUp = true;
                _controller1.buttonAUp = true;
                _controller1.buttonBUp = true;
                _controller1.button1Up = true;
                _controller1.button2Up = true;
                _controller1.buttonPlusUp = true;
                _controller1.buttonMinusUp = true;
            }
            if (m.ButtonRedPressed)
            {
                _viewModel.Play(Instrument.InstrumentType.Snare);
            }
            if (m.ButtonYellowPressed)
            {
                _viewModel.Play(Instrument.InstrumentType.Tom1);
            }
            if (m.ButtonBluePressed)
            {
                _viewModel.Play(Instrument.InstrumentType.Tom2);
            }
            if (m.ButtonGreenPressed)
            {
                _viewModel.Play(Instrument.InstrumentType.FloorTom, true);
            }
            if (m.ButtonAPressed && _controller1.buttonAUp)
            {
                // Toggle Metronome
                _controller1.buttonAUp = false;
                _viewModel.ToggleMetronome();
            }
            if (m.ButtonBPressed && _controller1.buttonBUp)
            {
                // Load saved configuration
                _controller1.buttonBUp = false;
                _viewModel.Load();
            }
            if (m.ButtonPlusPressed && _controller1.buttonPlusUp)
            {
                _controller1.buttonPlusUp = false;
                _viewModel.Bpm++;
            }
            if (m.ButtonMinusPressed && _controller1.buttonMinusUp)
            {
                _controller1.buttonMinusUp = false;
                _viewModel.Bpm--;
            }
            if (m.RightPressed && _controller1.rightUp)
            {
                _controller1.rightUp = false;
                if (_viewModel.Instruments[SelectedIndex].PlaySoundA)
                {
                    _viewModel.Instruments[SelectedIndex].SoundA = _viewModel.Instruments[SelectedIndex].SoundList[(_viewModel.Instruments[SelectedIndex].SoundList.IndexOf(_viewModel.Instruments[SelectedIndex].SoundA) + 1) % _viewModel.Instruments[SelectedIndex].SoundList.Count];
                }
                else
                {
                    _viewModel.Instruments[SelectedIndex].SoundB = _viewModel.Instruments[SelectedIndex].SoundList[(_viewModel.Instruments[SelectedIndex].SoundList.IndexOf(_viewModel.Instruments[SelectedIndex].SoundB) + 1) % _viewModel.Instruments[SelectedIndex].SoundList.Count];
                }
            }
            if (m.LeftPressed && _controller1.leftUp)
            {
                _controller1.leftUp = false;
                if (_viewModel.Instruments[SelectedIndex].PlaySoundA)
                {
                    _viewModel.Instruments[SelectedIndex].SoundA = _viewModel.Instruments[SelectedIndex].SoundList[(_viewModel.Instruments[SelectedIndex].SoundList.IndexOf(_viewModel.Instruments[SelectedIndex].SoundA) - 1 + _viewModel.Instruments[SelectedIndex].SoundList.Count) % _viewModel.Instruments[SelectedIndex].SoundList.Count];
                }
                else
                {
                    _viewModel.Instruments[SelectedIndex].SoundB = _viewModel.Instruments[SelectedIndex].SoundList[(_viewModel.Instruments[SelectedIndex].SoundList.IndexOf(_viewModel.Instruments[SelectedIndex].SoundB) - 1 + _viewModel.Instruments[SelectedIndex].SoundList.Count) % _viewModel.Instruments[SelectedIndex].SoundList.Count];
                }
            }
            if (m.UpPressed && _controller1.upUp)
            {
                _controller1.upUp = false;
                SelectedIndex--;
            }
            if (m.DownPressed && _controller1.downUp)
            {
                _controller1.downUp = false;
                SelectedIndex++;
            }
            if (m.ButtonPedalPressed && _controller1.pedalUp)
            {
                _controller1.pedalUp = false;
                _viewModel.Play(Instrument.InstrumentType.Kick);
            }
            if (m.Button1Pressed && _controller1.button1Up)
            {
                _controller1.button1Up = false;
                _viewModel.Instruments[SelectedIndex].PlaySoundA = !_viewModel.Instruments[SelectedIndex].PlaySoundA;
            }
            if (m.Button2Pressed && _controller1.button2Up)
            {
                _controller1.button2Up = false;
                foreach (Instrument i in _viewModel.Instruments)
                {
                    i.PlaySoundA = !i.PlaySoundA;
                }
            }
        }

        void _controller2_OnButtonEvent(IMessage m)
        {
            if (m.Depress)
            {
                _controller2.pedalUp = true;
                _controller2.leftUp = true;
                _controller2.rightUp = true;
                _controller2.downUp = true;
                _controller2.upUp = true;
                _controller2.buttonAUp = true;
                _controller2.buttonBUp = true;
                _controller2.button1Up = true;
                _controller2.button2Up = true;
                _controller2.buttonPlusUp = true;
                _controller2.buttonMinusUp = true;
            }
            if (m.ButtonRedPressed)
            {
                _viewModel.Play(Instrument.InstrumentType.Hihat);
            }
            if (m.ButtonYellowPressed)
            {
                _viewModel.Play(Instrument.InstrumentType.Crash);
            }
            if (m.ButtonBluePressed)
            {
                _viewModel.Play(Instrument.InstrumentType.Splash);
            }
            if (m.ButtonGreenPressed)
            {
                _viewModel.Play(Instrument.InstrumentType.Ride, true);
            }
            if (m.ButtonAPressed && _controller2.buttonAUp)
            {
                // Toggle Metronome
                _controller2.buttonAUp = false;
                _viewModel.ToggleMetronome();
            }
            if (m.ButtonBPressed && _controller2.buttonBUp)
            {
                // Load saved configuration
                _controller2.buttonBUp = false;
                _viewModel.Load();
            }
            if (m.ButtonPlusPressed && _controller2.buttonPlusUp)
            {
                _controller2.buttonPlusUp = false;
                _viewModel.Bpm++;
            }
            if (m.ButtonMinusPressed && _controller2.buttonMinusUp)
            {
                _controller2.buttonMinusUp = false;
                _controller1.OnButtonEvent -= _controller1_OnButtonEvent;
                _controller2.OnButtonEvent -= _controller2_OnButtonEvent;
                DrumKitController temp = _controller1;
                _controller1 = _controller2;
                _controller2 = temp;
                _controller1.OnButtonEvent += _controller1_OnButtonEvent;
                _controller2.OnButtonEvent += _controller2_OnButtonEvent;
            }
            if (m.RightPressed && _controller2.rightUp)
            {
                _controller2.rightUp = false;
                SelectedIndex++;
            }
            if (m.LeftPressed && _controller2.leftUp)
            {
                _controller2.leftUp = false;
                SelectedIndex--;
            }
            if (m.UpPressed && _controller2.upUp)
            {
                _controller2.upUp = false;
                _viewModel.Instruments[SelectedIndex].SoundA = _viewModel.Instruments[SelectedIndex].SoundList[(_viewModel.Instruments[SelectedIndex].SoundList.IndexOf(_viewModel.Instruments[SelectedIndex].SoundA) - 1 + _viewModel.Instruments[SelectedIndex].SoundList.Count) % _viewModel.Instruments[SelectedIndex].SoundList.Count];
            }
            if (m.DownPressed && _controller2.downUp)
            {
                _controller2.downUp = false;
                _viewModel.Instruments[SelectedIndex].SoundA = _viewModel.Instruments[SelectedIndex].SoundList[(_viewModel.Instruments[SelectedIndex].SoundList.IndexOf(_viewModel.Instruments[SelectedIndex].SoundA) + 1) % _viewModel.Instruments[SelectedIndex].SoundList.Count];
            }
            if (m.ButtonPedalPressed && _controller2.pedalUp)
            {
                _controller2.pedalUp = false;
                _viewModel.Play(Instrument.InstrumentType.Kick);
            }
            if (m.Button1Pressed && _controller2.button1Up)
            {
                _controller2.button1Up = false;
                _viewModel.Instruments[SelectedIndex].PlaySoundA = !_viewModel.Instruments[SelectedIndex].PlaySoundA;
            }
            if (m.Button2Pressed && _controller2.button2Up)
            {
                _controller2.button2Up = false;
                foreach (Instrument i in _viewModel.Instruments)
                {
                    i.PlaySoundA = !i.PlaySoundA;
                }
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Save();
        }

        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar tb = sender as ToolBar;
            var overflowGrid = tb.Template.FindName("OverflowGrid", tb) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = tb.Template.FindName("MainPanelBorder", tb) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void Record_Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.StartRecording();
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.PlayRecording();
        }

        private void StopRecord_Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.StopRecording();
        }

        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ExportRecording(@"another.wav");
        }

        private void View_Controls_Button_Click(object sender, RoutedEventArgs e)
        {
            ControlsView.ControlsWindow ctrlsWin = new ControlsView.ControlsWindow();
            ctrlsWin.Show();
        }

        static List<USBDeviceInfo> GetUSBDevices()
        {
              List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

              ManagementObjectCollection collection;
              using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity "))
                collection = searcher.Get();

              foreach (var device in collection)
              {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description"),
                device
                ));
              }

              collection.Dispose();
              return devices;
        }
      }

    class USBDeviceInfo
    {
        public USBDeviceInfo(string deviceID, string pnpDeviceID, string description, ManagementBaseObject dev)
        {
            this.DeviceID = deviceID;
            this.PnpDeviceID = pnpDeviceID;
            this.Description = description;
            this.Device = dev;
        }
        public ManagementBaseObject Device { get; set; }
        public string DeviceID { get; private set; }
        public string PnpDeviceID { get; private set; }
        public string Description { get; private set; }
    }
}
