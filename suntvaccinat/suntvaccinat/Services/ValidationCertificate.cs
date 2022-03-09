using DgcReader;
using DgcReader.Models;
using GemBox.Pdf;
using GemBox.Pdf.Forms;
using suntvaccinat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

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

        public async static Task<ResultModel> GetDataToCompute(Stream pdfStream)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            Dictionary<string, string> dataToComp = new Dictionary<string, string>();
            ResultModel resultModel = new ResultModel { Hash = "", Format = "", Validator = false, Semnatar = "", Find = false };
            using (var document = PdfDocument.Load(pdfStream))
            {
                foreach (var field in document.Form.Fields)
                {
                    if (field.FieldType == PdfFieldType.Signature)
                    {
                        var signatureField = (PdfSignatureField)field;
                        var a = signatureField.Actions;
                        var a1 = signatureField.Metadata;
                        var a2 = signatureField.Page;
                        var a3 = signatureField.Name;
                        var a4 = signatureField.Value;



                        var signature = signatureField.Value;
                        string rowDataContent = BytesToString(signatureField.Value.Content.SignerCertificate.GetRawData());
                        int i = 0;
                        if (signature != null)
                        {
                            var signatureValidationResult = signature.Validate();

                            if (signatureValidationResult.IsValid && entities.Contains(signatureField.Value.Name))
                            {
                                byte[] ba = signatureField.Value.ComputeHash(GemBox.Pdf.Security.PdfHashAlgorithm.SHA256);

                                resultModel.Hash = BytesToString(ba);
                                resultModel.Format = signatureField.Value.Format.ToString();
                                resultModel.Semnatar = signatureField.Value.Name;
                                resultModel.RawData = BytesToString(signature.Content.GetRawData());
                                resultModel.Validator = true;

                                Console.Write("Signature '{0}' is VALID, signed by '{1}'. ", signatureField.Name, signature.Content.SignerCertificate.SubjectCommonName);
                                Console.WriteLine("The document has not been modified since this signature was applied.");
                            }

                        }
                    }
                }

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

                var signatureFields = document.Form.Fields.
                                Where(f => f.FieldType == PdfFieldType.Signature).
                                Cast<PdfSignatureField>().
                                ToList();

                // Either remove the signature or the signature field.
                for (int i = 0; i < signatureFields.Count; ++i)
                    if (i % 2 == 0)
                        signatureFields[i].Value = null;
                    else
                        document.Form.Fields.Remove(signatureFields[i]);

                //using (Services.SHA1CryptoServiceProvider sha1 = new Services.SHA1CryptoServiceProvider())
                //{
                //    byte[] sourceBytes = Encoding.UTF8.GetBytes(document2.ToString());
                //    byte[] hashBytes = sha1.ComputeHash(sourceBytes);
                //    string hash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                //    int i = 0;
                //}
                // / data / user / 0 / com.companyname.suntvaccinat.ro / files / Remove_Digital_Signature.pdf
                // string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"Remove_Digital_Signature.pdf");
                //  document.Save("/storage/emulated/0/Android/data/com.companyname.suntvaccinat.ro/cache/2203693cc04e0be7f4f024d5f9499e13/eb40ab5a9d7145059268881cb6e8e096/Remove_Digital_Signature.pdf");

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
