using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public IEnumerable<DeptVO> Get()
    {
        return _myDbContext.DeptVOs.ToList();
    }
    
}