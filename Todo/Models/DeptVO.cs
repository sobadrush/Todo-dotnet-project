using System;
using System.Collections.Generic;

namespace Todo.Models;

public partial class DeptVO
{
    public int Deptno { get; set; }

    public string? Dname { get; set; }

    public string? Loc { get; set; }

    public virtual ICollection<EmpVO> EmpVOs { get; } = new List<EmpVO>();

}
