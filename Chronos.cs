using System;
using System.Collections.Generic;
using Oxide.Core.Libraries.Covalence;
using UnityEngine;
namespace Oxide.Plugins {
  [Info("Chronos", "RayMods", "0.0.0")]
  class Chronos : CovalencePlugin {
    private AnimationCurve _defaultTimeCurve;
    private float _defaultTotalDayLength;

    private int DAY_LENGTH = 30;  // minutes
    private int NIGHT_LENGTH = 5;  // minutes

    private TOD_Time _timeCtrl;
    private int _totalDayLength;

    private void Loaded() {
      _totalDayLength = DAY_LENGTH + NIGHT_LENGTH;
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
      int nightSpan = 24 * NIGHT_LENGTH / DAY_LENGTH;
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
  }
}