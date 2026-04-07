using UnityEngine;

public class SPBar : MonoBehaviour
{
    public Transform bar;

    private Skill skill;

    public void Init(Skill _skill)
    {
        skill = _skill;
        skill.OnSPChanged += UpdateBar;
        skill.OnActivated += UpdateBar;
        skill.OnDeactivated += UpdateBar;
        UpdateBar();
    }

    void Update()
    {
        // Длительность обновляется каждый кадр пока скилл активен
        if (skill != null && skill.isActive && skill.hasDuration)
            UpdateBar();
    }

    void UpdateBar()
    {
        if (skill == null) return;

        float percent;

        if (skill.isActive && skill.hasDuration)
        {
            // Показываем остаток длительности
            percent = skill.durationTimer / skill.duration;
        }
        else if (skill.isActive && !skill.hasDuration)
        {
            // Бесконечный скилл — полная шкала пока активен
            percent = 1f;
        }
        else
        {
            // Скилл не активен — показываем SP
            percent = skill.maxSP > 0 ? skill.currentSP / skill.maxSP : 0f;
        }

        bar.localScale = new Vector3(Mathf.Clamp01(percent), bar.localScale.y, 1f);
    }

    void OnDestroy()
    {
        if (skill != null)
        {
            skill.OnSPChanged -= UpdateBar;
            skill.OnActivated -= UpdateBar;
            skill.OnDeactivated -= UpdateBar;
        }
    }
}