using System;
using System.Collections.Generic;
using System.Linq;
using FreeSql.DataAnnotations;

namespace AspNetCore.StartUpTemplate.Model;

  
  public class Test
  {
   
      /// <summary>
      /// id 
      ///</summary>
      [Column(IsPrimary = true)]
       public long Id { get; set; }
      /// <summary>
      ///  用户名
      ///</summary>
       public string UserName { get; set; }
      /// <summary>
      ///  密码
      ///</summary>
       public string Password { get; set; }
      /// <summary>
      ///  地址
      ///</summary>
       public string Address { get; set; }
      /// <summary>
      ///  邮箱
      ///</summary>
       public string Email { get; set; }
      /// <summary>
      ///  电话
      ///</summary>
       public string Phone { get; set; }
      /// <summary>
      ///  头像地址
      ///</summary>
       public string IconUrl { get; set; }
  }
