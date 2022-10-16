namespace AspNetCore.StartUpTemplate.Utility.Utils;

public static class PathUtil
{
    /// <summary>
    /// 获取执行程序所在目录 可以获取到/debug级别目录
    /// </summary>
    /// <returns></returns>
    public static string GetExecuteDir()
    {
        return AppContext.BaseDirectory;
    } 
    /// <summary>
    /// 获取工作目录,在DEBUG模式下获取的是项目目录
    /// </summary>
    /// <returns></returns>
    public static string GetWorkingDir()
    {
        return Environment.CurrentDirectory;
    } 
}