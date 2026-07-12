using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace GUIApp
{
    static public class readXML
    {

        static private string ResourcesPath = Directory.GetCurrentDirectory() + "/Resources/";
        static private string AppdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BOTWM";

        static public Dictionary<string, string>[] getServerList()
        {
            //XmlTextReader reader = new XmlTextReader(Directory.GetCurrentDirectory() + "/Resources/serverList.xml");
            //XmlTextReader reader = new XmlTextReader(Properties.Resources.serverLista);
            XmlTextReader reader = new XmlTextReader(AppdataFolder + "/serverList.xml");
            Dictionary<string, string>[] svList = new Dictionary<string, string>[1];
            Dictionary<string, string> serverRead = new Dictionary<string, string>();
            Dictionary<string, string> serverReadCopy;
            //serverRead.Add("Name", "");
            serverRead.Add("IP", "");
            serverRead.Add("PORT", "");
            serverRead.Add("PASSWORD", "");
            int counter = 0;
            string next = "";
            string[] acceptedStrings = { "IP", "PORT", "PASSWORD" };

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (acceptedStrings.Contains(reader.Name))
                        {
                            next = reader.Name;
                        }else
                        {
                            next = "";
                        }
                        break;

                    case XmlNodeType.Text:
                        if (next != "")
                        {
                            serverRead[next] = reader.Value;
                            if (next == "PORT")
                            {
                                serverReadCopy = new Dictionary<string, string>(serverRead);
                                Array.Resize(ref svList, counter + 1);
                                svList[counter] = serverReadCopy;
                                counter++;
                            }
                        }
                        break;
                }
            }

            reader.Close();

            return svList;

        }

        static public Dictionary<string, string>[] getServerList(int a)
        {

            Dictionary<string, string>[] svList = new Dictionary<string, string>[4];

            return svList;

        }

        static public Dictionary<string, Dictionary<string, string>> readAnimationFile()
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            XmlTextReader reader = new XmlTextReader(new StringReader(Properties.Resources.animationHashes));
            Dictionary<string, string> animation = new Dictionary<string, string>();
            Dictionary<string, string> animationCopy = new Dictionary<string, string>();
            string[] acceptedStrings = { "Hash", "Schedule", "Animation", "Name"};
            string next = "";
            string hash = "";

            animation.Add("Schedule", "");
            animation.Add("Animation", "");
            animation.Add("Name", "");

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (acceptedStrings.Contains(reader.Name))
                        {
                            next = reader.Name;
                        }
                        break;

                    case XmlNodeType.Text:
                        if (next != "")
                        {
                            if(next == "Hash")
                            {
                                hash = reader.Value;
                            }else
                            {
                                animation[next] = reader.Value;
                                if (next == "Animation")
                                {
                                    animationCopy = new Dictionary<string, string>(animation);
                                    result.Add(hash, animationCopy);
                                    next = "";
                                }
                            }
                        }
                        break;
                }
            }

            reader.Close();

            return result;
        }
        
        static public void addServerToList(Dictionary<string, string>[] currentServerList, string IP, string PORT, string PASSWORD)
        {
            using (XmlTextWriter writer = new XmlTextWriter(AppdataFolder + "serverList.xml", Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteStartElement("serverList");

                foreach(Dictionary<string, string> server in currentServerList)
                {
                    
                    writer.WriteStartElement("server");

                    foreach (string key in server.Keys)
                    {

                        writer.WriteStartElement(key);

                        writer.WriteString(server[key]);

                        writer.WriteEndElement();


                    }

                    writer.WriteEndElement();
                }

                writer.WriteStartElement("server");

                writer.WriteStartElement("IP");

                writer.WriteString(IP);

                writer.WriteEndElement();

                writer.WriteStartElement("PORT");

                writer.WriteString(PORT);

                writer.WriteEndElement();

                writer.WriteStartElement("PASSWORD");

                writer.WriteString(PASSWORD);

                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndElement();



            }

        }

        static public void copyQuestFlags()
        {

            //string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BOTWM";
            string fileName = "\\QuestFlags.txt";

            if (!Directory.Exists(AppdataFolder))
            {
                Directory.CreateDirectory(AppdataFolder);
            }

            TextWriter tw = new StreamWriter(AppdataFolder + fileName);
            tw.Write(Properties.Resources.QuestFlags);
            tw.Close();

            fileName = "\\QuestFlagsNames.txt";

            if (!Directory.Exists(AppdataFolder))
            {
                Directory.CreateDirectory(AppdataFolder);
            }

            tw = new StreamWriter(AppdataFolder + fileName);
            tw.Write(Properties.Resources.QuestFlagsNames);
            tw.Close();

        }


        static public void copyWeaponDamages()
        {

            string fileName = "\\WeaponDamages.txt";

            if (!Directory.Exists(AppdataFolder))
            {
                Directory.CreateDirectory(AppdataFolder);
            }

            TextWriter tw = new StreamWriter(AppdataFolder + fileName);
            tw.Write(Properties.Resources.WeaponDamages);
            tw.Close();

        }


        static public void copyServerList()
        {

            //string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BOTWM";
            string fileName = "\\serverList.xml";

            if (!Directory.Exists(AppdataFolder))
            {
                Directory.CreateDirectory(AppdataFolder);
            }else
            {
                if(File.Exists(AppdataFolder + fileName))
                {
                    return;
                }
            }

            TextWriter tw = new StreamWriter(AppdataFolder + fileName);
            tw.Write(Properties.Resources.serverList);
            tw.Close();

        }


        static public void copyArmorMappings()
        {

            //string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BOTWM";
            string fileName = "\\ArmorMapping.txt";

            if (!Directory.Exists(AppdataFolder))
            {
                Directory.CreateDirectory(AppdataFolder);
            }
            else
            {
                if (File.Exists(AppdataFolder + fileName))
                {
                    return;
                }
            }

            TextWriter tw = new StreamWriter(AppdataFolder + fileName);
            tw.Write(Properties.Resources.ArmorMapping);
            tw.Close();

        }


    }
}