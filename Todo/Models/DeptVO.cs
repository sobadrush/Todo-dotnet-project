using Newtonsoft.Json;

namespace Todo.Models;

public partial class DeptVO
{
    public DeptVO()
    {
    }

    public DeptVO(string dname, string loc)
    {
        Dname = dname;
        Loc = loc;
    }

    public DeptVO(int deptno, string? dname, string? loc)
    {
        Deptno = deptno;
        Dname = dname;
        Loc = loc;
    }

    public int Deptno { get; set; }

    public string? Dname { get; set; }

    public string? Loc { get; set; }

    [JsonIgnore]
    public virtual ICollection<EmpVO> EmpVOs { get; } = new List<EmpVO>();
}
