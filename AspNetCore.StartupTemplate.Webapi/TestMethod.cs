using AspNetCore.StartUpTemplate.Configuration;
using AspNetCore.StartUpTemplate.Model;
using SqlSugar;

namespace AspNetCore.StartUpTemplate.Webapi;

public class TestMethod
{

    public void FunA()
    {
        var db=GetDbClient();
        db.BeginTran();
        var user = new Users();
        var id=SnowFlakeSingle.Instance.NextId();
        user.Id = id;
        user.UserName = Path.GetRandomFileName();
        db.Insertable<Users>(user).ExecuteCommand();
        FunB();
        db.CommitTran();
    }

    public void FunB()
    {
        var db = GetDbClient();
        db.BeginTran();
        var user = new Users();
        var id=SnowFlakeSingle.Instance.NextId();
        user.Id = id;
        user.UserName = Path.GetRandomFileName()+"from funb";
        db.Insertable<Users>(user).ExecuteCommand();
        db.RollbackTran();
    }
    private SqlSugarClient GetDbClient()
    {
        return  new SqlSugarClient(new ConnectionConfig(){
            ConnectionString = AppSettingsConstVars.DbConnection, 
            DbType = DbType.MySql,
            IsAutoCloseConnection = true});
    }
}