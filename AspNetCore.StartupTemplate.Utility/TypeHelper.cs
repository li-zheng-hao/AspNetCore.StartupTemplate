namespace AspNetCore.StartUpTemplate.Utility;

public static class TypeHelper
{
    public static bool IsSubClassOrEqualEx(this Type exA,Type exB)
    {
        return exA.IsSubclassOf(exB) || exA == exB;
    }
}