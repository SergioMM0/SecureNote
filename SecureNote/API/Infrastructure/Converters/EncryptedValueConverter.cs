using API.Core.Helpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Infrastructure.Converters;

public class EncryptedValueConverter : ValueConverter<string, string> {
    public EncryptedValueConverter(string masterKey)
        : base(
            v => EncryptionHelper.Encrypt(v, masterKey),
            v => EncryptionHelper.Decrypt(v, masterKey)) {
    }
}
