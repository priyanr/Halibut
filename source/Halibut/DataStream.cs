using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Halibut
{
    public class DataStream : IEquatable<DataStream>
    {
        readonly Action<Stream> writer;
        Action<Action<Stream>> attachedReader;

        [JsonConstructor]
        public DataStream()
        {
        }

        public DataStream(long length, Action<Stream> writer)
        {
            Length = length;
            Id = Guid.NewGuid();
            this.writer = writer;
        }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("length")]
        public long Length { get; set; }

        public void Attach(Action<Action<Stream>> reader)
        {
            attachedReader = reader;
        }

        public void Read(Action<Stream> reader)
        {
            attachedReader(reader);
        }

        public void Write(Stream stream)
        {
            writer(stream);
        }

        public bool Equals(DataStream other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataStream)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(DataStream left, DataStream right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DataStream left, DataStream right)
        {
            return !Equals(left, right);
        }

        public static DataStream FromBytes(byte[] data)
        {
            return new DataStream(data.Length, stream => stream.Write(data, 0, data.Length));
        }

        public static DataStream FromString(string text)
        {
            return FromString(text, new UTF8Encoding(false));
        }

        public static DataStream FromString(string text, Encoding encoding)
        {
            return new DataStream(encoding.GetByteCount(text), stream =>
            {
                var writer = new StreamWriter(stream, encoding);
                writer.Write(text);
                writer.Flush();
            });
        }

        public static DataStream FromStream(Stream source)
        {
            return new DataStream(source.Length, stream =>
            {
                source.CopyTo(stream);
                stream.Flush();
            });
        }
    }
}