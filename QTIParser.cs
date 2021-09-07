using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Xml;


partial class QTIParser : IEnumerable{
    public QTIParser(string path){
        Source = ZipFile.OpenRead(path);
        // ZipArchiveEntry entry = Source.GetEntry(bodyPath);
    }

    public string ReadEntry(ZipArchiveEntry entry){
        //if(path == blacklist) return null;
        //ZipArchiveEntry entry = Source.GetEntry(path);
        reader = XmlReader.Create(entry.Open());
        while(!(reader.NodeType == XmlNodeType.EndElement) || reader.Name == "p" || reader.Name == "simpleChoice"){
            reader.Read();
            //Console.WriteLine(reader.Name);
            if(XmlNodeType.Text == reader.NodeType) Console.WriteLine(reader.Value);
        }
        return null;
    }

    private XmlReader reader;
    private ZipArchive Source;
    public const string blacklist = "imsmanifest.xml";

    

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