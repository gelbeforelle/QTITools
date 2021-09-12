using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Collections.Generic;

interface ITask{
    string ToLia();
}

abstract class Choice : ITask {
    public string[] answers {get;}
    public Choice(string[] Answers){
        answers = Answers;
    }
    public abstract string ToLia();
}

class SingleChoice : Choice {
    public int correctAnswer {get;}
     public SingleChoice(string[] Answers, int CorrectAnswer) : base(Answers){
        correctAnswer = CorrectAnswer;
    } 
    public override string ToLia()
    {
        string result = string.Empty;
        for(int i=0; i<answers.Length; i++){
            result += (correctAnswer==i)?"[(x)]":"[( )]";
            result += answers[i] + "\n";
        }
        return result;
    }
}
class MultipleChoice : Choice {
    public bool[] correctAnswers {get;}
     public MultipleChoice(string[] Answers, bool[] CorrectAnswers) : base(Answers){
        correctAnswers = CorrectAnswers;
    } 
    public override string ToLia()
    {
        string result = string.Empty;
        for(int i=0; i<answers.Length; i++){
            result += (correctAnswers[i])?"[[x]]":"[[ ]]";
            result += answers[i] + "\n";
        }
        return result;
    }
}

class TextQuestion : ITask {
    public string answer;
    public TextQuestion(string Answer){
        answer = Answer;
    }
    public string ToLia(){
        return "[[" + answer + "]]";
    }
}

class TaskFile : IEnumerable{
    public List<ITask> Tasks {get; private set;} = new List<ITask>();
    public void AddTask(ITask task){ if(task != null) Tasks.Add(task);}
    public string Text {get; private set;}
    public void AddText(string text) => Text += text + "\n";
    public IEnumerator GetEnumerator() {return !(Tasks is null)?Tasks.GetEnumerator():null;}
    
}