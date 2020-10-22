using System.Runtime.Serialization;

[DataContract]
public class Statement {
    [DataMember]
    public int Id;
    [DataMember]
    public string Question;
    [DataMember]
    public Answer[] Answers;
    [DataMember]
    public bool Failure;
    [DataMember]
    public bool Success;
    [DataMember]
    public string Image;
}