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

    private void Start()
    {
        Refresh();
        CloseAllPopup();

        depositButton.onClick.AddListener(OpenDepositUI);
        withdrawButton.onClick.AddListener(OpenWithdrawalUI);
        backFromDepositButton.onClick.AddListener(CloseAllPopup);
        backFromWithdrawButton.onClick.AddListener(CloseAllPopup);
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
}
