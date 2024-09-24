using UnityEngine.UIElements;

public static class VisualElementExtensions {
	public static T GeFirstObjectTypeUp<T>(this VisualElement element) where T : VisualElement {
		VisualElement parent = element.parent;

		var searchVE = element;
        while (searchVE != null) {
			if (searchVE is T typedObj) return typedObj;
            searchVE = searchVE.parent;
		}
		return null;
	}
}