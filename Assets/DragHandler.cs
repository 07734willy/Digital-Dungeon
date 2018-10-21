using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Camera mainCamera;
    float zAxis = 0;
    Vector3 clickOffset = Vector3.zero;
	public static GameObject itemBeingDragged;
	public static Vector3 startPosition;
	public static Transform startParent;
	public Sprite image1;
	public Sprite image2;
    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
        zAxis = transform.position.z;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
		startPosition = transform.position;
		startParent = transform.parent;
		startParent.SetAsLastSibling();
		itemBeingDragged = gameObject;
        clickOffset = transform.position - mainCamera.ScreenPointToWoldOnPlane(eventData.position, zAxis);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = mainCamera.ScreenPointToWoldOnPlane(eventData.position, zAxis) + clickOffset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
		GetComponent<CanvasGroup>().blocksRaycasts = true;
		transform.position=startPosition;
    }
}

public static class extensionMethod
{
    public static Vector3 ScreenPointToWoldOnPlane(this Camera cam, Vector3 screenPosition, float zPos)
    {
        float enterDist;
        Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, zPos));
        Ray rayCast = cam.ScreenPointToRay(screenPosition);
        plane.Raycast(rayCast, out enterDist);
        return rayCast.GetPoint(enterDist);
    }
}
