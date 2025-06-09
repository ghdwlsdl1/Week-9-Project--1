using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{
    [Header("UI 참고")]
    public Text userNameText;
    public Text balanceText;
    public Text cashText;

    [Header("팝업 창")]
    public GameObject depositUI;
    public GameObject withdrawalUI;

    [Header("메인 버튼 그룹")]
    public GameObject mainButtonsGroup;

    [Header("버튼")]
    public Button depositButton;
    public Button withdrawButton;
    public Button backFromDepositButton;
    public Button backFromWithdrawButton;



    [Header("입금 버튼들")]
    public Button deposit10kButton;
    public Button deposit30kButton;
    public Button deposit50kButton;
    public Button depositInputConfirmButton;
    public Button errorPopupConfirmButton;

    [Header("입금 관련")]
    public InputField depositInputField;
    public GameObject popupError;

    private void Awake()
    {
        depositButton.onClick.AddListener(OpenDepositUI);
        withdrawButton.onClick.AddListener(OpenWithdrawalUI);
        backFromDepositButton.onClick.AddListener(CloseAllPopup);
        backFromWithdrawButton.onClick.AddListener(CloseAllPopup);

        deposit10kButton.onClick.AddListener(() => DepositMoney(10000));
        deposit30kButton.onClick.AddListener(() => DepositMoney(30000));
        deposit50kButton.onClick.AddListener(() => DepositMoney(50000));
        depositInputConfirmButton.onClick.AddListener(DepositFromInput);
        errorPopupConfirmButton.onClick.AddListener(CloseErrorPopup);
    }

    private void Start()
    {
        Refresh();
        CloseAllPopup();
    }

    public void Refresh()
    {
        var data = GameManager.Instance.userData;

        userNameText.text = data.userName;
        balanceText.text = string.Format("{0:N0}", data.balance);
        cashText.text = string.Format("{0:N0}", data.cash);
    }

    public void OpenDepositUI()
    {
        depositUI.SetActive(true);
        withdrawalUI.SetActive(false);
        mainButtonsGroup.SetActive(false);
    }

    public void OpenWithdrawalUI()
    {
        withdrawalUI.SetActive(true);
        depositUI.SetActive(false);
        mainButtonsGroup.SetActive(false);
    }

    public void CloseAllPopup()
    {
        depositUI.SetActive(false);
        withdrawalUI.SetActive(false);
        mainButtonsGroup.SetActive(true);
    }

    public void DepositMoney(int amount)
    {
        var data = GameManager.Instance.userData;

        if (data.cash >= amount)
        {
            data.cash -= amount;
            data.balance += amount;
            Refresh();
            depositInputField.text = ""; // 입력값 초기화
        }
        else
        {
            popupError.SetActive(true);
        }
    }

    public void DepositFromInput()
    {
        int amount;
        if (int.TryParse(depositInputField.text, out amount))
        {
            DepositMoney(amount);
        }
        else
        {
            popupError.SetActive(true);
        }
    }

    public void CloseErrorPopup()
    {
        popupError.SetActive(false);
    }
}
