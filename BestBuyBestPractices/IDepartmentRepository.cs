using System;
using System.Collections.Generic;
using System.Text;

namespace BestBuyBestPractices
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetAllDepartments();
        
        /* 
         Added InsertDepartment to interface even though it was not in the instructions.
         Program works either way, of course.
        */
        void InsertDepartment(string departmentName);
    }
}
