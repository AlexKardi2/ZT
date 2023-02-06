using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverheadMessage : MonoBehaviour
{
    private float redMessageTime = 1f;
    private float redMessageMoveUpDistance = 0.5f;
    
    [SerializeField] private TextMeshProUGUI overheadText;
    [SerializeField] private TextMeshProUGUI redText;
    private CombatCharacter combatCharacter;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        //overheadText = GetComponentInChildren<TextMeshProUGUI>(true);
        combatCharacter = GetComponentInParent<CombatCharacter>();
        startPosition = redText.transform.localPosition;
        ShowHP();
    }

    public void ShowHP() => overheadText.text=(combatCharacter?.HP.ToString()+ " HP");
    public void Show(string text) => overheadText.text = text;

    public void ShowRed (string text)
    {
        RectTransform redTextTransform = redText.gameObject.GetComponent<RectTransform>();
        redText.gameObject.SetActive(true);
        redText.text = text;

        StartCoroutine(MoveTextUp());

        IEnumerator MoveTextUp()
        {
            while (redTextTransform.localPosition.y < (startPosition.y + redMessageMoveUpDistance))
            {
                redTextTransform.Translate(Vector3.up * redMessageMoveUpDistance / redMessageTime * Time.deltaTime);
                yield return null;
            }
            redTextTransform.localPosition = startPosition;
            redText.gameObject.SetActive(false);
        }
    }

}
