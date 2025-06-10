using System;

[Serializable]
public class UserData
{
    public string id;
    public string password;
    public string userName;
    public int cash;
    public int balance;

    public UserData(string id, string password, string userName, int cash, int balance)
    {
        this.id = id;
        this.password = password;
        this.userName = userName;
        this.cash = cash;
        this.balance = balance;
    }
}
