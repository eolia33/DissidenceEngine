using System;
using CitizenFX.Core;
using Client;
using Configuration;
using static CitizenFX.Core.Native.API;


namespace SaltyTalkieClient
{
    public class Radio : BaseScript
    {
        #region Props/Fields

        private Random random { get; set; }
        private SharedConfig Config { get; }
        public NuiState NuiState { get; set; }

        public bool _isPoweredOn = true;

        public bool IsPoweredOn
        {
            get => _isPoweredOn;
            set
            {
                _isPoweredOn = value;

                if (value)
                {
                    if (_lastPrimaryRadioChannel != null)
                        PrimaryRadioChannel = _lastPrimaryRadioChannel;

                    if (_lastSecondaryRadioChannel != null)
                        SecondaryRadioChannel = _lastSecondaryRadioChannel;
                }
                else
                {
                    if (PrimaryRadioChannel != null)
                    {
                        _lastPrimaryRadioChannel = PrimaryRadioChannel;
                        PrimaryRadioChannel      = null;
                    }

                    if (SecondaryRadioChannel != null)
                    {
                        _lastSecondaryRadioChannel = SecondaryRadioChannel;
                        SecondaryRadioChannel      = null;
                    }
                }
            }
        }

        public bool IsMicClickEnabled
        {
            get => Exports["saltychat"].GetMicClick();
            set => Exports["saltychat"].SetMicClick(value);
        }

        public string PlayerName => Exports["saltychat"].GetPlayerName();

        private string _lastPrimaryRadioChannel;

        public string PrimaryRadioChannel
        {
            get => Exports["saltychat"].GetRadioChannel(true);
            set => Exports["saltychat"].SetRadioChannel(value, true);
        }

        private string _lastSecondaryRadioChannel;

        public string SecondaryRadioChannel
        {
            get => Exports["saltychat"].GetRadioChannel(false);
            set => Exports["saltychat"].SetRadioChannel(value, false);
        }

        public int RadioVolume
        {
            get => (int)(Exports["saltychat"].GetRadioVolume() * 100);
            set
            {
                float volume = value;
                Exports["saltychat"].SetRadioVolume(volume / 100);
            }
        }

        public bool IsRadioSpeakerEnabled
        {
            get => Exports["saltychat"].GetRadioSpeaker();
            set => Exports["saltychat"].SetRadioSpeaker(value);
        }

        public float VoiceRange => Exports["saltychat"].GetVoiceRange();

        #endregion

        #region CTOR

        public Radio(SharedConfig config, Bridge bridge, Player player, NuiState nuiState)
        {
            Config       = config;
            _isPoweredOn = true;

            RegisterCommand("+focusRadio", new Action(OnFocusPressed),  false);
            RegisterCommand("-focusRadio", new Action(OnFocusReleased), false);
            RegisterKeyMapping("+focusRadio", "Focus Radio", "keyboard", "F6");
        }

        #endregion

        #region Radio Events

        public void OnPrimaryRadioChannelChanged(string radioChannel, bool isPrimaryChannel)
        {
            SendNuiMessage(
                new NuiMessage(
                    isPrimaryChannel ? NuiMessageType.SetPrimaryRadioChannel : NuiMessageType.SetSecondaryRadioChannel,
                    new RadioChannel(radioChannel)
                ).ToString()
            );
        }

        public void OnRadioTrafficStateChanged(bool primaryReceive, bool primaryTransmit, bool secondaryReceive,
            bool secondaryTransmit)
        {
            SendNuiMessage(
                new NuiMessage(
                    NuiMessageType.SetRadioState,
                    new RadioState(primaryReceive, primaryTransmit, secondaryReceive, secondaryTransmit)
                ).ToString()
            );
        }

        #endregion

        #region NUI

        public void OnNuiReady(dynamic dummy, dynamic cb)
        {
            cb(
                new InitData(
                    IsPoweredOn,
                    IsRadioSpeakerEnabled,
                    PrimaryRadioChannel,
                    SecondaryRadioChannel,
                    IsMicClickEnabled,
                    RadioVolume
                ).ToString()
            );
        }

        public void OnNuiSetPrimaryChannel(dynamic channelName, dynamic cb)
        {
            if (channelName == null)
            {
                PrimaryRadioChannel = null;
            }
            else
            {
                if (channelName.GetType() != typeof(string))
                    channelName = Convert.ToString(channelName);

                PrimaryRadioChannel = $"st_{channelName}";
            }

            cb("");
        }

        public void OnNuiSetSecondaryChannel(dynamic channelName, dynamic cb)
        {
            if (channelName == null)
            {
                SecondaryRadioChannel = null;
            }
            else
            {
                if (channelName.GetType() != typeof(string))
                    channelName = Convert.ToString(channelName);

                SecondaryRadioChannel = $"st_{channelName}";
            }

            cb("");
        }

        public void OnNuiToggleMicClick(dynamic dummy, dynamic cb)
        {
            IsMicClickEnabled = !IsMicClickEnabled;

            cb(IsMicClickEnabled);
        }

        public void OnNuiToggleSpeaker(dynamic dummy, dynamic cb)
        {
            IsRadioSpeakerEnabled = !IsRadioSpeakerEnabled;

            cb(IsRadioSpeakerEnabled);
        }

        public void OnNuiTogglePower(dynamic dummy, dynamic cb)
        {
            IsPoweredOn = !IsPoweredOn;
            cb(IsPoweredOn);
        }

        public void OnNuiVolumeUp(dynamic dummy, dynamic cb)
        {
            RadioVolume += 10;

            cb(RadioVolume);
        }

        public void OnNuiVolumeDown(dynamic dummy, dynamic cb)
        {
            RadioVolume -= 10;

            cb(RadioVolume);
        }

        public void OnNuiUnfocus(dynamic dummy, dynamic cb)
        {
            SetNuiFocus(false, false);

            cb("");
        }

        #endregion

        #region Keybinds

        private void OnFocusPressed()
        {
            SendNuiMessage(new NuiMessage().ToString());
            SetNuiFocus(true, true);
        }

        private void OnFocusReleased()
        {
            // Dummy
        }

        #endregion

        #region Tick

#if DEBUG
        [Tick]
        public async Task FirstTickAsync()
        {
            Tick -= FirstTickAsync;

            API.SetNuiFocus(false, false);

            await Task.FromResult(0);
        }

        //[Tick]
        public async Task RadioTrafficDummyAsync()
        {
            API.SendNuiMessage(
                new NuiMessage(
                    NuiMessageType.SetRadioState,
                    new RadioState(
                        true,
                        false,
                        false,
                        true
                    )
                ).ToString()
            );

            await BaseScript.Delay(888);
            
            API.SendNuiMessage(
                new NuiMessage(
                    NuiMessageType.SetRadioState,
                    new RadioState(
                        false,
                        true,
                        true,
                        false
                    )
                ).ToString()
            );
            
            await BaseScript.Delay(888);
        }
#endif

        #endregion
    }
}