// See https://aka.ms/new-console-template for more information

using AspNetCore.StartUpTemplate.CoreGeneratorConsole;

Console.WriteLine("Hello, World!");

// 实体类名
const string MODEL_CLASS_NAME = "TESTMODEL";
// 生成的类描述
const string CLASS_DESCRIPTION = "新增的测试类";
// 对应的文件
string PATH = Path.Combine( Environment.CurrentDirectory,$"template-{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}.zip");
// 命名空间前缀 
const string NAMESPACE_PREFIX = "AspNetCore.StartupTemplate.Logging";

GeneratorCodeHelper.CodeGenerator(MODEL_CLASS_NAME,CLASS_DESCRIPTION,PATH,NAMESPACE_PREFIX);