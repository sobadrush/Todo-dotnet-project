using System;
using System.Collections.Generic;

namespace Todo.Models;

public partial class EmpVO
{
    public int Empno { get; set; }

    public string? Ename { get; set; }

    public string? Job { get; set; }

    public DateTime? Hiredate { get; set; }

    public int Deptno { get; set; }

    public virtual DeptVO DeptnoNavigation { get; set; } = null!;
}
