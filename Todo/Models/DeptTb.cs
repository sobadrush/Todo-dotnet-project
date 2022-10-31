using System;
using System.Collections.Generic;

namespace Todo.Models;

public partial class DeptTb
{
    public int Deptno { get; set; }

    public string? Dname { get; set; }

    public string? Loc { get; set; }

    public virtual ICollection<EmpTb> EmpTbs { get; } = new List<EmpTb>();

}
