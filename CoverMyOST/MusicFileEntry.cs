namespace CoverMyOST
{
    public class MusicFileEntry : MusicFile
    {
        public bool Selected { get; set; }

        internal MusicFileEntry(string path)
            : base(path)
        {
            Selected = true;
        }
    }
}