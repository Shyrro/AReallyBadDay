using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public class Answer {
    [DataMember]
    public string Label;
    [DataMember]
    public int NextQuestionId;
    [DataMember]
    public Dictionary<string, object> Required;
    [DataMember]
    public Dictionary<string, object> Change;
}