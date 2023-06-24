
using UdonSharp;
using UnityEngine.UI;

public class ScaleSlider : UdonSharpBehaviour {
	public Slider slider;
	public Text value;
	public Scaler scaler;
	public ScaleAdjust scaleSelect;
	public bool sendInitialValue;

	public float Value => slider == null ? 0 : slider.value;

	void Start() {
		value.text = slider.value.ToString("N2");
		if (sendInitialValue) scaler._AdjustScale(scaleSelect, slider.value);
	}

	public void _SetValue() {
		value.text = slider.value.ToString("N2");
		scaler._AdjustScale(scaleSelect, slider.value);
	}
}
