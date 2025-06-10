using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PopupLogin : MonoBehaviour
{
    [Header("패널 UI")]
    public GameObject loginPanel;      // 로그인 패널
    public GameObject signUpPanel;     // 회원가입 패널
    public GameObject popupError;      // 에러 팝업

    [Header("로그인 입력 필드")]
    public InputField loginIdField;       // 로그인 ID 입력 필드
    public InputField loginPasswordField; // 로그인 비밀번호 입력 필드

    [Header("오류 텍스트")]
    public Text errorText; // 에러 메시지를 출력할 텍스트

    [Header("UI 전환")]
    public GameObject popupLogin;  // 전체 로그인 팝업 오브젝트
    public GameObject popupBank;   // 로그인 성공 시 활성화할 은행 UI

    [Header("버튼들")]
    public Button loginButton;         // 로그인 버튼
    public Button signUpButton;        // 회원가입 화면 전환 버튼
    public Button errorConfirmButton;  // 에러 확인 버튼

    [Header("데이터")]
    public PopupBank popupBankScript; // PopupBank 스크립트 참조 (Refresh 호출용)

    private void Awake()
    {
        // 버튼 이벤트 등록
        loginButton.onClick.AddListener(OnClickLogin);
        signUpButton.onClick.AddListener(OnClickGoToSignUp);
        errorConfirmButton.onClick.AddListener(OnClickCloseError);
    }

    // 로그인 시도
    public void OnClickLogin()
    {
        string id = loginIdField.text;
        string pw = loginPasswordField.text;

        // 입력 유효성 검사
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            ShowError("아이디와 비밀번호를 입력해주세요.");
            return;
        }

        // JSON 파일 경로 설정
        string path = Path.Combine(Application.persistentDataPath, id + ".json");

        // 파일 존재 여부 확인
        if (!File.Exists(path))
        {
            ShowError("존재하지 않는 아이디입니다.");
            return;
        }

        // 파일에서 데이터 읽고 파싱
        string json = File.ReadAllText(path);
        UserData loadedData = JsonUtility.FromJson<UserData>(json);

        // 비밀번호 검증
        if (loadedData.password != pw)
        {
            ShowError("비밀번호가 틀렸습니다.");
            return;
        }

        // 로그인 성공 → GameManager에 정보 저장
        GameManager.Instance.currentUserId = id;
        GameManager.Instance.userData = loadedData;
        GameManager.Instance.SaveUserData(); // 로그인 직후 저장

        // UI 전환
        popupLogin.SetActive(false);
        popupBank.SetActive(true);

        popupBankScript.Refresh(); // UI 정보 갱신
    }

    // 회원가입 화면으로 이동
    public void OnClickGoToSignUp()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    // 에러 팝업 닫기
    public void OnClickCloseError()
    {
        popupError.SetActive(false);
    }

    // 에러 메시지 출력
    void ShowError(string message)
    {
        errorText.text = message;
        popupError.SetActive(true);
    }
}
