using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
namespace AspNetCore.StartUpTemplate.Model;

  /// <summary>
  /// 
  ///</summary>
  [SugarTable("Users")]
  public class Users
  {
      /// <summary>
      /// id 
      ///</summary>
       [SugarColumn(ColumnName="Id" ,IsPrimaryKey = true   )]
       public long Id { get; set; }
      /// <summary>
      ///  用户名
      ///</summary>
       [SugarColumn(ColumnName="UserName"    )]
       public string UserName { get; set; }
      /// <summary>
      ///  密码
      ///</summary>
       [SugarColumn(ColumnName="Password"    )]
       public string Password { get; set; }
      /// <summary>
      ///  地址
      ///</summary>
       [SugarColumn(ColumnName="Address"    )]
       public string Address { get; set; }
      /// <summary>
      ///  邮箱
      ///</summary>
       [SugarColumn(ColumnName="Email"    )]
       public string Email { get; set; }
      /// <summary>
      ///  电话
      ///</summary>
       [SugarColumn(ColumnName="Phone"    )]
       public string Phone { get; set; }
      /// <summary>
      ///  头像地址
      ///</summary>
       [SugarColumn(ColumnName="IconUrl"    )]
       public string IconUrl { get; set; }
  }
