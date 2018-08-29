using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabTextPaddingChanger : MonoBehaviour {
	public HorizontalLayoutGroup groupLayout;
	public Toggle toggle;
	public int activeTop, activeBottom;
	private int inactiveTop, inactiveBottom;
	
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		inactiveTop = groupLayout.padding.top;
		inactiveBottom = groupLayout.padding.bottom;
		toggle.onValueChanged.AddListener(delegate {
			ToggleValueChanged();
		});

		ToggleValueChanged();
	}

	public void ToggleValueChanged() {
		int bottomPadding, topPadding;

		if (toggle.isOn) {
			topPadding = activeTop;
			bottomPadding = activeBottom;
		}
		else {
			topPadding = inactiveTop;
			bottomPadding = inactiveBottom;
		}

		RectOffset tempPadding = new RectOffset(
 	        groupLayout.padding.left,
 	        groupLayout.padding.right,
 	        groupLayout.padding.top,
 	        groupLayout.padding.bottom);

 		tempPadding.top = topPadding;
 		tempPadding.bottom = bottomPadding;
 		groupLayout.padding = tempPadding;
	}
}