using System;
using System.IO;

namespace SpriteSheetMaker
{
    public sealed class FileInfo : IEquatable<FileInfo>, IComparable<FileInfo>
    {
        public readonly string name;
        public readonly string path;

        public FileInfo(string path)
        {
            this.path = path;
            this.name = Path.GetFileNameWithoutExtension(path);
        }

        public override string ToString() => this.name;

        public override int GetHashCode() => this.path.GetHashCode();
        public override bool Equals(object obj) => obj is FileInfo info && this.Equals(info);
        public bool Equals(FileInfo info) => info.path == this.path;

        public int CompareTo(FileInfo info) => this.path.CompareTo(info.path);
    }
}