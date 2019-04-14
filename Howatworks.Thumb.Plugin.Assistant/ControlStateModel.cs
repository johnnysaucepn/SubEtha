using System;
using System.Collections.Generic;
using Howatworks.Thumb.Plugin.Assistant.Messages;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public class ControlStateModel : IControlState
    {
        private bool _landingGearDown;
        private bool _supercruise;
        private bool _hardpointsDeployed;
        private bool _lightsOn;
        private bool _cargoScoopDeployed;
        private bool _nightVision;
        private bool _hudAnalysisMode;

        public event EventHandler Changed;

        public bool LandingGearDown
        {
            get => _landingGearDown;
            set => UpdateProperty(ref _landingGearDown, value);
        }

        public bool Supercruise
        {
            get => _supercruise;
            set => UpdateProperty(ref _supercruise, value);
        }

        public bool HardpointsDeployed
        {
            get => _hardpointsDeployed;
            set => UpdateProperty(ref _hardpointsDeployed, value);
        }

        public bool LightsOn
        {
            get => _lightsOn;
            set => UpdateProperty(ref _lightsOn, value);
        }

        public bool CargoScoopDeployed
        {
            get => _cargoScoopDeployed;
            set => UpdateProperty(ref _cargoScoopDeployed, value);
        }

        public bool NightVision
        {
            get => _nightVision;
            set => UpdateProperty(ref _nightVision, value);
        }

        public bool HudAnalysisMode
        {
            get => _hudAnalysisMode;
            set => UpdateProperty(ref _hudAnalysisMode, value);
        }

        public ControlState CreateControlStateMessage()
        {
            return new ControlState
            {
                CargoScoopDeployed = CargoScoopDeployed,
                HardpointsDeployed = HardpointsDeployed,
                HudAnalysisMode = HudAnalysisMode,
                LandingGearDown = LandingGearDown,
                LightsOn = LightsOn,
                NightVision = NightVision,
                Supercruise = Supercruise
            };
        }

        private void UpdateProperty<T>(ref T originalValue, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(originalValue, newValue))
            {
                originalValue = newValue;
                Changed?.Invoke(this, new EventArgs());
            }
        }

    }
}