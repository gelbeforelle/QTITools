using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Collections.Generic;

partial class QTIParser : IEnumerable{
    public QTIParser(string path){
        Source = ZipFile.OpenRead(path);
        // ZipArchiveEntry entry = Source.GetEntry(bodyPath);
    }

    public TaskFile ReadEntry(ZipArchiveEntry entry){
        //if(path == blacklist) return null;
        //ZipArchiveEntry entry = Source.GetEntry(path);
        var result = new TaskFile();
        reader = XmlReader.Create(entry.Open());
        var corrects = new List<string>();
        //var responses = new Dictionary<string, string[]>();
        while(!(reader.NodeType == XmlNodeType.EndElement) || reader.Name == "p" || reader.Name == "simpleChoice"){
            reader.Read();
            if(reader.Name == "responseDeclaration" && reader.NodeType == XmlNodeType.Element){
                //var key = reader.GetAttribute(0); //Should use pattern-maching!!!
                
                while(!(reader.Name == "responseDeclaration" && reader.NodeType == XmlNodeType.EndElement || reader.Name == "itemBody")){
                    reader.Read();
                    if(reader.NodeType == XmlNodeType.Text) corrects.Add("identifier=\""+reader.Value+"\"");
                }
                
                //responses.Add(key, content.ToArray());    
            }
            //Console.WriteLine(reader.Name);
            if(reader.Name == "assessmentItem"){
                
                while(reader.Name == "p" || reader.NodeType == XmlNodeType.Text){
                    reader.Read();
                    if(reader.NodeType == XmlNodeType.Text) result.AddText(reader.Value); 
                }
            } // ???
                if(reader.Name == "choiceInteraction"){
                    bool isMC = true;
                    List<string> answers = new List<string>();
                    List<bool> answerKey = new List<bool>();
                    for(int i=0;i<reader.AttributeCount;i++){
                        if(reader.GetAttribute(i) == "maxChoices=\"1\"") isMC = false;
                    }

                    while(!(reader.NodeType == XmlNodeType.EndElement && reader.Name == "choiceInteraction")){
                        string choice = string.Empty;
                        while(!(reader.NodeType == XmlNodeType.EndElement && reader.Name == "simpleChoice")){
                            if(reader.Name == "simpleChoice"){
                                if(corrects.Contains(reader.GetAttribute(0))) answerKey.Add(true);
                                    else answerKey.Add(false);
                            }
                            reader.Read();
                            if(reader.NodeType == XmlNodeType.Text) choice += reader.Value + "\n";
                        }
                        answers.Add(choice);
                        reader.Read();
                    }
                    if(isMC) result.AddTask(new MultipleChoice(answers.ToArray(), answerKey.ToArray()));
                        else{
                            int correctIndex = 0;
                            correctIndex = answerKey.FindIndex(x => x);
                            result.AddTask(new SingleChoice(answers.ToArray(), correctIndex));
                        }
                    
                }
            //}
           // if(XmlNodeType.Text == reader.NodeType) Console.WriteLine(reader.Value);
          //  Console.WriteLine(reader.Name);
        }
        return result;
    }

    private XmlReader reader;
    private ZipArchive Source;
    //public const string blacklist = "imsmanifest.xml";

    

    public IEnumerator GetEnumerator(){
        return Source.Entries.GetEnumerator();
        }

    /* IEnumerator IEnumerable.GetEnumerator()
    {
       return (IEnumerator)GetEnumerator();
    } */

    
    

}

/* class ArchiveEnum : IEnumerator{
        public ZipArchiveEntry[] content;
        private int enumPos = -1;
        public void Reset() => enumPos = -1;
        public bool MoveNext() => (++enumPos <  content.Length);
        public object Current{get => content[enumPos];}

        public ArchiveEnum(ZipArchiveEntry[] Content) => content = Content;


} */