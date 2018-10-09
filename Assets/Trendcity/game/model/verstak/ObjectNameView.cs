using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

[ExecuteInEditMode]

public class ObjectNameView : MonoBehaviour
{

    public string text = "<b>Создается</b> <color=#ffea00>Бревно</color> осталось";
    public int textSize = 14;
    public Font textFont;
    public Color textColor = Color.white;
    public float textHeight = 1.15f;
    public bool showShadow = true;
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowOffset = new Vector2(1, 1);
    private string textShadow;
    private Transform playerTrans = null;

    void Awake()
    {
        enabled = false;
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        TextShadowReady();
    }

    void TextShadowReady()
    {
        textShadow = Regex.Replace(text, "<color[^>]+>|</color>", string.Empty);
    }

    void OnGUI()
    {
        if (Vector3.Distance(playerTrans.position, this.transform.position) < 10f) { 
        GUI.depth = 9999;

        GUIStyle style = new GUIStyle();
        style.fontSize = textSize;
        style.richText = true;
        if (textFont) style.font = textFont;
        style.normal.textColor = textColor;
        style.alignment = TextAnchor.MiddleCenter;

        GUIStyle shadow = new GUIStyle();
        shadow.fontSize = textSize;
        shadow.richText = true;
        if (textFont) shadow.font = textFont;
        shadow.normal.textColor = shadowColor;
        shadow.alignment = TextAnchor.MiddleCenter;

        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + textHeight, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition.y = Screen.height - screenPosition.y;

        if (showShadow) GUI.Label(new Rect(screenPosition.x + shadowOffset.x, screenPosition.y + shadowOffset.y, 0, 0), textShadow, shadow);
        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), text, style);
        }
    }

    void OnBecameVisible()
    {
        enabled = true;
    }

    void OnBecameInvisible()
    {
        enabled = false;
    }
}