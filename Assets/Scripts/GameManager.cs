using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // 현재 사용자 데이터
    public UserData userData;
    // 저장 경로
    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 저장 경로 지정
            savePath = Path.Combine(Application.persistentDataPath, "userData.json");
            Debug.Log("저장위치: " + savePath);

            // 유저 데이터 불러오기
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
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(savePath, json);
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
        else
        {
            // 저장된 데이터가 없을 경우 → 기본값으로 생성 후 저장
            userData = new UserData("최홍진", 100000, 50000);
            SaveUserData();
        }
    }
}
