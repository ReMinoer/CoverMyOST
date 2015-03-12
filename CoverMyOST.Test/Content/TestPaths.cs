using System.IO;

namespace CoverMyOST.Test.Content
{
    static internal class TestPaths
    {
        static private readonly string RootDirectory = Path.GetFullPath("Content/");
        static public readonly string MusicDirectory = Path.GetFullPath(Path.Combine(RootDirectory, "Music/"));
        static public readonly string MusicA = Path.GetFullPath(Path.Combine(MusicDirectory, "musicA.mp3"));
        static public readonly string MusicB = Path.GetFullPath(Path.Combine(MusicDirectory, "musicB.mp3"));
        static public readonly string MusicC = Path.GetFullPath(Path.Combine(MusicDirectory, "musicC.mp3"));
        static public readonly string CoverDirectory = Path.GetFullPath(Path.Combine(RootDirectory, "Covers/"));
        static public readonly string CoverA = Path.GetFullPath(Path.Combine(CoverDirectory, "coverA.jpg"));
        static public readonly string CoverDirectoryBis = Path.GetFullPath(Path.Combine(RootDirectory, "Covers-bis/"));
        static public readonly string CoverB = Path.GetFullPath(Path.Combine(CoverDirectoryBis, "coverB.jpg"));
    }
}