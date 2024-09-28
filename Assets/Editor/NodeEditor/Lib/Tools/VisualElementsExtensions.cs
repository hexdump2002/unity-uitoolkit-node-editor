using UnityEngine.UIElements;
using UnityEngine;

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

    public static Vector2 GetMiddlePoint(this VisualElement element)  {
		// 24 because of the size of the bar at to top
        return new Vector2(element.worldBound.xMin + element.worldBound.width / 2,
                element.worldBound.yMin + element.worldBound.height / 2 - 24); ;
    }

    public static Vector2 GetTopLeft(this VisualElement element)  {
        // 24 because of the size of the bar at to top
        return new Vector2(element.worldBound.xMin, element.worldBound.yMin  - 24); ;
    }

    public static Vector2 GetTopRight(this VisualElement element)  {
        // 24 because of the size of the bar at to top
        return new Vector2(element.worldBound.xMin + element.worldBound.width,
                element.worldBound.yMin - 24); ;
    }
    
    public static Vector2 GetBottomRight(this VisualElement element) {
        // 24 because of the size of the bar at to top
        return new Vector2(element.worldBound.xMin + element.worldBound.width,
                element.worldBound.yMin + element.worldBound.height- 24);
    }

    public static Vector2 GetBottomLeft(this VisualElement element) {
        // 24 because of the size of the bar at to top
        return new Vector2(element.worldBound.xMin,
                element.worldBound.yMin + element.worldBound.height - 24); ;
    }
}
