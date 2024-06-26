﻿

using System.Data.Entity;

namespace TestNinja.Mocking
{
    public class EmployeeStorage : IEmployeeStorage
    {
        private EmployeeContext _db;

        public EmployeeStorage() 
        {
            _db = new EmployeeContext();
        }

        public void DeleteEmployee(int id)
        {
            var employee = _db.Employees.Find(id);

            if (employee == null) return;

            _db.Employees.Remove(employee);
            _db.SaveChanges();
        }
    }

    public interface IEmployeeStorage
    {
        void DeleteEmployee(int id);
    }

    public class EmployeeContext
    {
        public DbSet<Employee> Employees { get; set; }

        public void SaveChanges()
        {
        }
    }
}
