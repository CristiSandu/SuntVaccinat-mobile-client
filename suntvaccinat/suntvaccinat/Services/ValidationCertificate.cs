using DgcReader;
using DgcReader.Models;
using GemBox.Pdf;
using GemBox.Pdf.Forms;
using GreenpassReader.Models;
using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Base45Utility;
using OfficeOpenXml.Packaging.Ionic.Zlib;
using Com.AugustCellars.COSE;
using PeterO.Cbor;
using suntvaccinat.Models.GreenPassModels;
using Newtonsoft.Json;

namespace suntvaccinat.Services
{
    class ValidationCertificate
    {
        static List<string> entities = new List<string> { "INSTITUTUL NATIONAL DE SANATATE PUBLICA" };

        public async static Task<bool> SaveUser(User user)
        {
            SecureStorage.RemoveAll();
            await SecureStorage.SetAsync(Helpers.Constants.User, user.ToString());

            return true;
        }

        public async static Task<SignedDgc> DecodeGreenPass(string certificate)
        {
            var dgcReader = new DgcReaderService();
            var decoded = await dgcReader.Decode(certificate);

            return decoded;
        }


        public async static Task<GreenPassModel> DecodeGreenPassPersonal(string certificate)
        {
            var qrmessage = certificate.Substring(4);//remove first 4 chars
            Base45Utility.Base45 base45 = new Base45Utility.Base45();

            byte[] decodedBase45 = base45.Decode(qrmessage);//using base45 lib
            var cose = ZlibStream.UncompressBuffer(decodedBase45);//using zlib or similar

            var decrypted = Message.DecodeFromBytes(cose).GetContent(); //using COSE

            CBORObject cbor = CBORObject.DecodeFromBytes(decrypted);    //using Peter.O.. CBOR

            var keys = cbor.Keys;

            Console.WriteLine(keys);
            var jsonDecoded = cbor.ToJSONString();

            GreenPassModel myDeserializedClass = JsonConvert.DeserializeObject<GreenPassModel>(jsonDecoded);

            return myDeserializedClass;
        }

        public static bool ValidateGreenPassForName(GreenPassModel decodedValue, User user)
        {
            if (decodedValue != null)
            {
                DateTime oDate = DateTime.Parse(decodedValue.Body.Content.DateOfBirth);
                int years = DateTime.Now.Year - oDate.Year;

                if (decodedValue.Body.Content.Name.Surname == user.Name.ToUpper().Trim() &&
                    decodedValue.Body.Content.Name.Forename == user.SecondName.ToUpper().Trim() &&
                    years == Convert.ToInt32(user.Age))
                {
                    return true;
                }
            }

            return false;
        }

        public async static Task<ValidationModel> GetValueToSaveOnServer(string certificate, string phoneId, User user)
        {
            ValidationModel valModel = new ValidationModel();

            var decodedValue = await DecodeGreenPassPersonal(certificate);

            //if (!ValidateGreenPassForName(decodedValue, user))
            //    return null;

            DateTimeOffset longest = DateTimeOffset.MinValue;

            Models.GreenPassModels.TestEntry maxTimeOffestTests = null;
            Models.GreenPassModels.RecoveryEntry maxTimeOffestRecoveries = null;
            Models.GreenPassModels.VaccinationEntry maxTimeOffestVaccinations = null;
            string certificateId = string.Empty;

            if (decodedValue.Body.Content.Recoveries != null && decodedValue.Body.Content.Recoveries.Any())
            {
                maxTimeOffestRecoveries = decodedValue.Body.Content.Recoveries.Last();
                longest = maxTimeOffestRecoveries.ExpirationDate;
                certificateId = maxTimeOffestRecoveries.CertificateIdentifier;
            }
            else if (decodedValue.Body.Content.Tests != null && decodedValue.Body.Content.Tests.Any())
            {
                maxTimeOffestTests = decodedValue.Body.Content.Tests.Last();
                var timeOffsetLocal = maxTimeOffestTests.SampleCollectionDate.AddDays(30);
                if (timeOffsetLocal > longest)
                {
                    longest = timeOffsetLocal;
                    certificateId = maxTimeOffestTests.CertificateIdentifier;
                }
            }
            else if (decodedValue.Body.Content.Vaccines != null && decodedValue.Body.Content.Vaccines.Any())
            {
                maxTimeOffestVaccinations = decodedValue.Body.Content.Vaccines.Last();
                var timeOffsetLocal = maxTimeOffestVaccinations.DateOfVaccination.AddYears(1);
                if (timeOffsetLocal > longest)
                {
                    longest = timeOffsetLocal;
                    certificateId = maxTimeOffestVaccinations.CertificateIdentifier;
                }
            }

            valModel.PhoneId = phoneId;
            valModel.CertificateId = certificateId;

            return valModel;
        }

        public static ValidationModel GetValueToCheckWithServer(GreenPassModel decodedValue, string phoneId)
        {
            ValidationModel valModel = new ValidationModel();

            DateTimeOffset longest = DateTimeOffset.MinValue;

            Models.GreenPassModels.TestEntry maxTimeOffestTests = null;
            Models.GreenPassModels.RecoveryEntry maxTimeOffestRecoveries = null;
            Models.GreenPassModels.VaccinationEntry maxTimeOffestVaccinations = null;
            string certificateId = string.Empty;

            if (decodedValue.Body.Content.Recoveries != null && decodedValue.Body.Content.Recoveries.Any())
            {
                maxTimeOffestRecoveries = decodedValue.Body.Content.Recoveries.Last();
                longest = maxTimeOffestRecoveries.ExpirationDate;
                certificateId = maxTimeOffestRecoveries.CertificateIdentifier;
            }
            else if (decodedValue.Body.Content.Tests != null && decodedValue.Body.Content.Tests.Any())
            {
                maxTimeOffestTests = decodedValue.Body.Content.Tests.Last();
                var timeOffsetLocal = maxTimeOffestTests.SampleCollectionDate.AddDays(30);
                if (timeOffsetLocal > longest)
                {
                    longest = timeOffsetLocal;
                    certificateId = maxTimeOffestTests.CertificateIdentifier;
                }
            }
            else if (decodedValue.Body.Content.Vaccines != null && decodedValue.Body.Content.Vaccines.Any())
            {
                maxTimeOffestVaccinations = decodedValue.Body.Content.Vaccines.Last();
                var timeOffsetLocal = maxTimeOffestVaccinations.DateOfVaccination.AddYears(1);
                if (timeOffsetLocal > longest)
                {
                    longest = timeOffsetLocal;
                    certificateId = maxTimeOffestVaccinations.CertificateIdentifier;
                }
            }

            valModel.PhoneId = phoneId;
            valModel.CertificateId = certificateId;

            return valModel;
        }

        public async static Task<DgcValidationResult> DecodeVerifyGreenPass(string certificate)
        {
            var dgcReader = new DgcReaderService();

            string acceptanceCountry = "IT";    // Specify the 2-letter ISO code of the acceptance country

            // Decode and validate the qr code data.
            // The result will contain all the details of the validated object
            var result = await dgcReader.GetValidationResult(certificate, acceptanceCountry);

            var status = result.Status;
            // Note: all the validation details are available in the result

            return result;
        }

        public static string DecodeINSP(string cipherText)
        {
            string key = Helpers.Constants.KeyINSP;
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public async static Task<ResultModel> GetDataToCompute(Stream pdfStream)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            Dictionary<string, string> dataToComp = new Dictionary<string, string>();
            ResultModel resultModel = new ResultModel { Hash = "", Format = "", Validator = false, Semnatar = "", Find = false };
            using (var document = PdfDocument.Load(pdfStream))
            {
                foreach (var page in document.Pages)
                {
                    string str = page.Content.ToString();
                    string user = await SecureStorage.GetAsync("User");
                    string[] words = user.Split(' ');
                    string[] number_age = words[words.Length - 1].Split('=');
                    words[words.Length - 1] = number_age[0];
                    if (str.Contains(words[0]) && str.Contains(words[1]) && str.Contains($" { words[2]} ") && str.Contains($" {words[3]} ") && str.Contains(user))
                    {
                        resultModel.Find = true;
                    }
                }

            }

            return resultModel;
        }

        public static string BytesToString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        }
    }
}
