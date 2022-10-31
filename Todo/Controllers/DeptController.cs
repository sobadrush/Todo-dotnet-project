using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Todo.Context;
using Todo.Models;

namespace Todo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeptController : ControllerBase
{
    
    // DI
    private readonly MyDbContext _myDbContext;

    // DI 
    public DeptController(MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
    }

    // GET: Dept
    [HttpGet(Name = "GetAllDepts")]
    public IEnumerable<DeptVO> Get()
    {
        return _myDbContext.DeptVOs
            // .Include(vo => vo.EmpVOs)
            .ToList();
    }
    
    // GET: https://localhost:7076/api/dept/30
    [HttpGet("{deptId}", Name = "GetDeptById")]
    public DeptVO Get(int deptId)
    {
        Console.Out.WriteLine("deptId = " + deptId);
        return _myDbContext.DeptVOs
            .Include(vo => vo.EmpVOs)
            .Single(vo => vo.Deptno.Equals(deptId));
    }
    
}