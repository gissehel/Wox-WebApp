using AllGreen.Lib;
using Wox.WebApp.AllGreen.Helper;
using Wox.WebApp.Mock.Service;

namespace Wox.WebApp.AllGreen.Fixture
{
    public class Generate_file_fixture : FixtureBase<WebAppContext>
    {
        public string Line { get; set; }

        public override bool OnEnterSetup()
        {
            if (FileReaderService == null)
            {
                return false;
            }
            if (FileReaderService.LastCreatedFile == null)
            {
                FileReaderService.CreateFile();
            }
            FileReaderService.LastCreatedFile.Add(Line);
            return true;
        }

        public void Save_last_file_to(string path) => FileReaderService.SaveLastCreatedFileTo(path);

        private FileReaderServiceMock _fileReaderService;
        private FileReaderServiceMock FileReaderService => _fileReaderService ?? (_fileReaderService = Context.ApplicationStarter.FileReaderService);
    }
}