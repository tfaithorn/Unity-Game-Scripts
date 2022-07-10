using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Controls the UI reticle.
/// The reticle is zoomed for certain actions, such as firing a bow
/// </summary>
public class Reticle : MonoBehaviour
{
    public delegate void RecticleTargetChange(CharacterMB character);
    public event RecticleTargetChange reticleTargetChange;

    public enum PositionType { 
        ZOOMED = 1,
        RESTING = 2,
        HIDDEN = 3,
        PULSING = 4
    }

    private RectTransform reticle;
    private PositionType position;

    [SerializeField] private RectTransform topReticle;
    [SerializeField] private RectTransform bottomReticle;
    [SerializeField] private RectTransform leftReticle;
    [SerializeField] private RectTransform rightReticle;
    [SerializeField] private RectTransform reticleCenter;
    [SerializeField] private RectTransform reticleCircle;

    [SerializeField] private PlayerCharacterMB playerCharacterMB;
    
    Image topRectImage;
    Image bottomRectImage;
    Image leftRectImage;
    Image rightRectImage;

    public Ray crossHairRay;
    public Vector3 crossHairHitPoint;
    public GameObject crossHairHitTarget;
    public GameObject currentTarget;

    public CameraAnchor cameraAnchor;
    public Camera cam;

    private float relativeZoomDistance = 20f;
    private float relativeRestingDistance = 0f;

    private float distance = 0f;
    private float previousCurrentDistance = 0f;
    private float moveTime = 0f;
    private float ElapsedTime = 0f;

    public bool targetFound;

    Vector2 startPosLeft;
    Vector2 startPosRight;
    Vector2 startPosTop;
    Vector2 startPosBottom;

    private bool rectPositionChanging = true;

    private void Start()
    {
        reticle = GetComponent<RectTransform>();
        position = PositionType.RESTING;
        rectPositionChanging = true;
        topRectImage = topReticle.GetComponent<Image>();
        bottomRectImage = bottomReticle.GetComponent<Image>();
        leftRectImage = leftReticle.GetComponent<Image>();
        rightRectImage = rightReticle.GetComponent<Image>();
        ChangeReticleAlpha(0.5f);
    }

    private void Update()
    {
        EvaluateReticleTarget();
        EvaluateReticlePosition();
    }

    private void EvaluateReticleTarget()
    {
        crossHairRay = cam.ScreenPointToRay(reticle.position);
        //crossHairRay = cam.ScreenPointToRay(new Vector2(reticleCenter.position.x, reticleCenter.position.y));
        RaycastHit[] hits;

        //start ray cast from player's location not from UI elemnt
        float UiToCharacterDistance = Vector3.Distance(crossHairRay.origin, new Vector3(playerCharacterMB.characterModelTransform.position.x, crossHairRay.origin.y, playerCharacterMB.characterModelTransform.position.z));

        //Vector3 characterPos = crossHairRay.origin + (cam.transform.forward.normalized * UiToCharacterDistance);
        Vector3 characterPos = cameraAnchor.transform.position;

        Debug.DrawRay(characterPos, crossHairRay.direction * 10);

        hits = Physics.RaycastAll(characterPos, crossHairRay.direction).OrderBy(h => h.distance).ToArray();

        if (hits.Length > 0)
        {
            crossHairHitPoint = hits[0].point;
            crossHairHitTarget = hits[0].collider.gameObject;

            if (currentTarget != crossHairHitTarget)
            {
                CharacterMB character = crossHairHitTarget.GetComponentInParent<CharacterMB>();
                if (character)
                {
                    reticleTargetChange?.Invoke(character.GetComponent<CharacterMB>());
                    currentTarget = crossHairHitTarget;
                }
            }

            targetFound = true;
        }
        else {
            targetFound = false;
        }
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(crossHairHitPoint, 0.2f);
    }

    private void EvaluateReticlePosition()
    {
        if (rectPositionChanging)
        {
            switch (position)
            {
                case PositionType.RESTING:
                    MoveReticle(relativeRestingDistance);
                    break;
                case PositionType.ZOOMED:
                    MoveReticle(relativeZoomDistance);
                    break;
                case PositionType.HIDDEN:
                    break;
            }
        }
    }

    public void ChangePosition(PositionType positionType, float duration)
    {
        position = positionType;
        rectPositionChanging = true;
        moveTime = duration;
        ElapsedTime = 0f;

        startPosTop = topReticle.anchoredPosition;
        startPosRight = rightReticle.anchoredPosition;
        startPosLeft = leftReticle.anchoredPosition;
        startPosBottom = bottomReticle.anchoredPosition;

    }
    private bool MoveToPosition(RectTransform rect, Vector2 target, Vector2 startPos)
    {
        if (rect.anchoredPosition != target)
        {
            rect.anchoredPosition = Vector2.Lerp(startPos, target, ElapsedTime / moveTime);

            return false;
        }

        return true;
    }
    public void ChangeReticleAlpha(float alpha)
    {
        var tempColor = topRectImage.color;
        tempColor.a = alpha;

        topRectImage.color = tempColor;
        bottomRectImage.color = tempColor;
        leftRectImage.color = tempColor;
        rightRectImage.color = tempColor;

    }

    private void MoveReticle(float moveDis)
    {
        ElapsedTime += Time.deltaTime;

        distance = Mathf.Lerp(previousCurrentDistance, moveDis, ElapsedTime/moveTime);

        topReticle.anchoredPosition = new Vector2(0, -distance);
        bottomReticle.anchoredPosition = new Vector2(0, distance);
        leftReticle.anchoredPosition = new Vector2(distance, 0);
        rightReticle.anchoredPosition = new Vector2(-distance, 0);
    }

    public void EnableReticleCircle()
    {
        reticleCircle.gameObject.SetActive(true);
    }

    public void DisableReticleCircle()
    {
        reticleCircle.gameObject.SetActive(false);
    }


}
