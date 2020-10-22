using System.Runtime.Serialization;

[DataContract]
public class Statements {
    [DataMember]
    public Statement[] Questions;
}