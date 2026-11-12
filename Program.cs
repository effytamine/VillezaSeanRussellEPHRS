using System;
using System.Collections.Generic;
using System.Linq;

public class Employee
{
    public int Id { get; set; }                  // Column 1
    public string FirstName { get; set; }        // Column 2
    public string LastName { get; set; }         // Column 3
    public string Department { get; set; }       // Column 4
    public string JobTitle { get; set; }         // Column 5
    public decimal MonthlySalary { get; set; }   // Column 6
    public double TaxRate { get; set; }          // Column 7
    public DateTime HireDate { get; set; }       // Column 8
    public bool IsActive { get; set; }           // Column 9
    public string Email { get; set; }            // Column 10
}

class Program
{
    // starts empty, 0 pre-defined records
    static List<Employee> employees = new List<Employee>();

    static int nextId = 1001;

    static void Main(string[] args)
    {
        bool running = true;

        while (running)
        {
            Console.Clear();

            Console.WriteLine("=== EPHRS (MAIN BRANCH) and (FEATURE BRANCH) test ===");

            Console.WriteLine("1. Create (Add Employee)");
            Console.WriteLine("2. Read (View All Employees)");
            Console.WriteLine("3. Search / Filter");
            Console.WriteLine("4. Update Employee");
            Console.WriteLine("5. Delete Employee");
            Console.WriteLine("6. Exit");
            Console.Write("\nSelect an option: ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    CreateEmployee();
                    break;
                case "2":
                    ReadEmployees();
                    break;
                case "3":
                    SearchEmployees();
                    break;
                case "4":
                    UpdateEmployee();
                    break;
                case "5":
                    DeleteEmployee();
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            if (running)
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }

    // prints a table of whatever list it is given, used by Read and Search
    static void PrintEmployees(List<Employee> list)
    {
        Console.WriteLine($"{"Id",-5} {"First",-10} {"Last",-10} {"Dept",-10} {"Job Title",-18} {"Salary",-10} {"Tax",-6} {"Hired",-11} {"Status",-8} {"Email",-26}");
        Console.WriteLine(new string('-', 124));

        foreach (Employee emp in list)
        {
            string status = (emp.IsActive ? "Active" : "Inactive");
            Console.WriteLine($"{emp.Id,-5} {emp.FirstName,-10} {emp.LastName,-10} {emp.Department,-10} {emp.JobTitle,-18} {emp.MonthlySalary,-10:F2} {emp.TaxRate,-6:P0} {emp.HireDate,-11:yyyy-MM-dd} {status,-8} {emp.Email,-26}");
        }
    }

    static void CreateEmployee()
    {
        Console.WriteLine("--- Add New Employee ---");

        Console.Write("Enter First Name: ");
        string firstName = Console.ReadLine();

        Console.Write("Enter Last Name: ");
        string lastName = Console.ReadLine();

        Console.Write("Enter Department: ");
        string department = Console.ReadLine();

        Console.Write("Enter Job Title: ");
        string jobTitle = Console.ReadLine();

        Console.Write("Enter Monthly Salary: ");
        decimal.TryParse(Console.ReadLine(), out decimal salary);

        Console.Write("Enter Tax Rate (e.g. 0.12 for 12%): ");
        double.TryParse(Console.ReadLine(), out double taxRate);

        Console.Write("Enter Hire Date (yyyy-mm-dd): ");
        bool validDate = DateTime.TryParse(Console.ReadLine(), out DateTime hireDate);
        if (!validDate)
        {
            // if the date typed is wrong, just use today
            hireDate = DateTime.Today;
        }

        Console.Write("Is the employee active? (y/n): ");
        string activeInput = Console.ReadLine();
        bool isActive = (activeInput.ToLower() == "y");

        Console.Write("Enter Email: ");
        string email = Console.ReadLine();

        Employee newEmp = new Employee();
        newEmp.Id = nextId;
        newEmp.FirstName = firstName;
        newEmp.LastName = lastName;
        newEmp.Department = department;
        newEmp.JobTitle = jobTitle;
        newEmp.MonthlySalary = salary;
        newEmp.TaxRate = taxRate;
        newEmp.HireDate = hireDate;
        newEmp.IsActive = isActive;
        newEmp.Email = email;

        // Console.WriteLine($"DEBUG: adding employee with id {newEmp.Id}");
        employees.Add(newEmp);
        nextId++;

        Console.WriteLine($"\nEmployee added successfully! (Id: {newEmp.Id})");
    }

    static void ReadEmployees()
    {
        Console.WriteLine("--- Employee Records ---");

        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        PrintEmployees(employees);

        // payroll summary, only counts active employees
        List<Employee> activeOnes = employees.Where(e => e.IsActive).ToList();
        decimal totalGross = activeOnes.Sum(e => e.MonthlySalary);
        decimal totalNet = activeOnes.Sum(e => e.MonthlySalary - (e.MonthlySalary * (decimal)e.TaxRate));

        Console.WriteLine();
        Console.WriteLine($"Total employees: {employees.Count}  (Active: {activeOnes.Count})");
        Console.WriteLine($"Total monthly gross (active): {totalGross:F2}");
        Console.WriteLine($"Total monthly net after tax (active): {totalNet:F2}");
    }

    static void SearchEmployees()
    {
        Console.WriteLine("--- Search / Filter ---");
        Console.WriteLine("1. By Name (first or last)");
        Console.WriteLine("2. By Department");
        Console.WriteLine("3. By Salary (minimum)");
        Console.WriteLine("4. By Status (active / inactive)");
        Console.Write("\nSelect a filter: ");
        string filter = Console.ReadLine();
        Console.WriteLine();

        List<Employee> results = new List<Employee>();

        if (filter == "1")
        {
            Console.Write("Enter name to search: ");
            string keyword = Console.ReadLine().ToLower();
            results = employees.Where(e => e.FirstName.ToLower().Contains(keyword) || e.LastName.ToLower().Contains(keyword)).ToList();
        }
        else if (filter == "2")
        {
            Console.Write("Enter department: ");
            string dept = Console.ReadLine().ToLower();
            results = employees.Where(e => e.Department.ToLower() == dept).ToList();
        }
        else if (filter == "3")
        {
            Console.Write("Enter minimum monthly salary: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal minSalary))
            {
                results = employees.Where(e => e.MonthlySalary >= minSalary).ToList();
            }
            else
            {
                Console.WriteLine("\nInvalid salary.");
                return;
            }
        }
        else if (filter == "4")
        {
            Console.Write("Show active employees? (y = active, n = inactive): ");
            bool wantActive = (Console.ReadLine().ToLower() == "y");
            results = employees.Where(e => e.IsActive == wantActive).ToList();
        }
        else
        {
            Console.WriteLine("Invalid filter.");
            return;
        }

        Console.WriteLine();
        if (results.Count == 0)
        {
            Console.WriteLine("No matching employees found.");
        }
        else
        {
            Console.WriteLine($"Found {results.Count} employee(s):\n");
            PrintEmployees(results);
        }
    }

    static void UpdateEmployee()
    {
        ReadEmployees();

        if (employees.Count == 0)
        {
            return;
        }

        Console.Write("\nEnter the ID of the employee to update: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            Employee emp = employees.FirstOrDefault(e => e.Id == id);
            if (emp != null)
            {
                Console.WriteLine("(press Enter on any field to keep the current value)\n");

                Console.Write($"Enter new first name (current: {emp.FirstName}): ");
                string newFirst = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newFirst)) emp.FirstName = newFirst;

                Console.Write($"Enter new last name (current: {emp.LastName}): ");
                string newLast = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newLast)) emp.LastName = newLast;

                Console.Write($"Enter new department (current: {emp.Department}): ");
                string newDept = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDept)) emp.Department = newDept;

                Console.Write($"Enter new job title (current: {emp.JobTitle}): ");
                string newJob = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newJob)) emp.JobTitle = newJob;

                Console.Write($"Enter new monthly salary (current: {emp.MonthlySalary:F2}): ");
                if (decimal.TryParse(Console.ReadLine(), out decimal newSalary))
                {
                    emp.MonthlySalary = newSalary;
                }

                Console.Write($"Enter new tax rate (current: {emp.TaxRate}): ");
                if (double.TryParse(Console.ReadLine(), out double newTax))
                {
                    emp.TaxRate = newTax;
                }

                Console.Write($"Enter new hire date (current: {emp.HireDate:yyyy-MM-dd}): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime newDate))
                {
                    emp.HireDate = newDate;
                }

                Console.Write($"Is the employee active? y/n (current: {(emp.IsActive ? "y" : "n")}): ");
                string newActive = Console.ReadLine().ToLower();
                if (newActive == "y")
                {
                    emp.IsActive = true;
                }
                else if (newActive == "n")
                {
                    emp.IsActive = false;
                }

                Console.Write($"Enter new email (current: {emp.Email}): ");
                string newEmail = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newEmail)) emp.Email = newEmail;

                Console.WriteLine("\nEmployee updated successfully!");
            }
            else
            {
                Console.WriteLine("\nEmployee not found.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid ID format.");
        }
    }

    static void DeleteEmployee()
    {
        ReadEmployees();

        if (employees.Count == 0)
        {
            return;
        }

        Console.Write("\nEnter the ID of the employee to delete: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            // RemoveAll returns how many it removed, 0 means the id wasn't there
            int removed = employees.RemoveAll(e => e.Id == id);
            // Console.WriteLine($"DEBUG: removed = {removed}");

            if (removed > 0)
            {
                Console.WriteLine("\nEmployee deleted successfully.");
            }
            else
            {
                Console.WriteLine("\nEmployee not found.");
            }
        }
        else
        {
            Console.WriteLine("\nInvalid ID format.");
        }
    }
}