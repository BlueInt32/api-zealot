﻿using System.Text;

namespace SystemWrap
{
    public interface IFile
    {
        void WriteAllBytes(string path, byte[] bytes);
        void WriteAllText(string path, string text, Encoding encoding);
        string ReadAllText(string path, Encoding encoding);
        bool Exists(string path);
    }
}
