[System.Serializable]
public class Statement {
    public int Id;
    public string Question;
    public Answer[] Answers;
    public bool Failure;
    public bool Success;
    public string Image;
}