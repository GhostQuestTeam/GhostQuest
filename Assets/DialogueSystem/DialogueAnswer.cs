using System.Collections;
using System.Collections.Generic;

public class DialogueAnswer {
    string message;
    int    next;

    public string Message
    {
        get
        {
            return message;
        }
    }

    public int Next
    {
        get
        {
            return next;
        }
    }

    public DialogueAnswer(string message, int next)
    {
        this.message = message;
        this.next = next;
    }

}
