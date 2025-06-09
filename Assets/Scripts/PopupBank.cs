using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{
    [Header("UI 참고")]
    public Text userNameText;           // 유저 이름 텍스트
    public Text balanceText;            // 계좌 잔액 텍스트
    public Text cashText;               // 소지 현금 텍스트

    [Header("입출금 UI 창")]
    public GameObject depositUI;        // 입금 UI
    public GameObject withdrawalUI;     // 출금 UI

    [Header("메인 버튼 창")]
    public GameObject mainButtonsGroup; // 메인 버튼

    [Header("UI 버튼")]
    public Button depositButton;            // 입금 열기 버튼
    public Button withdrawButton;           // 출금 열기 버튼
    public Button backFromDepositButton;    // 입금 → 뒤로 버튼
    public Button backFromWithdrawButton;   // 출금 → 뒤로 버튼

    [Header("입금 버튼")]
    public Button deposit10kButton;         // 1만원 입금
    public Button deposit30kButton;         // 3만원 입금
    public Button deposit50kButton;         // 5만원 입금
    public Button depositInputConfirmButton;// 직접 입력 입금 버튼
    public InputField depositInputField;    // 입금 금액 입력창

    [Header("출금 버튼")]
    public Button withdraw10kButton;        // 1만원 출금
    public Button withdraw30kButton;        // 3만원 출금
    public Button withdraw50kButton;        // 5만원 출금
    public Button withdrawInputConfirmButton;// 직접 입력 출금 버튼
    public InputField withdrawInputField;   // 출금 금액 입력창

    [Header("에러 팝업")]
    public GameObject popupError;           // 에러 팝업 창
    public Button errorPopupConfirmButton;  // 에러 확인 버튼

    // 버튼 이벤트 연결
    private void Awake()
    {
        // 입금 관련 버튼
        depositButton.onClick.AddListener(OpenDepositUI);
        deposit10kButton.onClick.AddListener(() => DepositMoney(10000));
        deposit30kButton.onClick.AddListener(() => DepositMoney(30000));
        deposit50kButton.onClick.AddListener(() => DepositMoney(50000));
        depositInputConfirmButton.onClick.AddListener(DepositFromInput);

        // 출금 관련 버튼
        withdrawButton.onClick.AddListener(OpenWithdrawalUI);
        withdraw10kButton.onClick.AddListener(() => WithdrawMoney(10000));
        withdraw30kButton.onClick.AddListener(() => WithdrawMoney(30000));
        withdraw50kButton.onClick.AddListener(() => WithdrawMoney(50000));
        withdrawInputConfirmButton.onClick.AddListener(WithdrawFromInput);

        // 뒤로 가기 버튼
        backFromDepositButton.onClick.AddListener(CloseAllPopup);
        backFromWithdrawButton.onClick.AddListener(CloseAllPopup);

        // 에러 팝업 닫기
        errorPopupConfirmButton.onClick.AddListener(CloseErrorPopup);
    }

    // 게임 시작 시 UI 초기화
    private void Start()
    {
        Refresh();         // 데이터 갱신
        CloseAllPopup();   // 모든 팝업 비활성화
    }

    // UI 텍스트 새로고침
    public void Refresh()
    {
        var data = GameManager.Instance.userData;

        userNameText.text = data.userName;
        balanceText.text = string.Format("{0:N0}", data.balance);
        cashText.text = string.Format("{0:N0}", data.cash);
    }

    // 입금 UI 열기
    public void OpenDepositUI()
    {
        depositUI.SetActive(true);
        withdrawalUI.SetActive(false);
        mainButtonsGroup.SetActive(false);
    }

    // 출금 UI 열기
    public void OpenWithdrawalUI()
    {
        withdrawalUI.SetActive(true);
        depositUI.SetActive(false);
        mainButtonsGroup.SetActive(false);
    }

    // 모든 팝업 닫고 메인으로
    public void CloseAllPopup()
    {
        depositUI.SetActive(false);
        withdrawalUI.SetActive(false);
        mainButtonsGroup.SetActive(true);
    }

    // 입금 처리
    public void DepositMoney(int amount)
    {
        var data = GameManager.Instance.userData;

        if (data.cash >= amount)
        {
            data.cash -= amount;
            data.balance += amount;

            GameManager.Instance.SaveUserData(); // 저장
            Refresh();
            depositInputField.text = "";         // 입력값 초기화
        }
        else
        {
            popupError.SetActive(true);          // 에러 팝업
        }
    }

    // 입금 (직접 입력)
    public void DepositFromInput()
    {
        int amount;
        if (int.TryParse(depositInputField.text, out amount))
        {
            DepositMoney(amount);
        }
        else
        {
            popupError.SetActive(true);          // 숫자 아님
        }
    }

    // 출금 처리
    public void WithdrawMoney(int amount)
    {
        var data = GameManager.Instance.userData;

        if (data.balance >= amount)
        {
            data.balance -= amount;
            data.cash += amount;

            GameManager.Instance.SaveUserData(); // 저장
            Refresh();
            withdrawInputField.text = "";        // 입력값 초기화
        }
        else
        {
            popupError.SetActive(true);          // 에러 팝업
        }
    }

    // 출금 (직접 입력)
    public void WithdrawFromInput()
    {
        int amount;
        if (int.TryParse(withdrawInputField.text, out amount))
        {
            WithdrawMoney(amount);
        }
        else
        {
            popupError.SetActive(true);          // 숫자 아님
        }
    }

    // 에러 팝업 닫기
    public void CloseErrorPopup()
    {
        popupError.SetActive(false);
    }
}
