using System;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{
    [Header("UI 참고")]
    public Text userNameText;
    public Text balanceText;
    public Text cashText;

    [Header("입출금UI 창")]
    public GameObject depositUI;
    public GameObject withdrawalUI;

    [Header("메인 버튼 창")]
    public GameObject mainButtonsGroup;

    [Header("UI버튼")]
    public Button depositButton;
    public Button withdrawButton;
    public Button backFromDepositButton;
    public Button backFromWithdrawButton;



    [Header("입금 버튼")]
    public Button deposit10kButton;
    public Button deposit30kButton;
    public Button deposit50kButton;
    public Button depositInputConfirmButton;
    public InputField depositInputField;

    [Header("출금 버튼")]
    public Button withdraw10kButton;
    public Button withdraw30kButton;
    public Button withdraw50kButton;
    public Button withdrawInputConfirmButton;
    public InputField withdrawInputField;

    [Header("에러 팝업")]
    public GameObject popupError;
    public Button errorPopupConfirmButton;

    private void Awake()
    {
        // 입금 UI 버튼
        depositButton.onClick.AddListener(OpenDepositUI);
        deposit10kButton.onClick.AddListener(() => DepositMoney(10000));
        deposit30kButton.onClick.AddListener(() => DepositMoney(30000));
        deposit50kButton.onClick.AddListener(() => DepositMoney(50000));
        depositInputConfirmButton.onClick.AddListener(DepositFromInput);

        // 출금 UI 버튼
        withdrawButton.onClick.AddListener(OpenWithdrawalUI);
        withdraw10kButton.onClick.AddListener(() => WithdrawMoney(10000));
        withdraw30kButton.onClick.AddListener(() => WithdrawMoney(30000));
        withdraw50kButton.onClick.AddListener(() => WithdrawMoney(50000));
        withdrawInputConfirmButton.onClick.AddListener(WithdrawFromInput);

        // 뒤로 가기 버튼 (입금/출금 공용)
        backFromDepositButton.onClick.AddListener(CloseAllPopup);
        backFromWithdrawButton.onClick.AddListener(CloseAllPopup);

        // 에러 팝업 닫기 버튼
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

    public void WithdrawMoney(int amount)
    {
        var data = GameManager.Instance.userData;

        if (data.balance >= amount)
        {
            data.balance -= amount;
            data.cash += amount;
            Refresh();
            depositInputField.text = ""; // 같은 필드 사용
        }
        else
        {
            popupError.SetActive(true);
        }
    }

    public void WithdrawFromInput()
    {
        int amount;
        if (int.TryParse(withdrawInputField.text, out amount))
        {
            WithdrawMoney(amount);
        }
        else
        {
            popupError.SetActive(true);
        }
    }
}
