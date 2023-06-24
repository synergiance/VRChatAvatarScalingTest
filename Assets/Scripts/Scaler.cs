
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

public enum ScaleAdjust {
	MinimumHeight, MaximumHeight, EyeHeight, AvatarScale
}

public class Scaler : UdonSharpBehaviour {
	public Toggle enableScalingToggle;
	public Text initialHeightValue;
	public Text currentHeightValue;
	public Text currentScaleValue;

	private float initialAvatarHeight;
	private float currentAvatarHeight;
	private float currentAvatarScale;

	private float eyeHeightSetting;
	private float avatarScaleSetting;

	void Start() {
		if (Networking.LocalPlayer == null) return;
		SetInitialHeight(Networking.LocalPlayer.GetAvatarEyeHeightAsMeters());
		_ToggleScaling();
	}

	public void OnAvatarChanged(VRCPlayerApi player) {
		if (!Utilities.IsValid(player)) return;
		if (!player.isLocal) return;
		SetInitialHeight(player.GetAvatarEyeHeightAsMeters());
	}

	public void OnAvatarEyeHeightChanged(VRCPlayerApi player, float height) {
		if (!Utilities.IsValid(player)) return;
		if (!player.isLocal) return;
		SetCurrentHeight(height);
	}

	public void _AdjustScale(ScaleAdjust scale, float value) {
		if (Networking.LocalPlayer == null) return;
		switch (scale) {
			case ScaleAdjust.MinimumHeight:
				Networking.LocalPlayer.SetAvatarEyeHeightMinimumByMeters(value);
				break;
			case ScaleAdjust.MaximumHeight:
				Networking.LocalPlayer.SetAvatarEyeHeightMaximumByMeters(value);
				break;
			case ScaleAdjust.EyeHeight:
				eyeHeightSetting = value;
				break;
			case ScaleAdjust.AvatarScale:
				avatarScaleSetting = value;
				break;
			default:
				Debug.LogError("Range Error: " + nameof(scale));
				break;
		}
	}

	public void _ApplyHeight() {
		if (Networking.LocalPlayer == null) return;
		Networking.LocalPlayer.SetAvatarEyeHeightByMeters(eyeHeightSetting);
	}

	public void _ApplyScale() {
		if (Networking.LocalPlayer == null) return;
		Networking.LocalPlayer.SetAvatarEyeHeightByMultiplier(avatarScaleSetting);
	}

	public void _ToggleScaling() {
		Networking.LocalPlayer.SetManualAvatarScalingAllowed(enableScalingToggle.isOn);
	}

	private void SetInitialHeight(float height) {
		initialAvatarHeight = height;
		initialHeightValue.text = height.ToString("N2");
		SetCurrentHeight(height);
	}

	private void SetCurrentHeight(float height) {
		currentAvatarHeight = height;
		currentHeightValue.text = height.ToString("N2");
		currentAvatarScale = currentAvatarHeight / initialAvatarHeight;
		currentScaleValue.text = currentAvatarScale.ToString("N2");
	}
}
