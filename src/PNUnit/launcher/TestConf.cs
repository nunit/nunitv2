using System;
using System.IO;
using System.Xml.Serialization;
using PNUnit.Framework;

namespace PNUnit.Launcher
{
    [Serializable]
    public class TestGroup
    {
        public ParallelTest[] ParallelTests;
    }

    [Serializable]
    public class ParallelTest
    {
        public string Name;
        public string[] Agents;
        public TestConf[] Tests;
    }

    [Serializable]
    public class TestConf
    {
        public string Name;
        public string Assembly;
        public string TestToRun;
        public string Machine;
        public string[] TestParams;
        public string StartBarrier = Names.ServerBarrier;
        public string EndBarrier = Names.EndBarrier;
        public string[] WaitBarriers;
    }

    public class TestConfLoader
    {
        public static TestGroup LoadFromFile(string file)
        {
            FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read);
            XmlSerializer ser = new XmlSerializer(typeof(TestGroup));
            return (TestGroup)ser.Deserialize(reader);
        }

        public static void WriteToFile(TestGroup group, string file)
        {
            FileStream writer = new FileStream(file, FileMode.Create, FileAccess.Write);
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(TestGroup));
                ser.Serialize(writer, group);
            }
            finally
            {
                writer.Close();
            }
        }
    }

}
