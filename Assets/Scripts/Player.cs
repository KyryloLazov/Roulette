public class Player
{
    public string Name { get; private set; }
    public float Balance { get; set; }

    public Player(string name, float balance)
    {
        Name = name;
        Balance = balance;
    }
}