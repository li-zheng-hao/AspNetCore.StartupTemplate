namespace AspNetCore.StartUpTemplate.Contract;

/// <summary>
/// DTM示例事务发送的对象
/// </summary>
public class TransRequest
{
    public TransRequest(int userId, int number)
    {
        UserId = userId;
        Number = number;
    }

    public int UserId { get; set; }
    public int Number{ get; set; }
}