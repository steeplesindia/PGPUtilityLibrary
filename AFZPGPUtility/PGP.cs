using PgpCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AFZPGPUtility
{
    public class PGP
    {
        private string PartnerPublicKey { get; set; }
        private string MyPrivateKey { get; set; }
        private string PartnerPrivateKey { get; set; }
        public string Password { get; set; }
        public PGP(string partnerPublicKey, string myPrivateKey, string partnerPrivateKey, string password)
        {
            this.PartnerPublicKey = partnerPublicKey;
            this.MyPrivateKey = myPrivateKey;
            this.PartnerPrivateKey = partnerPrivateKey;
            this.Password = password;
        }   
        public async Task<Response> encryptAsync(FileInfo inputFile, FileInfo encryptedFile)
        {
            Response response = new Response();
            try
            {
                EncryptionKeys encryptionKeys = new EncryptionKeys(PartnerPublicKey, MyPrivateKey, Password);
                PgpCore.PGP pgp = new PgpCore.PGP(encryptionKeys);
                await pgp.EncryptAndSignAsync(inputFile, encryptedFile);
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorText = ex.Message;
            }
            return response;
        }

        public async Task<Response> encryptAsync(string plainText)
        {
            Response response = new Response();
            try
            {
                EncryptionKeys encryptionKeys = new EncryptionKeys(PartnerPublicKey, MyPrivateKey, Password);
                PgpCore.PGP pgp = new PgpCore.PGP(encryptionKeys);
                string encryptedText  = await pgp.EncryptAndSignAsync(plainText);
                response.EncryptedText = encryptedText;
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorText = ex.Message;
            }
            return response;
        }

        public async Task<Response> encryptAsync(Stream inputStream)
        {
            Response response = new Response();
            try
            {
                EncryptionKeys encryptionKeys = new EncryptionKeys(PartnerPublicKey, MyPrivateKey, Password);
                PgpCore.PGP pgp = new PgpCore.PGP(encryptionKeys);
                Stream encryptedStream = new MemoryStream();
                await pgp.EncryptAndSignAsync(inputStream, encryptedStream);
                response.EncryptedStream = encryptedStream;
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorText = ex.Message;
            }
            return response;
        }

        public async Task<Response> decryptAsync(FileInfo encryptedFile, FileInfo decryptedFile)
        {
            Response response = new Response();
            try
            {
                EncryptionKeys encryptionKeys = new EncryptionKeys(PartnerPrivateKey, Password);
                PgpCore.PGP pgp = new PgpCore.PGP(encryptionKeys);
                await pgp.DecryptAsync(encryptedFile, decryptedFile);
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorText = ex.Message;
            }
            return response;
        }

        public async Task<Response> decryptAsync(string encryptedText)
        {
            Response response = new Response();
            try
            {
                EncryptionKeys encryptionKeys = new EncryptionKeys(PartnerPrivateKey, Password);
                PgpCore.PGP pgp = new PgpCore.PGP(encryptionKeys);
                string decryptedText = await pgp.DecryptAsync(encryptedText);
                response.DecryptedText = decryptedText;
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorText = ex.Message;
            }
            return response;
        }

        public async Task<Response> decryptAsync(Stream encryptedStream)
        {
            Response response = new Response();
            try
            {
                EncryptionKeys encryptionKeys = new EncryptionKeys(PartnerPrivateKey, Password);
                PgpCore.PGP pgp = new PgpCore.PGP(encryptionKeys);
                Stream decryptedStream = new MemoryStream();
                await pgp.DecryptAsync(encryptedStream, decryptedStream);
                response.DecryptedStream = decryptedStream;
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.ErrorText = ex.Message;
            }
            return response;
        }
    }

    public class Response
    {
        public bool Error { get; set; }
        public string ErrorText { get; set; } = string.Empty;
        public string EncryptedText { get; set; } = string.Empty;
        public Stream? EncryptedStream { get; set; } 
        public string DecryptedText { get; set; } = string.Empty;
        public Stream? DecryptedStream { get; set; }
    }
}
