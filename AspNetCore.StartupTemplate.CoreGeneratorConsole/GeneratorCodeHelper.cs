using System.IO.Compression;
using System.Text;
using DotLiquid;

namespace AspNetCore.StartUpTemplate.CoreGeneratorConsole;

/// <summary>
/// 自动生成代码 生成的部分包括
/// 1. Service层
/// 2. IService层
/// 3. Repository层
/// 4. IRepository层
/// 5. Controller层
/// </summary>
public static class GeneratorCodeHelper
{
    /// <summary>
    /// 当新增一个表时，通过本方法生成对应的service、repository、controller层模板代码
    /// </summary>
    /// <param name="modelName">模型类名</param>
    /// <param name="classDescription">类描述</param>
    /// <param name="path">压缩包路径及文件名</param>
    /// <param name="namespacePrefix">命名空间前缀</param>
    public static void CodeGenerator(string modelName, string classDescription, string path, string namespacePrefix)
    {
        //ModelClassName
        //ModelName
        //ModelFields  Name Comment
        var dt = DateTime.Now;
        byte[] data;
        var obj = new
        {
            ModelCreateTime = dt,
            ModelName = modelName,
            ModelDescription = classDescription,
            ModelClassName = modelName,
            NameSpacePrefix = namespacePrefix
        };
        using (var ms = new FileStream(path, FileMode.CreateNew))
        {
            using (ZipArchive zip = new ZipArchive(ms, ZipArchiveMode.Create, false))
            {
                string file;
                string result;
                Template template;
                using var controllerStream =
                    new FileStream(Path.Combine(Environment.CurrentDirectory, "CrudTemplate/Controllers/Controller.tpl"), FileMode.Open);
                //Controller
                using (var reader =
                       new StreamReader(
                           controllerStream, Encoding.UTF8))
                {
                    file = reader.ReadToEnd();
                    template = Template.Parse(file);
                    result = template.Render(Hash.FromAnonymousObject(obj));
                    ZipArchiveEntry entry4 = zip.CreateEntry("Controller/" + modelName + "Controller.cs");
                    using (StreamWriter entryStream = new StreamWriter(entry4.Open()))
                    {
                        entryStream.Write(result);
                    }
                }

                using var iRepositoryStream =
                    new FileStream(Path.Combine(Environment.CurrentDirectory, "CrudTemplate/Repositories/IRepository.tpl"), FileMode.Open);
                //IRespository
                using (var reader =
                       new StreamReader(iRepositoryStream, Encoding.UTF8))
                {
                    file = reader.ReadToEnd();
                    template = Template.Parse(file);
                    result = template.Render(Hash.FromAnonymousObject(obj));
                    ZipArchiveEntry entry3 = zip.CreateEntry("IRepository/I" + modelName + "Repository.cs");
                    using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                    {
                        entryStream.Write(result);
                    }
                }

                using var repositoryStream =
                    new FileStream(Path.Combine(Environment.CurrentDirectory, "CrudTemplate/Repositories/Repository.tpl"), FileMode.Open);
                //Respository
                using (var reader =
                       new StreamReader(repositoryStream, Encoding.UTF8))
                {
                    file = reader.ReadToEnd();
                    template = Template.Parse(file);
                    result = template.Render(Hash.FromAnonymousObject(obj));
                    ZipArchiveEntry entry1 = zip.CreateEntry("Repository/" + modelName + "Repository.cs");
                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                    {
                        entryStream.Write(result);
                    }
                }

                using var iServicesStream =
                    new FileStream(Path.Combine(Environment.CurrentDirectory, "CrudTemplate/Services/IServices.tpl"), FileMode.Open);
                //IServices
                using (var reader =
                       new StreamReader(iServicesStream, Encoding.UTF8))
                {
                    file = reader.ReadToEnd();
                    template = Template.Parse(file);
                    result = template.Render(Hash.FromAnonymousObject(obj));
                    ZipArchiveEntry entry3 = zip.CreateEntry("IServices/I" + modelName + "Services.cs");
                    using (StreamWriter entryStream = new StreamWriter(entry3.Open()))
                    {
                        entryStream.Write(result);
                    }
                }

                using var servicesStream =
                    new FileStream(Path.Combine(Environment.CurrentDirectory, "CrudTemplate/Services/Services.tpl"), FileMode.Open);
                //Services
                using (var reader =
                       new StreamReader(servicesStream, Encoding.UTF8))
                {
                    file = reader.ReadToEnd();
                    template = Template.Parse(file);
                    result = template.Render(Hash.FromAnonymousObject(obj));
                    ZipArchiveEntry entry1 = zip.CreateEntry("Services/" + modelName + "Services.cs");
                    using (StreamWriter entryStream = new StreamWriter(entry1.Open()))
                    {
                        entryStream.Write(result);
                    }
                }
            }
        }
    }
}