using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PopupBank : MonoBehaviour
{
    [Header("UI 참고")]
    public Text userNameText;           // 유저 이름 텍스트
    public Text balanceText;            // 계좌 잔액 텍스트
    public Text cashText;               // 소지 현금 텍스트

    [Header("입금 출금 송금 UI 창")]
    public GameObject depositUI;        // 입금 UI
    public GameObject withdrawalUI;     // 출금 UI
    public GameObject remittanceUI;     // 송금 UI

    [Header("메인 버튼 창")]
    public GameObject mainButtonsGroup; // 메인 버튼

    [Header("UI 버튼")]
    public Button depositButton;            // 입금 열기 버튼
    public Button withdrawButton;           // 출금 열기 버튼
    public Button remittanceOpenButton;     // 송금 열기 버튼
    public Button backFromDepositButton;    // 입금 → 뒤로 버튼
    public Button backFromWithdrawButton;   // 출금 → 뒤로 버튼
    public Button backFromRemittanceButton; // 송금 → 뒤로 가기 버튼

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

    [Header("송금 UI")]
    public InputField remittanceTargetField;// 송금 대상 ID 입력 필드
    public InputField remittanceAmountField;// 송금 금액 입력 필드
    public Button remittanceButton;         // 송금 실행 버튼

    [Header("에러 팝업")]
    public GameObject popupError;           // 에러 팝업 창
    public Button errorPopupConfirmButton;  // 에러 확인 버튼
    public Text errorText;                  // 에러 텍스트


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

        // 송금 관련 버튼
        remittanceOpenButton.onClick.AddListener(OpenRemittanceUI);
        remittanceButton.onClick.AddListener(OnClickRemit);

        // 뒤로 가기 버튼
        backFromDepositButton.onClick.AddListener(CloseAllPopup);
        backFromWithdrawButton.onClick.AddListener(CloseAllPopup);
        backFromRemittanceButton.onClick.AddListener(CloseAllPopup);

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
        Debug.Log("불러온 이름: " + data.userName);

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
        remittanceUI.SetActive(false);
    }

    // 출금 UI 열기
    public void OpenWithdrawalUI()
    {
        withdrawalUI.SetActive(true);
        depositUI.SetActive(false);
        mainButtonsGroup.SetActive(false);
        remittanceUI.SetActive(false);
    }

    // 송금 UI 열기
    public void OpenRemittanceUI()
    {
        remittanceUI.SetActive(true);
        depositUI.SetActive(false);
        withdrawalUI.SetActive(false);
        mainButtonsGroup.SetActive(false);
    }

    // 모든 팝업 닫고 메인으로
    public void CloseAllPopup()
    {
        depositUI.SetActive(false);
        withdrawalUI.SetActive(false);
        remittanceUI.SetActive(false);
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
            ShowError("소지금이 부족합니다.");   // 에러 팝업
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
            ShowError("숫자를 정확히 입력해주세요."); // 숫자 아님
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
            ShowError("계좌에 잔액이 부족합니다."); // 에러 팝업
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
            ShowError("숫자를 정확히 입력해주세요."); // 숫자 아님
        }
    }

    // 에러 팝업 닫기
    public void CloseErrorPopup()
    {
        popupError.SetActive(false);
    }

    // 송금 버튼 클릭 시 호출되는 함수
    public void OnClickRemit()
    {
        string targetId = remittanceTargetField.text;     // 송금 대상 ID
        string amountText = remittanceAmountField.text;   // 송금 금액 문자열

        // 입력값 누락 확인
        if (string.IsNullOrEmpty(targetId) || string.IsNullOrEmpty(amountText))
        {
            ShowError("송금 대상과 금액을 입력해주세요.");
            return;
        }

        // 금액 숫자 유효성 확인
        if (!int.TryParse(amountText, out int amount) || amount <= 0)
        {
            ShowError("올바른 금액을 입력해주세요.");
            return;
        }
        
        // 자기 자신에게 송금시도
        if (targetId == GameManager.Instance.currentUserId)
        {
            ShowError("자기 자신에게는 송금할 수 없습니다.");
            return;
        }

        // 내 잔액 부족 확인
        if (GameManager.Instance.userData.balance < amount)
        {
            ShowError("잔액이 부족합니다.");
            return;
        }

        // 대상 ID의 저장 파일 확인
        string path = Path.Combine(Application.persistentDataPath, targetId + ".json");
        if (!File.Exists(path))
        {
            ShowError("해당 ID의 사용자가 존재하지 않습니다.");
            return;
        }

        // 대상 유저 데이터 불러오기
        string json = File.ReadAllText(path);
        UserData targetData = JsonUtility.FromJson<UserData>(json);

        // 송금 처리
        GameManager.Instance.userData.balance -= amount;
        targetData.balance += amount;

        // 대상 유저 데이터 저장
        File.WriteAllText(path, JsonUtility.ToJson(targetData, true));
        // 내 유저 데이터 저장
        GameManager.Instance.SaveUserData();

        // UI 갱신 및 입력창 초기화
        Refresh();
        remittanceTargetField.text = "";
        remittanceAmountField.text = "";
    }
    // 에러 팝업 텍스트
    private void ShowError(string message)
    {
        errorText.text = message;         // 오류 메시지 텍스트 설정
        popupError.SetActive(true);       // 에러 팝업 활성화
    }

}
