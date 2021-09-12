using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace QTITools
{
    class Program
    {
        static void Main(string[] args)
        {
            var file = new QTIParser("C:/Users/Niclas/Documents/QTI/test2.zip");
            foreach(ZipArchiveEntry entry in file){ 
                var test = file.ReadEntry(entry);
                if(file.ReadEntry(entry).Tasks != null) foreach(ITask t in file.ReadEntry(entry).Tasks){
                   if(entry != null) Console.WriteLine(t.ToLia());
                   else Console.WriteLine("NullEntry");
                }
                    else Console.WriteLine("NullTasks");
                }
            Console.WriteLine("Done.");
        }
    }
}
