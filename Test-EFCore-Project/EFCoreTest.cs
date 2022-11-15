using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using Todo.Context;
using Todo.Models;
using Xunit.Abstractions;

namespace Test_EFCore_Project;

public class EFCoreTest
{
    // JetBrains Rider will warn you about usages of Console.WriteLine inside Fact methods
    // and suggest a quick-fix that will convert these usages to instances of ITestOutputHelper.
    // ref. https://www.jetbrains.com/help/rider/Xunit.XunitTestWithConsoleOutput.html
    private readonly ITestOutputHelper _testOutputHelper;

    private MyDbContext? _myDbContext;

    public EFCoreTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private void SetUp()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
        var config = builder.Build();

        // var dbContextBuilder = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        var dbContextBuilder =
            new DbContextOptionsBuilder<MyDbContext>().UseSqlServer(config.GetConnectionString("MyDatabaseConnString"));
        _myDbContext = new MyDbContext(dbContextBuilder.Options);
    }

    [Fact]
    public void testLinqJoin()
    {
        SetUp();
        // var targetEmp = 
        //     from emp in _myDbContext.EmpVOs 
        //     where EF.Functions.Like(emp.Ename, "M%") && (emp.Job == "CLERK")
        //     // where emp.Ename.StartsWith("M") && emp.Job == "CLERK"
        //     select emp;

        // var targetEmp = _myDbContext.EmpVOs.Where(vo =>
        //     vo.Ename.StartsWith("M") &&
        //     vo.Job == "CLERK"
        // );

        // JOIN + ORDERBY + WHERE
        // var targetEmp = from emp in _myDbContext.EmpVOs
        //     join dept in _myDbContext.DeptVOs on emp.Deptno equals dept.Deptno
        //     where emp.Ename.StartsWith("M")
        //     orderby emp.Empno ascending 
        //     select new { emp, dept.Dname };

        // JOIN + ORDERBY + WHERE (ref. https://dotblogs.com.tw/erictsaiblog/2015/05/17/151321)
        var targetEmp = _myDbContext.EmpVOs.Join(
            _myDbContext.DeptVOs, // 第一個參數為 要加入的資料來源
            empVO => empVO.Deptno, // 主表要join的值
            deptVO => deptVO.Deptno, // 次表要join的值
            (ee, dd) => new // (c,s)代表將資料集合起來
            {
                empNoGG = ee.Empno,
                empNameGG = ee.Ename,
                deptNameGG = dd.Dname
            }
        ).Where(result => result.empNameGG.StartsWith("M"))
            .OrderByDescending(result => result.empNoGG);
        
        // 印出結果
        targetEmp.ToList().ForEach(obj => _testOutputHelper.WriteLine(obj.ToString()));
    }

    [Fact]
    public void Test1_FindAllDepts()
    {
        SetUp();
        _testOutputHelper.WriteLine("========== 我是 EFCoreTest - Test1_FindAllDepts ==========");
        var deptVos = _myDbContext?.DeptVOs.OrderByDescending(vo => vo.Deptno).ToList();
        foreach (var dVO in deptVos)
        {
            _testOutputHelper.WriteLine(dVO.ToJson());
        }
    }

    [Fact]
    public void Test2_EagerFetch()
    {
        SetUp();
        _testOutputHelper.WriteLine("========== 我是 EFCoreTest - Test2_EagerFetch ==========");
        var deptVos = _myDbContext?.DeptVOs.Include(dept => dept.EmpVOs).ToList();
        foreach (var dVO in deptVos)
        {
            _testOutputHelper.WriteLine(dVO.ToJson());

            var empVOs = dVO.EmpVOs;
            foreach (var eVO in empVOs)
            {
                _testOutputHelper.WriteLine(">>> " + eVO.ToJson());
            }
        }
    }

    [Theory]
    [InlineData("研發部", "臺灣新竹")]
    [InlineData("生管部", "中國上海")]
    public void Test3_FindByCondition(string pDName, string pDLoc)
    {
        SetUp();
        _testOutputHelper.WriteLine("========== 我是 EFCoreTest - Test3_FindByName ==========");
        var targetVo = _myDbContext?.DeptVOs.Single(vo =>
            pDName.Equals(vo.Dname)
            &&
            pDLoc.Equals(vo.Loc)
        );
        _testOutputHelper.WriteLine(targetVo.ToJson());
    }

    [Theory]
    [InlineData("財務部")]
    [InlineData("業務部")]
    public void Test4_FindPureSQL(string pDName)
    {
        SetUp();
        _testOutputHelper.WriteLine("========== 我是 EFCoreTest - Test4_FindPureSQL ==========");
        var targetVo = _myDbContext?.DeptVOs.FromSql(
            $"SELECT t.* FROM DB_EMP_DEPT.dbo.DEPT_TB t WHERE DNAME = {pDName}"
        );
        _testOutputHelper.WriteLine(targetVo.ToJson());
    }

    [Fact]
    public void Test5_insertTest()
    {
        SetUp();
        _testOutputHelper.WriteLine("========== 我是 EFCoreTest - Test5_insertTest ==========");
        _myDbContext?.DeptVOs.Add(new DeptVO("國防部", "博愛特區"));
        _myDbContext?.SaveChanges();
    }

    [Fact]
    public void Test6_updateTest()
    {
        SetUp();
        _testOutputHelper.WriteLine("========== 我是 EFCoreTest - Test6_updateTest ==========");
        var wantToUpdateVo = new DeptVO();
        wantToUpdateVo.Deptno = 10;

        // var targetVO = _myDbContext?.DeptVOs.Find(deptVo.Deptno);
        var targetVO = (_myDbContext?.DeptVOs).Single(vo => vo.Deptno.Equals(wantToUpdateVo.Deptno));
        targetVO.Dname += "_New";
        _myDbContext?.SaveChanges();

        _testOutputHelper.WriteLine(targetVO.ToJson());
    }

    [Fact]
    public void Test7_DeleteTest()
    {
        SetUp();
        _testOutputHelper.WriteLine("========== 我是 EFCoreTest - Test7_DeleteTest ==========");
        var wantToDeleteVo = new DeptVO();
        wantToDeleteVo.Deptno = 50;
        var dbContextTransaction = _myDbContext?.Database.BeginTransaction();
        _myDbContext?.DeptVOs.Where(vo => vo.Deptno.Equals(wantToDeleteVo.Deptno)).ExecuteDelete();
        dbContextTransaction?.Rollback();
    }

    [Fact]
    public void Test8_DeleteCascadeTest()
    {
        SetUp();
        _testOutputHelper.WriteLine("========== 我是 EFCoreTest - Test8_DeleteCascadeTest ==========");

        var wantToDeleteVo = new DeptVO();
        wantToDeleteVo.Deptno = 20;
        
        var deptVo = _myDbContext?.DeptVOs.Include(vo => vo.EmpVOs) // Eager Loading
            .First(vo => vo.Deptno.Equals(wantToDeleteVo.Deptno));
        _testOutputHelper.WriteLine(deptVo.ToJson());
        foreach (var emp in deptVo.EmpVOs)
        {
            _testOutputHelper.WriteLine(" >>> " + emp.ToJson());
        }

        // Roger-Fix
        _myDbContext?.Remove(deptVo);
        
        _myDbContext?.SaveChanges();
    }
}