using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // 현재 사용자 데이터
    public UserData userData;
    // 저장 경로
    private string savePath;
    public string currentUserId; // 현재 로그인한 사용자 ID (파일 저장 시 사용)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 저장 경로 지정
            savePath = Path.Combine(Application.persistentDataPath, "userData.json");
            Debug.Log("저장위치: " + savePath);

            // 유저 데이터 불러오기 (현재 사용하지 않음, 로그인 시 개별 유저 파일을 불러오므로)
            LoadUserData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 유저 데이터 저장
    public void SaveUserData()
    {
        if (string.IsNullOrEmpty(currentUserId)) // 유저 ID가 없는 경우 저장 불가
        {
            Debug.LogWarning("저장 실패: currentUserId가 비어 있습니다.");
            return;
        }

        string path = Path.Combine(Application.persistentDataPath, currentUserId + ".json");
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(path, json);
    }

    // 유저 데이터 불러오기
    public void LoadUserData()
    {
        if (File.Exists(savePath))
        {
            // 저장된 json 파일이 있을 경우 → 불러오기
            string json = File.ReadAllText(savePath);
            userData = JsonUtility.FromJson<UserData>(json);
        }
        // else
        // {
        //     // 저장된 데이터가 없을 경우 → 기본값으로 생성 후 저장
        //     userData = new UserData("최홍진", 100000, 50000);
        //     SaveUserData();
        // }
    }
}
