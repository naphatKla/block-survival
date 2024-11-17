using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : Singleton<SkillSystem>
{
    [SerializeField] public List<SkillBase> skills;
    [SerializeField] public GameObject playerHelperSkill;
    private Player _player;
    private Level _level;
    void Start()
    {
        _player = GetComponent<Player>();
        _level = GetComponent<Level>();
        
        // I don't know why this doesn't work
         foreach (SkillBase skill in skills)
         {
           skill.isCooldown = false;
         }
    }
    
    void Update()
    {
        if (Time.timeScale.Equals(0)) return;
        
        foreach (SkillBase skill in skills)
        {
            if (CheckSkillCondition(skill))
            {
                PlaySkill(skill);
            }
        }
        
        if(_level.playerLevel < 15) return;
        if(playerHelperSkill.activeSelf) return;
        playerHelperSkill.SetActive(true);
    }
    
    private bool CheckSkillCondition(SkillBase skill)
    {
        if (_level.playerLevel < skill.skillLevelUnlock) return false;
        if (skill.isCooldown) return false;
        if (Input.GetKeyDown(skill.skillKey)) return true;
        return false;
    }
    
    private void PlaySkill(SkillBase skill) 
    {
        Vector3 skillOffSet = _player.playerTransform.up * skill.skillOffset; 
        Instantiate(skill, _player.playerTransform.position + skillOffSet, _player.playerTransform.rotation); 
        StartCoroutine(SkillsCooldown(skill));
    }
    
    private IEnumerator SkillsCooldown(SkillBase skill) 
    {
        skill.isCooldown = true;
        skill.skillCurrentCooldown = skill.skillCooldown;
        
        while (skill.skillCurrentCooldown > 0)
        {
            skill.skillCurrentCooldown -= Time.deltaTime;
            skill.skillCurrentCooldown = Mathf.Clamp(skill.skillCurrentCooldown, 0, skill.skillCooldown);
            yield return null;
        }
        
        skill.isCooldown = false;
    } 
}
