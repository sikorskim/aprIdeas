using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace arpIdeas.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [DisplayName("Imię")]
        public string FirstName { get; set; }
        [DisplayName("Nazwisko")]
        public string LastName { get; set; }
        public int PositionId { get; set; }
        [DisplayName("Stanowisko")]
        public Position Position { get { return getPosition(); } }
        [DisplayName("Wynagrodzenie")]
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public string NIP { get; set; }
        [DisplayName("Wiek")]
        public int Age { get { return getAge(); } }
        [DisplayName("Data urodzenia")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        string xmlPath = "Employees.xml";

        int getAge()
        {
            int age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateOfBirth.Month > DateTime.Now.Month)
            {
                age--;
            }
            else if (DateOfBirth.Month==DateTime.Now.Month)
            {
                if (DateOfBirth.Day > DateTime.Now.Day)
                {
                    age--;
                }
            }
            return age;
        }

        Position getPosition()
        {
            Position position = new Position();
            return position.getPosition(PositionId);
        }

        public int getId()
        {
            XmlDb xmlDb = new XmlDb(xmlPath);
            return xmlDb.getId();
        }

        public List<Employee> getEmployees()
        {
            XmlDb xmlDb = new XmlDb(xmlPath);
            List<XElement> xElems = xmlDb.getElements();
            List<Employee> employeesList = new List<Employee>();

            foreach (XElement xElem in xElems)
            {
                Employee emp = getEmployeeFromXElem(xElem);
                employeesList.Add(emp);
            }

            return employeesList;
        }

        public Employee getEmployee(int id)
        {
            return getEmployees().Single(p=>p.Id==id);
        }

        Employee getEmployeeFromXElem(XElement xElem)
        {
            Employee emp = new Employee();
            emp.Id = Int32.Parse(xElem.Attribute("Id").Value);
            emp.FirstName = xElem.Attribute("FirstName").Value;
            emp.LastName = xElem.Attribute("LastName").Value;
            emp.NIP = xElem.Attribute("NIP").Value;
            emp.Salary = Decimal.Parse(xElem.Attribute("Salary").Value);
            emp.DateOfBirth = DateTime.Parse(xElem.Attribute("DateOfBirth").Value);
            emp.PositionId= Int32.Parse(xElem.Attribute("PositionId").Value);
            return emp;
        }

        XElement getXElementFromEmployee()
        {
            XElement empElem = new XElement("Employee");
            XAttribute id = new XAttribute("Id", Id);
            XAttribute firstName = new XAttribute("FirstName", FirstName);
            XAttribute lastName = new XAttribute("LastName", LastName);
            XAttribute nip = new XAttribute("NIP", NIP);
            XAttribute salary = new XAttribute("Salary", Salary);
            XAttribute dateOfBirth = new XAttribute("DateOfBirth", DateOfBirth);
            XAttribute positionId = new XAttribute("PositionId", PositionId);

            salary.Value=salary.Value.Replace('.', ',');
            empElem.Add(id, firstName, lastName, nip, salary, positionId, dateOfBirth);
            return empElem;
        }

        public void add()
        {
            XElement empElem = getXElementFromEmployee();            
            XmlDb xmlDb = new XmlDb(xmlPath);
            xmlDb.addElement(empElem);
        }

        public void edit()
        {
            XmlDb xmlDb = new XmlDb(xmlPath);
            XElement employeeBeforeEdit = xmlDb.getElement("Employee", "Id", Id.ToString());
            XElement employeeAfterEdit = getXElementFromEmployee();
            xmlDb.editElement(employeeBeforeEdit, employeeAfterEdit);
        }

        public void delete()
        {
            XmlDb xmlDb = new XmlDb(xmlPath);
            XElement employeeToDelete = getXElementFromEmployee();            
            xmlDb.deleteElement(employeeToDelete);
        }
    }
}
