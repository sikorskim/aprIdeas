using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace arpIdeas.Models
{
    public class XmlDb
    {
        public string Path { get; set; }
        public string RootElemName { get { return getRootElemName(); } }

        public XmlDb(string path)
        {
            Path = path;
            if (!checkFileExist(Path))
            {
                createXml(Path, RootElemName);
            }
        }

        bool checkFileExist(string path)
        {
            return File.Exists(path);
        }

        string getRootElemName()
        {
            return Path.Substring(0, Path.IndexOf('.'));
        }

        string getElemenstName()
        {
            return Path.Substring(0, Path.IndexOf('.') - 1);
        }

        public List<XElement> getElements()
        {
            XDocument doc = XDocument.Load(Path);
            return doc.Element(RootElemName).Elements(getElemenstName()).ToList();
        }

        void createXml(string path, string rootElemName)
        {
            XDocument doc = new XDocument();
            XElement root = new XElement(rootElemName);
            doc.Add(root);
            doc.Save(path);
        }

        public XElement getElement(string elemName, string attribute, string value)
        {
            XDocument doc = XDocument.Load(Path);
            XElement elem = doc.Element(RootElemName).Elements(elemName).Single(p => p.Attribute(attribute).Value == value);
            return elem;
        }

        public void addElement(XElement elem)
        {
            XDocument doc = XDocument.Load(Path);
            XElement rootElement = doc.Element(RootElemName);
            rootElement.Add(elem);
            doc.Save(Path);
        }

        public void editElement(XElement oldElem, XElement newElem)
        {
            XDocument doc = XDocument.Load(Path);
            XElement rootElement = doc.Element(RootElemName);

            oldElem = rootElement.Elements(oldElem.Name).Single(p => p.Attribute("Id").Value == oldElem.Attribute("Id").Value);

            foreach (XAttribute oldAtr in oldElem.Attributes())
            {
                var newAtr = newElem.Attributes().Single(p => p.Name == oldAtr.Name);
                oldAtr.SetValue(newAtr.Value);
            }

            doc.Save(Path);
        }

        public void deleteElement(XElement element)
        {
            XDocument doc = XDocument.Load(Path);
            XElement rootElement = doc.Element(RootElemName);
            element = rootElement.Elements(element.Name).Single(p=>p.Attribute("Id").Value == element.Attribute("Id").Value);
            element.Remove();
            doc.Save(Path);
        }

        public int getId()
        {
            XDocument doc = XDocument.Load(Path);
            XElement rootElement = doc.Element(RootElemName);
            return Int32.Parse(rootElement.Elements(getElemenstName()).Last().Attribute("Id").Value)+1;
        }
    }
}
