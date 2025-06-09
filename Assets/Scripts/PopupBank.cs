using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{
    [Header("UI 참고")]
    public Text userNameText;
    public Text balanceText;
    public Text cashText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        var data = GameManager.Instance.userData;

        // 유저 이름
        userNameText.text = data.userName;

        // 금액 포맷팅 (천 단위 , 추가)
        balanceText.text = string.Format("{0:N0}", data.balance);
        cashText.text = string.Format("{0:N0}", data.cash);
    }
}
