using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProtoFileComparer
{
    /// <summary>
    /// Represents temporary file. When instance is disposed, file will be deleted.
    /// </summary>
    public class TempFile : IDisposable
    {
        public TempFile()
        {
            Path = System.IO.Path.GetTempFileName();
        }

        public string Path
        {
            get;
            private set;
        }

        ~TempFile()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
                GC.SuppressFinalize(this); // this has been already disposed

            if (File.Exists(Path))
                try
                {
                    File.Delete(Path);
                }
                catch { }
        }
    }
}
