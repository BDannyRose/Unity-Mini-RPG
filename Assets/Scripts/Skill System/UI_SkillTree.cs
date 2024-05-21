using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField] private GameObject skillTreeVisual;
    [SerializeField] private TMPro.TextMeshProUGUI skillPointsText;
    private List<SkillButton> skillButtons;

    protected EventBus eventBus;


    private void Start()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();

        skillButtons = new List<SkillButton>();
        SetPlayerSKills(PlayerController.Instance.playerSkills);
        skillTreeVisual.SetActive(false);

        eventBus.Subscribe<E_OnSkillPointsChanged>(UpdateSkillPointsText);
    }

    public void PlayerSkills_OnSkillUnlocked(E_OnSkillUnlocked onSkillUnlocked)
    {
        UpdateVisuals();
    }

    public void SetPlayerSKills(SkillSystem playerSkills)
    {
        eventBus.Subscribe<E_OnSkillUnlocked>(PlayerSkills_OnSkillUnlocked);

        skillButtons = new List<SkillButton>();
        UI_SkillButton[] ui_SkillButtons = FindObjectsOfType<UI_SkillButton>();
        if (ui_SkillButtons.Length > 0)
        {
            foreach (UI_SkillButton button in ui_SkillButtons)
            {
                skillButtons.Add(new SkillButton(button.transform, playerSkills, button.skillType));
            }
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (skillButtons.Count < 1) return;

        foreach ( SkillButton button in skillButtons )
        {
            button.UpdateVisual();
        }
    }

    private class SkillButton
    {
        private Transform transform;
        private Image image;
        private SkillSystem playerSkills;
        private SkillSystem.SkillType skillType;

        public SkillButton(Transform transform, SkillSystem playerSkills, SkillSystem.SkillType skillType)
        {
            this.transform = transform;
            this.image = transform.GetComponent<Image>();
            this.playerSkills = playerSkills;
            this.skillType = skillType;

            this.transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                string warning;
                if (!playerSkills.TryUnlockSkill(skillType, out warning))
                {
                    TooltipWarning.ShowTooltip_Static(() => warning, 2f);
                }
            });
        }

        public void UpdateVisual()
        {
            if (playerSkills.IsSkillUnlocked(skillType))
            {
                image.color = Color.green;
            }
            else
            {
                if (playerSkills.CanUnlock(skillType))
                {
                    image.color = Color.red;
                }
                else
                {
                    image.color = Color.gray;
                }
            }
        }
    }

    public void OnSkillTreeButtonPressed(InputAction.CallbackContext context)
    {
        skillTreeVisual.SetActive(!skillTreeVisual.activeSelf);
        UpdateVisuals();
    }

    private void UpdateSkillPointsText(E_OnSkillPointsChanged onSkillPointsChanged)
    {
        skillPointsText.text = "Skill points: " + onSkillPointsChanged.skillPoints.ToString();
        UpdateVisuals();
    }
}
