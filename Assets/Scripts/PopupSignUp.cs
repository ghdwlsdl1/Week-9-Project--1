using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PopupSignUp : MonoBehaviour
{
    [Header("입력 필드")]
    public InputField idInputField;               // 사용자 ID 입력 필드
    public InputField nameInputField;             // 사용자 이름 입력 필드
    public InputField passwordInputField;         // 비밀번호 입력 필드
    public InputField passwordConfirmInputField;  // 비밀번호 확인 입력 필드

    [Header("UI 창")]
    public GameObject popupLoginUI;   // 로그인 UI 오브젝트
    public GameObject popupSignUpUI;  // 회원가입 UI 오브젝트
    public GameObject popupError;     // 에러 팝업 UI 오브젝트

    [Header("버튼")]
    public Button signUpButton;            // 회원가입 버튼
    public Button cancelButton;            // 취소 버튼
    public Button errorPopupConfirmButton; // 에러 팝업 닫기 버튼

    [Header("오류 텍스트")]
    public Text errorText; // 에러 메시지 출력 텍스트

    private void Awake()
    {
        // 버튼 클릭 이벤트 등록
        signUpButton.onClick.AddListener(OnClickSignUp);
        cancelButton.onClick.AddListener(OnClickCancel);
        errorPopupConfirmButton.onClick.AddListener(() => popupError.SetActive(false));
    }

    // 회원가입 버튼 클릭 시 실행
    public void OnClickSignUp()
    {
        string id = idInputField.text;
        string name = nameInputField.text;
        string pw = passwordInputField.text;
        string pwConfirm = passwordConfirmInputField.text;

        // 입력값 검증
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(pw) || string.IsNullOrEmpty(pwConfirm))
        {
            ShowError("모든 항목을 입력해주세요.");
            return;
        }

        if (pw != pwConfirm)
        {
            ShowError("비밀번호가 일치하지 않습니다.");
            return;
        }

        // 경로 생성
        string path = Path.Combine(Application.persistentDataPath, id + ".json");

        // 중복 ID 체크
        if (File.Exists(path))
        {
            ShowError("이미 존재하는 ID입니다.");
            return;
        }

        // 새로운 유저 데이터 생성 및 저장
        UserData newUser = new UserData(id, pw, name, 100000, 50000);
        string json = JsonUtility.ToJson(newUser, true);
        File.WriteAllText(path, json);

        // 로그인 UI로 전환
        popupSignUpUI.SetActive(false);
        popupLoginUI.SetActive(true);
    }

    // 취소 버튼 클릭 시 로그인 화면으로 전환
    public void OnClickCancel()
    {
        popupSignUpUI.SetActive(false);
        popupLoginUI.SetActive(true);
    }

    // 에러 메시지 출력
    void ShowError(string message)
    {
        errorText.text = message;
        popupError.SetActive(true);
    }
}
