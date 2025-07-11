using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public Vector2 moveDir;
    public bool jumped;
    public bool holdingJumpBtn;
    public bool attacked;
    public bool holdingAttackBtn { get; private set; }
    public bool getAttackedBtnUp { get; private set; }
    [SerializeField] private float holdingAttackBtnTimer = 0f;
    public bool holdingMoveDownBtn { get; private set; }
    public bool dashed;
    public bool parried;
    public bool usedSkill;
    public bool healed;
    private void Awake() 
    {
        if(Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
        jumped = false;
        holdingJumpBtn = false;
        attacked = false;
        dashed = false;
        parried = false;
        usedSkill = false;
        healed = false;
    }
    private void Update()
    {
        if (holdingAttackBtn || Input.GetKey(KeyCode.K))
            holdingAttackBtnTimer += Time.deltaTime;
        else
            holdingAttackBtnTimer = 0f;
    }
    public bool CheckCanChargeAtk()
    {
        return holdingAttackBtnTimer >= .2f;
    }
    public bool CheckCanDashAtk()
    {
        return holdingAttackBtnTimer >= .1f;
    }
    private void LateUpdate()
    {
        if(attacked)
            attacked = false;
        if(jumped && !Player.Instance.canLadder)
        {
            jumped = false;
        }
        if(dashed)
            dashed = false;
        if(parried)
            parried = false;
        if(usedSkill)
            usedSkill = false;
        if(healed)
            healed = false;
        if(getAttackedBtnUp)
            getAttackedBtnUp = false;
    }
    public void StartPressMoveLeft()
    {
        moveDir.x = -1f;
        //mobileInputTesting.mobileInputText.text = "Move Left";
    }
    public void StartPressMoveRight()
    {
        moveDir.x = 1f;
        //mobileInputTesting.mobileInputText.text = "Move Right";
    }
    public void GetHorizontalMoveButtonUp()
    {
        moveDir.x = 0;
        //mobileInputTesting.mobileInputText.text = "Stop Move";
    }
    public void GetVerticalMoveButtonUp()
    {
        moveDir.y = 0;
        //mobileInputTesting.mobileInputText.text = "Stand Up";
    }
    public void StartPressMoveDown()
    {
        moveDir.y = -1f;
        holdingMoveDownBtn = true;
        //mobileInputTesting.mobileInputText.text = "Sit down";
    }
    public void GetMoveDownBtnUp()
    {
        holdingMoveDownBtn = false;
    }
    public void StartPressJump()
    {
        jumped = true;
        holdingJumpBtn = true;
        //mobileInputTesting.mobileInputText.text = "Jump";
    }
    public void HoldJumpForMoveUpOnLadder()
    {
        if(Player.Instance.canLadder)
        {
            jumped = true;
        }
    }
    public void ReleaseHoldingJumpBtn() { holdingJumpBtn = false; }
    public void SetJumpFalse()
    {
        jumped = false;
        //mobileInputTesting.mobileInputText.text = "Not Jump";
    }
    public void StartPressAttack()
    {
        attacked = true;
        holdingAttackBtn = true;
    }
    public void GetAttackBtnUp()
    {
        holdingAttackBtn = false;
        getAttackedBtnUp = true;
    }
    public void StartPressDash()
    {
        dashed = true;
    }
    public void StartPressParry()
    {
        parried = true;
        //mobileInputTesting.mobileInputText.text = "Parry";
    }
    public void StartPressUseSkill()
    {
        usedSkill = true;
        //mobileInputTesting.mobileInputText.text = "Use Magic Skill";
    }
    public void StartPressHeal()
    {
        healed = true;
        //mobileInputTesting.mobileInputText.text = "Healing";
    }
    #region MoveLeft button effect
    public void PressMoveLeftButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetMoveLeftButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressMoveLeftButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
    }
    public void GetMoveLeftButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion
    #region MoveRight button effect
    public void PressMoveRightButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetMoveRightButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressMoveRightButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
    }
    public void GetMoveRightButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion
    #region MoveDown button effect
    public void PressMoveDownButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetMoveDownButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressMoveDownButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
    }
    public void GetMoveDownButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion
    #region Parry button effect
    public void PressParryButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetParryButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressParryButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
        buttonImage.transform.parent.GetComponent<Animator>().SetTrigger("Press");
    }
    public void GetParryButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion
    #region Dash button effect
    public void PressDashButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetDashButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressDashButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
        buttonImage.transform.parent.GetComponent<Animator>().SetTrigger("Press");
    }
    public void GetDashButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion
    #region Attack button effect
    public void PressAttackButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetAttackButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressAttackButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
        buttonImage.transform.parent.GetComponent<Animator>().SetTrigger("Press");
    }
    public void GetAttackButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion effect
    #region Jump button effect
    public void PressJumpButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetJumpButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressJumpButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
        buttonImage.transform.parent.GetComponent<Animator>().SetTrigger("Press");
    }
    public void GetJumpButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion effect
    #region Heal button effect
    public void PressHealButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetHealButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressHealButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
        buttonImage.transform.parent.GetComponent<Animator>().SetTrigger("Press");
    }
    public void GetHealButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion
    #region Skill button effect
    public void PressSkillButton_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 1f);
    }
    public void GetSkillButtonUp_Border(Image borderImage)
    {
        borderImage.color = new Color(borderImage.color.r, borderImage.color.g, borderImage.color.b, 130f / 255f);
    }
    public void PressSkillButton_Main(Image buttonImage)
    {
        buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
        buttonImage.transform.parent.GetComponent<Animator>().SetTrigger("Press");
    }
    public void GetSkillButtonUp_Main(Image mainImage)
    {
        mainImage.color = new Color(mainImage.color.r, mainImage.color.g, mainImage.color.b, 130f / 255f);
    }
    #endregion
}
