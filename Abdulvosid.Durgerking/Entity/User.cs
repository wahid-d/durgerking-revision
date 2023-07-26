namespace Abdulvosid.Durgerking.Entity;

public class User
{
    public long Id { get; set; }
    public string Fullname { get; set; }
    public string Username { get; set; }
    public string Language { get; set; }
    public string Phone { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}