using AspNetCore.StartUpTemplate.Model;
using Autofac.Extras.DynamicProxy;

namespace AspNetCore.StartUpTemplate.IService;
public interface ITestService
{
    void TestNestedTransOk();
    void TestNestedTransError();
}