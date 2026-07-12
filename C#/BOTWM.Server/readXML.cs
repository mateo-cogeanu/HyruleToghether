using System.Reflection;
using System.Xml;
namespace BOTWM.Server
{
    static public class readXML
    {

        static public Dictionary<string, Dictionary<string, string>> readAnimationFile()
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            XmlTextReader reader = new XmlTextReader(new StringReader(Resources.animationHashes));
            Dictionary<string, string> animation = new Dictionary<string, string>();
            Dictionary<string, string> animationCopy = new Dictionary<string, string>();
            string[] acceptedStrings = { "Hash", "Schedule", "Animation", "Name" };
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
                            if (next == "Hash")
                            {
                                hash = reader.Value;
                            }
                            else
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

    }
}
