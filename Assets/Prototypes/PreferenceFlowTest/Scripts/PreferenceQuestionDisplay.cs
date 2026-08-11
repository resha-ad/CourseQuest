using UnityEngine;
using TMPro;

/// <summary>
/// Shared floating question-text display used by all three preference mechanics.
/// Deliberately shows only the statement text -- no numbers, no progress counter --
/// consistent with keeping the Preference zone free of explicit scoring cues.
/// </summary>
public class PreferenceQuestionDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;

    public void SetQuestion(string statement)
    {
        if (questionText != null)
            questionText.text = statement;
    }
}
