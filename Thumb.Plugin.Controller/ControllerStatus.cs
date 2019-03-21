using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Thumb.Plugin.Controller
{
    public class ControllerStatus
    {
        private bool _landingGearDown;
        private bool _supercruise;
        private bool _hardpointsDeployed;
        private bool _lightsOn;
        private bool _cargoScoopDeployed;
        private bool _nightVision;
        private bool _hudAnalysisMode;

        public event EventHandler Changed;

        [JsonProperty]
        public bool LandingGearDown
        {
            get => _landingGearDown;
            set => UpdateProperty(ref _landingGearDown, value);
        }


        [JsonProperty]
        public bool Supercruise
        {
            get => _supercruise;
            set => UpdateProperty(ref _supercruise, value);
        }

        [JsonProperty]
        public bool HardpointsDeployed
        {
            get => _hardpointsDeployed;
            set => UpdateProperty(ref _hardpointsDeployed, value);
        }

        [JsonProperty]
        public bool LightsOn
        {
            get => _lightsOn;
            set => UpdateProperty(ref _lightsOn, value);
        }

        [JsonProperty]
        public bool CargoScoopDeployed
        {
            get => _cargoScoopDeployed;
            set => UpdateProperty(ref _cargoScoopDeployed, value);
        }

        [JsonProperty]
        public bool NightVision
        {
            get => _nightVision;
            set => UpdateProperty(ref _nightVision, value);
        }

        [JsonProperty]
        public bool HudAnalysisMode
        {
            get => _hudAnalysisMode;
            set => UpdateProperty(ref _hudAnalysisMode, value);
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