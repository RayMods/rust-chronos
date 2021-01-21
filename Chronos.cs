using System;
using System.Collections.Generic;
using Oxide.Core.Libraries.Covalence;
using UnityEngine;

namespace Oxide.Plugins {
  [Info("Chronos", "RayMods", "0.1.0")]
  [Description("Provides simple day/night cycle customization.")]
  class Chronos : CovalencePlugin {
    private AnimationCurve _defaultTimeCurve;
    private float _defaultTotalDayLength;
    private int _totalDayLength;
    private PluginConfig _config;
    private TOD_Time _timeCtrl;

    private void Init() {
      _config = Config.ReadObject<PluginConfig>();
      _totalDayLength = _config.DAY_LENGTH + _config.NIGHT_LENGTH;
    }

    protected override void LoadDefaultConfig() {
      Config.WriteObject(GetDefaultConfig(), true);
    }

    private void Loaded() {
      LoadTimeController();
    }

    private void Unload() {
      _timeCtrl.DayLengthInMinutes = _defaultTotalDayLength;
      _timeCtrl.TimeCurve = _defaultTimeCurve;
      _timeCtrl.RefreshTimeCurve();
    }

    private void LoadTimeController(int callTimes = 0) {
      if (TOD_Sky.Instance == null) {
        if (callTimes < 10) {
          timer.Once(10, () => LoadTimeController(callTimes + 1));
        } else {
          PrintError("Time Controller component not loaded.");
        }
        return;
      }

      _timeCtrl = TOD_Sky.Instance.Components.Time;
      _defaultTimeCurve = _timeCtrl.TimeCurve;
      _defaultTotalDayLength = _timeCtrl.DayLengthInMinutes;
      SetTimeConfig();
    }

    private void SetTimeConfig() {
      int nightSpan = 24 * _config.NIGHT_LENGTH / _config.DAY_LENGTH;
      AnimationCurve newTimeCurve = new AnimationCurve(
        new Keyframe(0, 0),
        new Keyframe(nightSpan / 2, 9),
        new Keyframe(24 - nightSpan / 2, 18),
        new Keyframe(24, 24)
      );

      _timeCtrl.DayLengthInMinutes = _totalDayLength;
      _timeCtrl.TimeCurve = newTimeCurve;
      _timeCtrl.RefreshTimeCurve();
    }

    private PluginConfig GetDefaultConfig() {
      return new PluginConfig {
        DAY_LENGTH = 45,
        NIGHT_LENGTH = 15,
      };
    }

    private class PluginConfig {
      public int DAY_LENGTH;
      public int NIGHT_LENGTH;
    }
  }
}