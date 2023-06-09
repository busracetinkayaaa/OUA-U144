using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class interactSnowyFw : MonoBehaviour
{
    public GameObject player;
    public Button collectButton;
    public Button useCapacity;
    private float interactDistance = 18f;
    public List<GameObject> interactiveObjects = new List<GameObject>();
    private List<GameObject> visitedObjects = new List<GameObject>();

    private int pollenCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("FantasyBee");
        collectButton.onClick.AddListener(OnClickButton);
        useCapacity.onClick.AddListener(OnClickUseCapacity);

    }

    // Update is called once per frame
    void Update()
    {
        bool anyObjectInRange = false;
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in interactiveObjects)
        {
            if (!visitedObjects.Contains(obj))
            {
                float distance = Vector3.Distance(player.transform.position, obj.transform.position);

                if (distance < interactDistance && distance < closestDistance)
                {
                    anyObjectInRange = true;
                    closestObject = obj;
                    closestDistance = distance;
                }
            }
        }

        if (collectButton != null)
        {
            collectButton.gameObject.SetActive(anyObjectInRange);

            if (anyObjectInRange)
            {

                Vector3 objPosition = closestObject.transform.position;
                Vector3 buttonPosition = Camera.main.WorldToScreenPoint(objPosition);

                buttonPosition += new Vector3(150f, 100f, 0f);
                Debug.Log("The distance to " + buttonPosition);
                RectTransform canvasRectTransform = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
                Vector2 viewPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, buttonPosition, null, out viewPos);
                collectButton.GetComponent<RectTransform>().anchoredPosition = viewPos;
            }
        }
    }

    public void AddInteractiveObject(GameObject obj)
    {
        if (!interactiveObjects.Contains(obj))
        {
            interactiveObjects.Add(obj);
        }
    }

    public void RemoveInteractiveObject(GameObject obj)
    {
        if (interactiveObjects.Contains(obj))
        {
            interactiveObjects.Remove(obj);
        }
    }
    public void OnClickButton()
    {
        GameObject closestObject = FindClosestInteractiveObject();

        if (closestObject != null)
        {
            visitedObjects.Add(closestObject);

            if (useCapacity.interactable)
            {
                if (pollenCounter < 150)
                {
                    pollenCounter += 10;
                    if (pollenCounter >= 150)
                    {
                        collectButton.interactable = false;
                        useCapacity.interactable = false;
                    }
                }
            }
            else
            {
                if (pollenCounter < 100)
                {
                    pollenCounter += 10;
                    if (pollenCounter >= 100)
                    {
                        collectButton.interactable = false;
                    }
                }
            }
        }
    }

    public void OnClickUseCapacity()
    {
        GameObject closestObject = FindClosestInteractiveObject();

        if (closestObject != null)
        {
            visitedObjects.Add(closestObject);

            if (pollenCounter < 150)
            {
                pollenCounter += 10;
                if (pollenCounter >= 150)
                {
                    collectButton.interactable = false;
                    useCapacity.interactable = false;
                }
            }

        }
    }

    private GameObject FindClosestInteractiveObject()
    {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject obj in interactiveObjects)
        {
            if (!visitedObjects.Contains(obj))
            {
                float distance = Vector3.Distance(player.transform.position, obj.transform.position);

                if (distance < interactDistance && distance < closestDistance)
                {
                    closestObject = obj;
                    closestDistance = distance;
                }
            }
        }

        return closestObject;
    }


}
