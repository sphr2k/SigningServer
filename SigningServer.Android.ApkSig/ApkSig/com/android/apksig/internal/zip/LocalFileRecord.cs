// <auto-generated>
// This code was auto-generated.
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
// </auto-generated>

/*
 * Copyright (C) 2022 Daniel Kuschny (C# port)
 * Copyright (C) 2016 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace SigningServer.Android.Com.Android.Apksig.Internal.Zip
{
    /// <summary>
    /// ZIP Local File record.
    /// 
    /// &lt;p&gt;The record consists of the Local File Header, file data, and (if present) Data Descriptor.
    /// </summary>
    public class LocalFileRecord
    {
        internal static readonly int RECORD_SIGNATURE = 0x04034b50;
        
        internal static readonly int HEADER_SIZE_BYTES = 30;
        
        internal static readonly int GP_FLAGS_OFFSET = 6;
        
        internal static readonly int CRC32_OFFSET = 14;
        
        internal static readonly int COMPRESSED_SIZE_OFFSET = 18;
        
        internal static readonly int UNCOMPRESSED_SIZE_OFFSET = 22;
        
        internal static readonly int NAME_LENGTH_OFFSET = 26;
        
        internal static readonly int EXTRA_LENGTH_OFFSET = 28;
        
        internal static readonly int NAME_OFFSET = SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.HEADER_SIZE_BYTES;
        
        internal static readonly int DATA_DESCRIPTOR_SIZE_BYTES_WITHOUT_SIGNATURE = 12;
        
        internal static readonly int DATA_DESCRIPTOR_SIGNATURE = 0x08074b50;
        
        internal readonly string mName;
        
        internal readonly int mNameSizeBytes;
        
        internal readonly SigningServer.Android.IO.ByteBuffer mExtra;
        
        internal readonly long mStartOffsetInArchive;
        
        internal readonly long mSize;
        
        internal readonly int mDataStartOffset;
        
        internal readonly long mDataSize;
        
        internal readonly bool mDataCompressed;
        
        internal readonly long mUncompressedDataSize;
        
        internal LocalFileRecord(string name, int nameSizeBytes, SigningServer.Android.IO.ByteBuffer extra, long startOffsetInArchive, long size, int dataStartOffset, long dataSize, bool dataCompressed, long uncompressedDataSize)
        {
            mName = name;
            mNameSizeBytes = nameSizeBytes;
            mExtra = extra;
            mStartOffsetInArchive = startOffsetInArchive;
            mSize = size;
            mDataStartOffset = dataStartOffset;
            mDataSize = dataSize;
            mDataCompressed = dataCompressed;
            mUncompressedDataSize = uncompressedDataSize;
        }
        
        public virtual string GetName()
        {
            return mName;
        }
        
        public virtual SigningServer.Android.IO.ByteBuffer GetExtra()
        {
            return (mExtra.Capacity() > 0) ? mExtra.Slice() : mExtra;
        }
        
        public virtual int GetExtraFieldStartOffsetInsideRecord()
        {
            return SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.HEADER_SIZE_BYTES + mNameSizeBytes;
        }
        
        public virtual long GetStartOffsetInArchive()
        {
            return mStartOffsetInArchive;
        }
        
        public virtual int GetDataStartOffsetInRecord()
        {
            return mDataStartOffset;
        }
        
        /// <summary>
        /// Returns the size (in bytes) of this record.
        /// </summary>
        public virtual long GetSize()
        {
            return mSize;
        }
        
        /// <summary>
        /// Returns {@code true} if this record's file data is stored in compressed form.
        /// </summary>
        public virtual bool IsDataCompressed()
        {
            return mDataCompressed;
        }
        
        /// <summary>
        /// Returns the Local File record starting at the current position of the provided buffer
        /// and advances the buffer's position immediately past the end of the record. The record
        /// consists of the Local File Header, data, and (if present) Data Descriptor.
        /// </summary>
        public static SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord GetRecord(SigningServer.Android.Com.Android.Apksig.Util.DataSource apk, SigningServer.Android.Com.Android.Apksig.Internal.Zip.CentralDirectoryRecord cdRecord, long cdStartOffset)
        {
            return SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.GetRecord(apk, cdRecord, cdStartOffset, true, true);
        }
        
        /// <summary>
        /// Returns the Local File record starting at the current position of the provided buffer
        /// and advances the buffer's position immediately past the end of the record. The record
        /// consists of the Local File Header, data, and (if present) Data Descriptor.
        /// </summary>
        internal static SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord GetRecord(SigningServer.Android.Com.Android.Apksig.Util.DataSource apk, SigningServer.Android.Com.Android.Apksig.Internal.Zip.CentralDirectoryRecord cdRecord, long cdStartOffset, bool extraFieldContentsNeeded, bool dataDescriptorIncluded)
        {
            string entryName = cdRecord.GetName();
            int cdRecordEntryNameSizeBytes = cdRecord.GetNameSizeBytes();
            int headerSizeWithName = SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.HEADER_SIZE_BYTES + cdRecordEntryNameSizeBytes;
            long headerStartOffset = cdRecord.GetLocalFileHeaderOffset();
            long headerEndOffset = headerStartOffset + headerSizeWithName;
            if (headerEndOffset > cdStartOffset)
            {
                throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Local File Header of " + entryName + " extends beyond start of Central" + " Directory. LFH end: " + headerEndOffset + ", CD start: " + cdStartOffset);
            }
            SigningServer.Android.IO.ByteBuffer header;
            try
            {
                header = apk.GetByteBuffer(headerStartOffset, headerSizeWithName);
            }
            catch (global::System.IO.IOException e)
            {
                throw new global::System.IO.IOException("Failed to read Local File Header of " + entryName, e);
            }
            header.Order(SigningServer.Android.IO.ByteOrder.LITTLE_ENDIAN);
            int recordSignature = header.GetInt();
            if (recordSignature != SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.RECORD_SIGNATURE)
            {
                throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Not a Local File Header record for entry " + entryName + ". Signature: 0x" + (recordSignature & 0xffffffffL).ToString("x"));
            }
            short gpFlags = header.GetShort(SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.GP_FLAGS_OFFSET);
            bool dataDescriptorUsed = (gpFlags & SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GP_FLAG_DATA_DESCRIPTOR_USED) != 0;
            bool cdDataDescriptorUsed = (cdRecord.GetGpFlags() & SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GP_FLAG_DATA_DESCRIPTOR_USED) != 0;
            if (dataDescriptorUsed != cdDataDescriptorUsed)
            {
                throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Data Descriptor presence mismatch between Local File Header and Central" + " Directory for entry " + entryName + ". LFH: " + dataDescriptorUsed + ", CD: " + cdDataDescriptorUsed);
            }
            long uncompressedDataCrc32FromCdRecord = cdRecord.GetCrc32();
            long compressedDataSizeFromCdRecord = cdRecord.GetCompressedSize();
            long uncompressedDataSizeFromCdRecord = cdRecord.GetUncompressedSize();
            if (!dataDescriptorUsed)
            {
                long crc32 = SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GetUnsignedInt32(header, SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.CRC32_OFFSET);
                if (crc32 != uncompressedDataCrc32FromCdRecord)
                {
                    throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("CRC-32 mismatch between Local File Header and Central Directory for entry " + entryName + ". LFH: " + crc32 + ", CD: " + uncompressedDataCrc32FromCdRecord);
                }
                long compressedSize = SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GetUnsignedInt32(header, SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.COMPRESSED_SIZE_OFFSET);
                if (compressedSize != compressedDataSizeFromCdRecord)
                {
                    throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Compressed size mismatch between Local File Header and Central Directory" + " for entry " + entryName + ". LFH: " + compressedSize + ", CD: " + compressedDataSizeFromCdRecord);
                }
                long uncompressedSize = SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GetUnsignedInt32(header, SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.UNCOMPRESSED_SIZE_OFFSET);
                if (uncompressedSize != uncompressedDataSizeFromCdRecord)
                {
                    throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Uncompressed size mismatch between Local File Header and Central Directory" + " for entry " + entryName + ". LFH: " + uncompressedSize + ", CD: " + uncompressedDataSizeFromCdRecord);
                }
            }
            int nameLength = SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GetUnsignedInt16(header, SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.NAME_LENGTH_OFFSET);
            if (nameLength > cdRecordEntryNameSizeBytes)
            {
                throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Name mismatch between Local File Header and Central Directory for entry" + entryName + ". LFH: " + nameLength + " bytes, CD: " + cdRecordEntryNameSizeBytes + " bytes");
            }
            string name = SigningServer.Android.Com.Android.Apksig.Internal.Zip.CentralDirectoryRecord.GetName(header, SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.NAME_OFFSET, nameLength);
            if (!entryName.Equals(name))
            {
                throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Name mismatch between Local File Header and Central Directory. LFH: \\" + name + "\\, CD: \\" + entryName + "\\");
            }
            int extraLength = SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GetUnsignedInt16(header, SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.EXTRA_LENGTH_OFFSET);
            long dataStartOffset = headerStartOffset + SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.HEADER_SIZE_BYTES + nameLength + extraLength;
            long dataSize;
            bool compressed = (cdRecord.GetCompressionMethod() != SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.COMPRESSION_METHOD_STORED);
            if (compressed)
            {
                dataSize = compressedDataSizeFromCdRecord;
            }
            else 
            {
                dataSize = uncompressedDataSizeFromCdRecord;
            }
            long dataEndOffset = dataStartOffset + dataSize;
            if (dataEndOffset > cdStartOffset)
            {
                throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Local File Header data of " + entryName + " overlaps with Central Directory" + ". LFH data start: " + dataStartOffset + ", LFH data end: " + dataEndOffset + ", CD start: " + cdStartOffset);
            }
            SigningServer.Android.IO.ByteBuffer extra = SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.EMPTY_BYTE_BUFFER;
            if ((extraFieldContentsNeeded) && (extraLength > 0))
            {
                extra = apk.GetByteBuffer(headerStartOffset + SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.HEADER_SIZE_BYTES + nameLength, extraLength);
            }
            long recordEndOffset = dataEndOffset;
            if ((dataDescriptorIncluded) && ((gpFlags & SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GP_FLAG_DATA_DESCRIPTOR_USED) != 0))
            {
                long dataDescriptorEndOffset = dataEndOffset + SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.DATA_DESCRIPTOR_SIZE_BYTES_WITHOUT_SIGNATURE;
                if (dataDescriptorEndOffset > cdStartOffset)
                {
                    throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Data Descriptor of " + entryName + " overlaps with Central Directory" + ". Data Descriptor end: " + dataEndOffset + ", CD start: " + cdStartOffset);
                }
                SigningServer.Android.IO.ByteBuffer dataDescriptorPotentialSig = apk.GetByteBuffer(dataEndOffset, 4);
                dataDescriptorPotentialSig.Order(SigningServer.Android.IO.ByteOrder.LITTLE_ENDIAN);
                if (dataDescriptorPotentialSig.GetInt() == SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.DATA_DESCRIPTOR_SIGNATURE)
                {
                    dataDescriptorEndOffset += 4;
                    if (dataDescriptorEndOffset > cdStartOffset)
                    {
                        throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Data Descriptor of " + entryName + " overlaps with Central Directory" + ". Data Descriptor end: " + dataEndOffset + ", CD start: " + cdStartOffset);
                    }
                }
                recordEndOffset = dataDescriptorEndOffset;
            }
            long recordSize = recordEndOffset - headerStartOffset;
            int dataStartOffsetInRecord = SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.HEADER_SIZE_BYTES + nameLength + extraLength;
            return new SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord(entryName, cdRecordEntryNameSizeBytes, extra, headerStartOffset, recordSize, dataStartOffsetInRecord, dataSize, compressed, uncompressedDataSizeFromCdRecord);
        }
        
        /// <summary>
        /// Outputs this record and returns returns the number of bytes output.
        /// </summary>
        public virtual long OutputRecord(SigningServer.Android.Com.Android.Apksig.Util.DataSource sourceApk, SigningServer.Android.Com.Android.Apksig.Util.DataSink output)
        {
            long size = GetSize();
            sourceApk.Feed(GetStartOffsetInArchive(), size, output);
            return size;
        }
        
        /// <summary>
        /// Outputs this record, replacing its extra field with the provided one, and returns returns the
        /// number of bytes output.
        /// </summary>
        public virtual long OutputRecordWithModifiedExtra(SigningServer.Android.Com.Android.Apksig.Util.DataSource sourceApk, SigningServer.Android.IO.ByteBuffer extra, SigningServer.Android.Com.Android.Apksig.Util.DataSink output)
        {
            long recordStartOffsetInSource = GetStartOffsetInArchive();
            int extraStartOffsetInRecord = GetExtraFieldStartOffsetInsideRecord();
            int extraSizeBytes = extra.Remaining();
            int headerSize = extraStartOffsetInRecord + extraSizeBytes;
            SigningServer.Android.IO.ByteBuffer header = SigningServer.Android.IO.ByteBuffer.Allocate(headerSize);
            header.Order(SigningServer.Android.IO.ByteOrder.LITTLE_ENDIAN);
            sourceApk.CopyTo(recordStartOffsetInSource, extraStartOffsetInRecord, header);
            header.Put(extra.Slice());
            header.Flip();
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.SetUnsignedInt16(header, SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.EXTRA_LENGTH_OFFSET, extraSizeBytes);
            long outputByteCount = header.Remaining();
            output.Consume(header);
            long remainingRecordSize = GetSize() - mDataStartOffset;
            sourceApk.Feed(recordStartOffsetInSource + mDataStartOffset, remainingRecordSize, output);
            outputByteCount += remainingRecordSize;
            return outputByteCount;
        }
        
        /// <summary>
        /// Outputs the specified Local File Header record with its data and returns the number of bytes
        /// output.
        /// </summary>
        public static long OutputRecordWithDeflateCompressedData(string name, int lastModifiedTime, int lastModifiedDate, byte[] compressedData, long crc32, long uncompressedSize, SigningServer.Android.Com.Android.Apksig.Util.DataSink output)
        {
            byte[] nameBytes = name.GetBytes(SigningServer.Android.IO.Charset.StandardCharsets.UTF_8);
            int recordSize = SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.HEADER_SIZE_BYTES + nameBytes.Length;
            SigningServer.Android.IO.ByteBuffer result = SigningServer.Android.IO.ByteBuffer.Allocate(recordSize);
            result.Order(SigningServer.Android.IO.ByteOrder.LITTLE_ENDIAN);
            result.PutInt(SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.RECORD_SIGNATURE);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.PutUnsignedInt16(result, 0x14);
            result.PutShort(SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.GP_FLAG_EFS);
            result.PutShort(SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.COMPRESSION_METHOD_DEFLATED);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.PutUnsignedInt16(result, lastModifiedTime);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.PutUnsignedInt16(result, lastModifiedDate);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.PutUnsignedInt32(result, crc32);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.PutUnsignedInt32(result, compressedData.Length);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.PutUnsignedInt32(result, uncompressedSize);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.PutUnsignedInt16(result, nameBytes.Length);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.ZipUtils.PutUnsignedInt16(result, 0);
            result.Put(nameBytes);
            if (result.HasRemaining())
            {
                throw new SigningServer.Android.Core.RuntimeException("pos: " + result.Position() + ", limit: " + result.Limit());
            }
            result.Flip();
            long outputByteCount = result.Remaining();
            output.Consume(result);
            outputByteCount += compressedData.Length;
            output.Consume(compressedData, 0, compressedData.Length);
            return outputByteCount;
        }
        
        internal static readonly SigningServer.Android.IO.ByteBuffer EMPTY_BYTE_BUFFER = SigningServer.Android.IO.ByteBuffer.Allocate(0);
        
        /// <summary>
        /// Sends uncompressed data of this record into the the provided data sink.
        /// </summary>
        public virtual void OutputUncompressedData(SigningServer.Android.Com.Android.Apksig.Util.DataSource lfhSection, SigningServer.Android.Com.Android.Apksig.Util.DataSink sink)
        {
            long dataStartOffsetInArchive = mStartOffsetInArchive + mDataStartOffset;
            try
            {
                if (mDataCompressed)
                {
                    try
                    {
                        using(SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.InflateSinkAdapter inflateAdapter = new SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.InflateSinkAdapter(sink))
                        {
                            lfhSection.Feed(dataStartOffsetInArchive, mDataSize, inflateAdapter);
                            long actualUncompressedSize = inflateAdapter.GetOutputByteCount();
                            if (actualUncompressedSize != mUncompressedDataSize)
                            {
                                throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Unexpected size of uncompressed data of " + mName + ". Expected: " + mUncompressedDataSize + " bytes" + ", actual: " + actualUncompressedSize + " bytes");
                            }
                        }
                    }
                    catch (global::System.IO.IOException e)
                    {
                        if (e.InnerException is SigningServer.Android.Util.Zip.DataFormatException)
                        {
                            throw new SigningServer.Android.Com.Android.Apksig.Zip.ZipFormatException("Data of entry " + mName + " malformed", e);
                        }
                        throw e;
                    }
                }
                else 
                {
                    lfhSection.Feed(dataStartOffsetInArchive, mDataSize, sink);
                }
            }
            catch (global::System.IO.IOException e)
            {
                throw new global::System.IO.IOException("Failed to read data of " + ((mDataCompressed) ? "compressed" : "uncompressed") + " entry " + mName, e);
            }
        }
        
        /// <summary>
        /// Sends uncompressed data pointed to by the provided ZIP Central Directory (CD) record into the
        /// provided data sink.
        /// </summary>
        public static void OutputUncompressedData(SigningServer.Android.Com.Android.Apksig.Util.DataSource source, SigningServer.Android.Com.Android.Apksig.Internal.Zip.CentralDirectoryRecord cdRecord, long cdStartOffsetInArchive, SigningServer.Android.Com.Android.Apksig.Util.DataSink sink)
        {
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord lfhRecord = SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.GetRecord(source, cdRecord, cdStartOffsetInArchive, false, false);
            lfhRecord.OutputUncompressedData(source, sink);
        }
        
        /// <summary>
        /// Returns the uncompressed data pointed to by the provided ZIP Central Directory (CD) record.
        /// </summary>
        public static byte[] GetUncompressedData(SigningServer.Android.Com.Android.Apksig.Util.DataSource source, SigningServer.Android.Com.Android.Apksig.Internal.Zip.CentralDirectoryRecord cdRecord, long cdStartOffsetInArchive)
        {
            if (cdRecord.GetUncompressedSize() > int.MaxValue)
            {
                throw new global::System.IO.IOException(cdRecord.GetName() + " too large: " + cdRecord.GetUncompressedSize());
            }
            byte[] result = new byte[(int)cdRecord.GetUncompressedSize()];
            SigningServer.Android.IO.ByteBuffer resultBuf = SigningServer.Android.IO.ByteBuffer.Wrap(result);
            SigningServer.Android.Com.Android.Apksig.Internal.Util.ByteBufferSink resultSink = new SigningServer.Android.Com.Android.Apksig.Internal.Util.ByteBufferSink(resultBuf);
            SigningServer.Android.Com.Android.Apksig.Internal.Zip.LocalFileRecord.OutputUncompressedData(source, cdRecord, cdStartOffsetInArchive, resultSink);
            return result;
        }
        
        /// <summary>
        /// {@link DataSink} which inflates received data and outputs the deflated data into the provided
        /// delegate sink.
        /// </summary>
        internal class InflateSinkAdapter: SigningServer.Android.Com.Android.Apksig.Util.DataSink, System.IDisposable
        {
            internal readonly SigningServer.Android.Com.Android.Apksig.Util.DataSink mDelegate;
            
            internal SigningServer.Android.Util.Zip.Inflater mInflater = new SigningServer.Android.Util.Zip.Inflater(true);
            
            internal byte[] mOutputBuffer;
            
            internal byte[] mInputBuffer;
            
            internal long mOutputByteCount;
            
            internal bool mClosed;
            
            internal InflateSinkAdapter(SigningServer.Android.Com.Android.Apksig.Util.DataSink @delegate)
            {
                mDelegate = @delegate;
            }
            
            public void Consume(byte[] buf, int offset, int length)
            {
                CheckNotClosed();
                mInflater.SetInput(buf, offset, length);
                if (mOutputBuffer == null)
                {
                    mOutputBuffer = new byte[65536];
                }
                while (!mInflater.Finished())
                {
                    int outputChunkSize;
                    try
                    {
                        outputChunkSize = mInflater.Inflate(mOutputBuffer);
                    }
                    catch (SigningServer.Android.Util.Zip.DataFormatException e)
                    {
                        throw new global::System.IO.IOException("Failed to inflate data", e);
                    }
                    if (outputChunkSize == 0)
                    {
                        return;
                    }
                    mDelegate.Consume(mOutputBuffer, 0, outputChunkSize);
                    mOutputByteCount += outputChunkSize;
                }
            }
            
            public void Consume(SigningServer.Android.IO.ByteBuffer buf)
            {
                CheckNotClosed();
                if (buf.HasArray())
                {
                    Consume(buf.Array(), buf.ArrayOffset() + buf.Position(), buf.Remaining());
                    buf.Position(buf.Limit());
                }
                else 
                {
                    if (mInputBuffer == null)
                    {
                        mInputBuffer = new byte[65536];
                    }
                    while (buf.HasRemaining())
                    {
                        int chunkSize = global::System.Math.Min(buf.Remaining(), mInputBuffer.Length);
                        buf.Get(mInputBuffer, 0, chunkSize);
                        Consume(mInputBuffer, 0, chunkSize);
                    }
                }
            }
            
            public virtual long GetOutputByteCount()
            {
                return mOutputByteCount;
            }
            
            public void Dispose()
            {
                mClosed = true;
                mInputBuffer = null;
                mOutputBuffer = null;
                if (mInflater != null)
                {
                    mInflater.End();
                    mInflater = null;
                }
            }
            
            internal void CheckNotClosed()
            {
                if (mClosed)
                {
                    throw new System.InvalidOperationException("Closed");
                }
            }
            
        }
        
    }
    
}