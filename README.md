# DrumKitMachine
A early-stage Windows app that turns USB "Rock Band" drum kit(s) into an electric drum set

To load samples, place .wav files in a /Documents/DrumSamples/ directory (to be configurable later).

Features:
* Supports up to 2 USB (only tested with Wii) RockBand drum kits
	* This allows for up to 8 pads, 2 pedals, and the button interface on each kit (D-pad, 1, 2, A, B, +, -)
* Save/Load configurations for drum pads
* Remappable sounds for each pad
* Metronome with configurable tempo
* Audio Recording and Playback/Exporting


Todo:
* Remappable controls
* New controls for drum 2 (currently they clone drum 1)
* Configurable sample and export paths
* Configurable state save/load location
* Upgrade UI
* Second Pedal (hi-hat)
	* We want total configuration over this being a second bass or a hi-hat
		* Should be able to switch between the two at runtime
	* Hi-hat should have two samples: one if pedal down, one if pedal not down (also sample for when pedal goes down)
* Error handling for controllers coming in and out (or not in at all)
