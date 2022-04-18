// <auto-generated>
// This code was auto-generated.
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
// </auto-generated>

using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SigningServer.Android.Com.Android.Apksig.Util
{
    /// <summary>
    /// Tests for the {@link DataSink} returned by
    /// {@link DataSinks#asDataSink(java.io.RandomAccessFile)}.
    /// </summary>
    [TestClass]
    public class DataSinkFromRAFTest: SigningServer.Android.Com.Android.Apksig.Util.DataSinkTestBase<Com.Android.Apksig.Internal.Util.RandomAccessFileDataSink>
    {
        protected override SigningServer.Android.Com.Android.Apksig.Util.DataSinkTestBase<Com.Android.Apksig.Internal.Util.RandomAccessFileDataSink>.CloseableWithDataSink CreateDataSink()
        {
            System.IO.FileInfo tmp = CreateTemporaryFile(typeof(SigningServer.Android.Com.Android.Apksig.Util.DataSourceFromRAFTest).Name, ".bin");
            SigningServer.Android.IO.RandomAccessFile f = null;
            try
            {
                f = new SigningServer.Android.IO.RandomAccessFile(tmp, "rw");
            }
            finally
            {
                if (f == null)
                {
                    tmp.Delete();
                }
            }
            return SigningServer.Android.Com.Android.Apksig.Util.DataSinkTestBase<Com.Android.Apksig.Internal.Util.RandomAccessFileDataSink>.CloseableWithDataSink.Of((Com.Android.Apksig.Internal.Util.RandomAccessFileDataSink)Com.Android.Apksig.Util.DataSinks.AsDataSink(f), new SigningServer.Android.Com.Android.Apksig.Util.DataSourceFromRAFTest.TmpFileCloseable(tmp, f));
        }

        protected override SigningServer.Android.IO.ByteBuffer GetContents(Com.Android.Apksig.Internal.Util.RandomAccessFileDataSink dataSink)
        {
            SigningServer.Android.IO.RandomAccessFile f = dataSink.GetFile();
            if (f.Length() > int.MaxValue)
            {
                throw new System.IO.IOException("File too large: " + f.Length());
            }
            byte[] contents = new byte[(int)f.Length()];
            f.Seek(0);
            f.ReadFully(contents);
            return SigningServer.Android.IO.ByteBuffer.Wrap(contents);
        }
        
    }
    
}