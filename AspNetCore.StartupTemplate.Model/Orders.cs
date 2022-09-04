using System;
using System.Collections.Generic;
using System.Linq;
using FreeSql.DataAnnotations;

namespace AspNetCore.StartUpTemplate.Model;

  
  public class Orders
  {
   
      /// <summary>
      /// id 
      ///</summary>
      [Column(IsPrimary = true)]
      public long Id { get; set; }
      /// <summary>
      /// 购买人id
      /// </summary>
      [Column(IsNullable = true)]

      public long  UserId { get; set; }
      /// <summary>
      /// 商品id
      /// </summary>
      [Column(IsNullable = true)]

      public long ProductId { get; set; }
      /// <summary>
      /// 价格
      /// </summary>
      [Column(IsNullable = true)]

      public double price { get; set; }
      /// <summary>
      /// 创建日期
      /// </summary>
      [Column(IsNullable = true)]
      public DateTime CreateTime { get; set; }
      /// <summary>
      /// 更新日期
      /// </summary>
      [Column(IsNullable = true)]
      public DateTime UpdateTime { get; set; }
  }
