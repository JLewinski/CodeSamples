using static CodeSamples.Extensions.DataAccess.DataReaderExtensions;

namespace CodeSamples.Test.Data
{
    /// <summary>
    /// First attempt at a unit test in a long time.
    /// </summary>
    public class ReadValueTest
    {
        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void Int_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<int>(value);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void IntNullable_ReadValueTest_Theory(object? value)
        {
            var result = ReadValue<int?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void IntNullable_ReadNull()
        {
            var result = ReadValue<int?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Int_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<int>(DBNull.Value); });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5.4)]
        [InlineData(7)]
        [InlineData(11.5)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void Double_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<double>(value);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5.4)]
        [InlineData(7)]
        [InlineData(11.5)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void DoubleNullable_ReadValueTest_Theory(object? value)
        {
            var result = ReadValue<double?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void DoubleNullable_ReadNull()
        {
            var result = ReadValue<double?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Double_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<double>(DBNull.Value); });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5.4)]
        [InlineData(7)]
        [InlineData(11.5)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void Decimal_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<decimal>(value);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5.4)]
        [InlineData(7)]
        [InlineData(11.5)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void DecimalNullable_ReadValueTest_Theory(object? value)
        {
            var result = ReadValue<decimal?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void DecimalNullable_ReadNull()
        {
            var result = ReadValue<decimal?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Decimal_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<decimal>(DBNull.Value); });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void Long_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<long>(value);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void LongNullable_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<long?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void LongNullable_ReadNull()
        {
            var result = ReadValue<long?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Long_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<long>(DBNull.Value); });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void Short_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<short>(value);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void ShortNullable_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<short?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void ShortNullable_ReadNull()
        {
            var result = ReadValue<short?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Short_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<short>(DBNull.Value); });
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void Byte_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<byte>(value);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(7)]
        [InlineData(11)]
        [InlineData(13)]
        [InlineData(17)]
        [InlineData(19)]
        public void ByteNullable_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<byte?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void ByteNullable_ReadNull()
        {
            var result = ReadValue<byte?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Byte_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<byte>(DBNull.Value); });
        }

        public enum TestEnum
        {
            Bill,
            Bob,
            Tim,
            Tom,
            Anthony,
            Jacob
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        public void Enum_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<TestEnum>(value);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void EnumNullable_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<TestEnum?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void EnumNullable_ReadNull()
        {
            var result = ReadValue<TestEnum?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Enum_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<TestEnum>(DBNull.Value); });
        }

        [Fact]
        public void DateTime_ReadValueTest_Theory()
        {
            object value = DateTime.Now;
            var result = ReadValue<DateTime>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void DateTimeNullable_ReadValueTest_Theory()
        {
            object value = DateTime.Now;
            var result = ReadValue<DateTime?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void DateTimeNullable_ReadNull()
        {
            var result = ReadValue<DateTime?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void DateTime_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<DateTime>(DBNull.Value); });
        }

        [Fact]
        public void String_ReadValueTest_Theory()
        {
            object value = DateTime.Now.ToLongDateString();
            var result = ReadValue<string>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void StringNullable_ReadValueTest_Theory()
        {
            object value = DateTime.Now.ToLongDateString();
            var result = ReadValue<string?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void StringNullable_ReadNull()
        {
            var result = ReadValue<string?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void String_ReadNull()
        {
            var result = ReadValue<string>(DBNull.Value);
            Assert.Null(result);
            //Assert.Throws<InvalidCastException>(() => { ReadValue<string>(DBNull.Value); });
        }

        [Fact]
        public void Char_ReadValueTest_Theory()
        {
            object value = DateTime.Now.ToLongDateString()[0];
            var result = ReadValue<char>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void CharNullable_ReadValueTestTheory()
        {
            object value = DateTime.Now.ToLongDateString()[0];
            var result = ReadValue<char?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void CharNullable_ReadNull()
        {
            var result = ReadValue<char?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Char_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<char>(DBNull.Value); });
        }

        [Fact]
        public void Guid_ReadValueTest_Theory()
        {
            object value = Guid.NewGuid();
            var result = ReadValue<Guid>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void GuidNullable_ReadValueTestTheory()
        {
            object value = Guid.NewGuid();
            var result = ReadValue<Guid?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void GuidNullable_ReadNull()
        {
            var result = ReadValue<Guid?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Guid_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<Guid>(DBNull.Value); });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(true)]
        [InlineData(false)]
        public void Bool_ReadValueTest_Theory(object value)
        {
            var result = ReadValue<bool>(value);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public void BoolNullable_ReadValueTestTheory(object value)
        {
            var result = ReadValue<bool?>(value);
            Assert.NotNull(result);
        }

        [Fact]
        public void BoolNullable_ReadNull()
        {
            var result = ReadValue<bool?>(DBNull.Value);
            Assert.Null(result);
        }

        [Fact]
        public void Bool_ReadNull()
        {
            Assert.Throws<InvalidCastException>(() => { ReadValue<bool>(DBNull.Value); });
        }
    }
}
