﻿using Howatworks.Thumb.Assistant.Core.Messages;
using System;
using System.Collections.Generic;

namespace Howatworks.Thumb.Assistant.Core
{
    public class ControlStateModel : IControlState
    {
        public bool IsDirty { get; set; }

        private bool _landingGearDown;
        private bool _supercruise;
        private bool _hardpointsDeployed;
        private bool _lightsOn;
        private bool _cargoScoopDeployed;
        private bool _nightVision;
        private bool _hudAnalysisMode;
        private bool _fssMode;
        private bool _saaMode;

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

        public bool FssMode
        {
            get => _fssMode;
            set => UpdateProperty(ref _fssMode, value);
        }

        public bool SaaMode
        {
            get => _saaMode;
            set => UpdateProperty(ref _saaMode, value);
        }

        public ControlState CreateControlStateMessage()
        {
            return new ControlState
            {
                CargoScoopDeployed = this.CargoScoopDeployed,
                HardpointsDeployed = this.HardpointsDeployed,
                HudAnalysisMode = this.HudAnalysisMode,
                LandingGearDown = this.LandingGearDown,
                LightsOn = this.LightsOn,
                NightVision = this.NightVision,
                Supercruise = this.Supercruise,
                FssMode = this.FssMode,
                SaaMode = this.SaaMode
            };
        }

        private void UpdateProperty<T>(ref T originalValue, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(originalValue, newValue))
            {
                originalValue = newValue;
                IsDirty = true;
            }
        }
    }
}