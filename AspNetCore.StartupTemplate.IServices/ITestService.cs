using AspNetCore.StartUpTemplate.Model;

namespace AspNetCore.StartUpTemplate.IService;
public interface ITestService
{
    void TestNestedTransOk();
    void TestNestedTransError();
}