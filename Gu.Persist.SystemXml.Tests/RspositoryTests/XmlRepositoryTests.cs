namespace Gu.Persist.SystemXml.Tests.RspositoryTests
{
    using System.IO;

    using Gu.Persist.Core;
    using Gu.Persist.Core.Tests.Repositories;
    using Gu.Persist.SystemXml;
    using Gu.Persist.SystemXml.Tests.Helpers;

    public class XmlRepositoryTests : RepositoryTests
    {
        protected override IRepository Create()
        {
            return new XmlRepository();
        }

        protected override void Save<T>(T item, FileInfo file)
        {
            TestHelper.Save(item, file);
        }

        protected override T Read<T>(FileInfo file)
        {
            return TestHelper.Read<T>(file);
        }
    }
}