using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UserDirection : MonoBehaviour
{
    public List<string> tasks = new List<string>();

    public TextMeshProUGUI text;

    public static UserDirection instance;
   
    public int index = 0;

    public void Start()
    {
        instance = this;
        UpdateText();
    }
    public void Updateindex(int index)
    {
        if (index > this.index)
        {
            this.index = index;
            UpdateText();
        }
    }
    public void UpdateText()
    {
        if (index == tasks.Count - 1)
            StartCoroutine(HideText());
        text.text = tasks[index].Replace("\\n", "\n");
    }
    IEnumerator HideText()
    {
        yield return new WaitForSeconds(2);

        text.CrossFadeAlpha(0, 1, false);
    }
}
