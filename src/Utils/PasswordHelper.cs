using System;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public sealed class PasswordHelper
    {
        public static string GeneratePassword(int length)
        {
            var buffer = new byte[length];
            RandomChars(buffer, 0, buffer.Length);
            return Encoding.ASCII.GetString(buffer);
        }

        public static string HashPassword(string plainText)
        {
            return HashPassword(Encoding.UTF8.GetBytes(plainText));
        }
        public static string HashPassword(byte[] plainBuffer)
        {
            var resultLen = 512 / 8;
            var saltLen = 4;
            //var blockSize = 1024 * 8;

            var salt = new byte[saltLen];
            RandomSalt(salt, 0, salt.Length);

            var pwd = "";
            using (var sha512 = SHA512.Create())
            {
                sha512.Initialize();
                sha512.TransformBlock(plainBuffer, 0, plainBuffer.Length, plainBuffer, 0);
                sha512.TransformFinalBlock(salt, 0, saltLen);

                var hash = sha512.Hash;
                var saltedHash = new byte[hash.Length + saltLen];
                Array.Copy(hash, 0, saltedHash, 0, resultLen);
                Array.Copy(salt, 0, saltedHash, resultLen, saltLen);

                var hashed = Convert.ToBase64String(saltedHash);
                pwd = hashed;
            }

            return pwd;
        }

        private static void RandomChars(byte[] buffer, int offset, int count)
        {
            const string salt_chars =
                "$-()@&.,;!?/0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            RandomFill(salt_chars, buffer, offset, count);
        }
        private static void RandomSalt(byte[] buffer, int offset, int count)
        {
            const string salt_chars =
                "./0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            RandomFill(salt_chars, buffer, offset, count);
        }

        private static void RandomFill(string chars, byte[] buffer, int offset, int count)
        {
            var availableBytes = Encoding.ASCII.GetBytes(chars);

            var ul = 0x0000000007FFFFFFF;
            var seed = DateTime.UtcNow.Ticks;
            var masked = seed & ul;
            var random = new Random(Convert.ToInt32(masked));

            for (var index = offset; index < count; index++)
                buffer[index] = availableBytes[random.Next(availableBytes.Length)]; }
    }
}