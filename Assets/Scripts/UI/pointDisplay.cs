using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class pointDisplay : MonoBehaviour
{
    [SerializeField] private Points points;
    [SerializeField] private GameObject TextParent;
    [SerializeField] private TextMeshProUGUI pointsText;

    private void OnEnable()
    {
        points.ClientOnPointsUpdated += HandleHealthUpdated;
    }


    private void OnDisable()
    {
        points.ClientOnPointsUpdated -= HandleHealthUpdated;
    }

    private void Start()
    {
        spawnPointsText();
    }

    private void HandleHealthUpdated(int currentPoints, int maxPoints)
    {
        pointsText.text = currentPoints.ToString();
    }

    private void spawnPointsText()
    {
        GameObject parentRef = Instantiate(TextParent,
                    transform.position -
                    new Vector3(3f * (transform.position.x /
                    Mathf.Abs(transform.position.x)), -3, 0),
                    Quaternion.identity);

        pointsText = parentRef.GetComponentInChildren<TextMeshProUGUI>();
    }

}
