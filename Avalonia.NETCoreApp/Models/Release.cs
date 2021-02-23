using System.Collections.Generic;

namespace Avalonia.NETCoreApp.Models
{
    public class Release
    {
        public int Id { get;  set; }

        public string TagName { get;  set; }


        public string Name { get;  set; }

        public string Body { get;  set; }


        public bool Prerelease { get;  set; }
        
        public IReadOnlyList<ReleaseAsset> Assets { get;  set; }

    }
}