using System.IO;

namespace CoverMyOST.Test
{
    static internal class TestPaths
    {
		static public readonly string RootDirectory = Path.GetFullPath("Content/");

		static public readonly string MusicDirectory = Path.Combine(RootDirectory, "Music/");
        static public readonly string MusicA = Path.Combine(MusicDirectory, "musicA.mp3");
        static public readonly string MusicB = Path.Combine(MusicDirectory, "musicB.mp3");
		static public readonly string MusicC = Path.Combine(MusicDirectory, "dir/musicC.mp3");

		static public readonly string CoverDirectory = Path.Combine(RootDirectory, "Covers/");
        static public readonly string CoverA = Path.Combine(CoverDirectory, "coverA.jpg");
    }
}