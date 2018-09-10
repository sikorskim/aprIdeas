using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace arpIdeas.Models
{
    public class Position
    {
        public int Id { get; set; }
        [DisplayName("Nazwa stanowiska")]
        public string Name { get; set; }

        string xmlPath = "Positions.xml";

        public List<Position> getPositions()
        {
            XmlDb xmlDb = new XmlDb(xmlPath);
            List<XElement> xElems = xmlDb.getElements();
            List<Position> positionsList = new List<Position>();

            foreach (XElement xElem in xElems)
            {
                Position pos = getPositionFromXElem(xElem);
                positionsList.Add(pos);
            }

            return positionsList;
        }

        public Position getPosition(int id)
        {
            return getPositions().Single(p => p.Id == id);
        }

        public int getId()
        {
            XmlDb xmlDb = new XmlDb(xmlPath);
            return xmlDb.getId();
        }

        Position getPositionFromXElem(XElement xElem)
        {
            Position pos = new Position();
            pos.Id = Int32.Parse(xElem.Attribute("Id").Value);
            pos.Name = xElem.Attribute("Name").Value;
            return pos;
        }

        XElement getXElementFromPosition()
        {
            XElement posElem = new XElement("Position");
            XAttribute id = new XAttribute("Id", Id);
            XAttribute name = new XAttribute("Name", Name);

            posElem.Add(id, name);
            return posElem;
        }

        public void add()
        {
            XElement posElem = new XElement("Position");
            XAttribute id = new XAttribute("Id", Id);
            XAttribute name = new XAttribute("Name", Name);

            posElem.Add(id, name);
            XmlDb xmlDb = new XmlDb(xmlPath);
            xmlDb.addElement(posElem);
        }

        public void edit()
        {
            XmlDb xmlDb = new XmlDb(xmlPath);
            XElement positionBeforeEdit = xmlDb.getElement("Position", "Id", Id.ToString());
            XElement positionAfterEdit = getXElementFromPosition();
            xmlDb.editElement(positionBeforeEdit, positionAfterEdit);
        }

        public void delete()
        {
            XmlDb xmlDb = new XmlDb(xmlPath);
            XElement positionToDelete = getXElementFromPosition();
        }

        // returns true if employee with given positionId exist 
        public bool checkConstraints()
        {
            Employee employee = new Employee();
            List<Employee> employees = employee.getEmployees();

            if (employees.Exists(p => p.PositionId == Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
