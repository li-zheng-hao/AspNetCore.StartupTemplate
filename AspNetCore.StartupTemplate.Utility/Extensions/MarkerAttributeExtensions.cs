using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AspNetCore.StartUpTemplate.Utility;


public static class MarkerAttributeExtensions
{
    /// <summary>
    /// 检查接口的controller上是否有指定的特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="that"></param>
    /// <returns></returns>
    public static bool ControllerHasAttribute<T>(this ActionDescriptor that) where T:Attribute
    {
        var controllerActionDescriptor = that as ControllerActionDescriptor;

        if (controllerActionDescriptor != null)
        {
           

            // Check if the attribute exists on the controller
            if (controllerActionDescriptor.ControllerTypeInfo?.GetCustomAttributes(typeof(T), true)?.Any() ?? false)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 检查接口上是否有指定的特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="that"></param>
    /// <returns></returns>
    public static bool MethodHasAttribute<T>(this ActionDescriptor that) where T : Attribute
    {
        var controllerActionDescriptor = that as ControllerActionDescriptor;

        if (controllerActionDescriptor != null)
        {
            // Check if the attribute exists on the action method
            if (controllerActionDescriptor.MethodInfo?.GetCustomAttributes(inherit: true)?.Any(a => a.GetType().Equals(typeof(T))) ?? false)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 检查接口上是否有指定的特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="that"></param>
    /// <returns></returns>
    public static bool MethodHasAttribute<T>(this MethodInfo that) where T : Attribute
    {
        // Check if the attribute exists on the action method
        if (that.GetCustomAttributes(inherit: true)?.Any(a => a.GetType().Equals(typeof(T))) ?? false)
                return true;
        return false;
    }
    /// <summary>
    /// 检查接口的controller或接口本身是否有指定的特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="that"></param>
    /// <returns></returns>
    public static bool ControllerOrMethodHasAttribute<T>(this ActionDescriptor that) where T : Attribute
    {
        var controllerActionDescriptor = that as ControllerActionDescriptor;

        if (controllerActionDescriptor != null)
        {
            // Check if the attribute exists on the action method
            if (controllerActionDescriptor.MethodInfo?.GetCustomAttributes(inherit: true)?.Any(a => a.GetType().Equals(typeof(T))) ?? false)
                return true;
            // Check if the attribute exists on controller 
            if (controllerActionDescriptor.ControllerTypeInfo?.GetCustomAttributes(inherit: true)?.Any(a => a.GetType().Equals(typeof(T))) ?? false)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 检查controller是否有指定特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="that"></param>
    /// <returns></returns>
    public static bool HasMarkerAttribute<T>(this ControllerBase that) where T : Attribute
    {
        return that.GetType().HasMarkerAttribute<T>();
    }
    /// <summary>
    /// 检查类型上是否有指定的特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="that"></param>
    /// <returns></returns>
    public static bool HasMarkerAttribute<T>(this Type that) where T : Attribute
    {
        return that.IsDefined(typeof(T), false);
    }
    /// <summary>
    /// 获取指定的特性
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="that"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetCustomAttributes<T>(this Type that) where T : Attribute
    {
        return that.GetCustomAttributes(typeof(T), false).Cast<T>();
    }

}
