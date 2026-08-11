using UnityEngine;

/// <summary>
/// Drives a single reused "stage" through a sequence of preference questions: shows the
/// statement text, has the avatar narrate it, activates the matching mechanic (only one
/// visible at a time, in the same spot each time), and advances automatically once the
/// player answers. No teleporting or navigation required between questions.
/// </summary>
public class PreferenceFlowController : MonoBehaviour
{
    public enum MechanicType { Orb, Crystal, Lever }

    [System.Serializable]
    public class FlowQuestion
    {
        public string statement;
        public MechanicType mechanic;
    }

    [SerializeField] private FlowQuestion[] questions;
    [SerializeField] private PreferenceQuestionDisplay questionDisplay;
    [SerializeField] private AvatarGuide avatarGuide;
    [SerializeField] private GameObject orbMechanicRoot;
    [SerializeField] private GameObject crystalMechanicRoot;
    [SerializeField] private GameObject leverMechanicRoot;
    [SerializeField] private OrbSelectionMechanic orbMechanic;
    [SerializeField] private CrystalLightingMechanic crystalMechanic;
    [SerializeField] private LeverMechanic leverMechanic;
    [SerializeField] private float delayBeforeAdvance = 1.4f; // lets the confirm effect/sound land before the next question loads

    private int currentIndex = -1;

    private void Start()
    {
        orbMechanic.OnAnswered += _ => OnQuestionAnswered();
        crystalMechanic.OnAnswered += _ => OnQuestionAnswered();
        leverMechanic.OnAnswered += _ => OnQuestionAnswered();

        ShowQuestion(0);
    }

    private void ShowQuestion(int index)
    {
        if (index >= questions.Length)
        {
            questionDisplay.SetQuestion("That's all for now -- thank you.");
            avatarGuide.Speak("That's all for now, thank you for trying these out.");
            orbMechanicRoot.SetActive(false);
            crystalMechanicRoot.SetActive(false);
            leverMechanicRoot.SetActive(false);
            return;
        }

        currentIndex = index;
        var q = questions[index];

        questionDisplay.SetQuestion(q.statement);
        avatarGuide.Speak(q.statement);

        orbMechanicRoot.SetActive(q.mechanic == MechanicType.Orb);
        crystalMechanicRoot.SetActive(q.mechanic == MechanicType.Crystal);
        leverMechanicRoot.SetActive(q.mechanic == MechanicType.Lever);

        if (q.mechanic == MechanicType.Orb) orbMechanic.ResetMechanic();
        if (q.mechanic == MechanicType.Crystal) crystalMechanic.ResetMechanic();
        if (q.mechanic == MechanicType.Lever) leverMechanic.ResetMechanic();
    }

    private void OnQuestionAnswered()
    {
        StartCoroutine(AdvanceAfterDelay());
    }

    private System.Collections.IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeAdvance);
        ShowQuestion(currentIndex + 1);
    }
}
