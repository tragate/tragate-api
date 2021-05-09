
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace sd
{
    public class Fonksiyonlar
    {
        public static IConfiguration Ayarlar;
        public static string root;

        public static bool MailGonder(string gonderenisim, string gonderenmail, string aliciisim, string alicimail, string baslik, string mesaj, ref string hata_mesaji)
        {
            bool functionReturnValue = false;
            try
            {
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(gonderenmail, gonderenisim),
                    IsBodyHtml = true,
                    Subject = baslik,
                    BodyEncoding = Encoding.UTF8,
                    Body = mesaj
                };
                mail.To.Add(new MailAddress(alicimail, aliciisim));

                SmtpClient sc = new SmtpClient
                {
                    Host = "smtp.mandrillapp.com",
                    Port = 587,
                    Credentials = new NetworkCredential("xxxx", "xxxx"),
                    EnableSsl = true
                };
                sc.SendAsync(mail, null);
                functionReturnValue = true;
            }
            catch (Exception ex)
            {
                functionReturnValue = false;
                hata_mesaji = ex.Message;
            }
            return functionReturnValue;
        }

        public static async Task<bool> MailGonderAsync(string gonderenisim, string gonderenmail, string aliciisim, string alicimail, string baslik, string mesaj)
        {
            bool functionReturnValue = false;
            try
            {
                await Task.Run(() =>
                {

                    MailMessage mail = new MailMessage
                    {
                    From = new MailAddress(gonderenmail, gonderenisim),
                    IsBodyHtml = true,
                    Subject = baslik,
                    BodyEncoding = Encoding.UTF8,
                    Body = mesaj
                    };
                    mail.To.Add(new MailAddress(alicimail, aliciisim));

                    SmtpClient sc = new SmtpClient
                    {
                    Host = "smtp.mandrillapp.com",
                    Port = 587,
                    Credentials = new NetworkCredential("xxxx", "xxxxx"),
                    EnableSsl = true
                    };
                    sc.SendAsync(mail, null);
                    functionReturnValue = true;

                });
            }
            catch (Exception ex)
            {
                functionReturnValue = false;
            }
            return functionReturnValue;
        }


        //public static bool MailGonder(string gonderenisim, string gonderenmail, string aliciisim, string alicimail, string baslik, string mesaj, ref string hata_mesaji)
        //{
        //    bool functionReturnValue = false;

        //    try
        //    {
        //        var message = new MimeMessage();
        //        message.From.Add(new MailboxAddress(gonderenmail, gonderenisim));
        //        message.To.Add(new MailboxAddress(aliciisim, alicimail));
        //        message.Subject = baslik;

        //        var bodyBuilder = new BodyBuilder();
        //        bodyBuilder.HtmlBody = mesaj;
        //        message.Body = bodyBuilder.ToMessageBody();

        //        using (var client = new SmtpClient())
        //        {
        //            // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
        //            client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        //            client.Connect("smtp.yandex.com", 465, true);

        //            // Note: since we don't have an OAuth2 token, disable
        //            // the XOAUTH2 authentication mechanism.
        //            client.AuthenticationMechanisms.Remove("XOAUTH2");

        //            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //            client.UseDefaultCredentials = false;


        //            // Note: only needed if the SMTP server requires authentication
        //            client.Authenticate("d70555c89e7840ce8de434962014369d", "vgrgicglslnhexps");

        //            client.Send(message);
        //            client.Disconnect(true);
        //        }

        //        functionReturnValue = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        functionReturnValue = false;
        //        hata_mesaji = ex.Message;
        //    }
        //    return functionReturnValue;
        //}






        //public static string getir(string action, NameValueCollection ek_header = null)
        //{
        //    HttpWebRequest req = HttpWebRequest.Create(action);
        //    if ((ek_header != null))
        //    {
        //        req.Headers.Add(ek_header);
        //    }

        //    //req.Headers.Add("Accept-Language", "")
        //    HttpWebResponse resp = req.GetResponse();
        //    StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("ISO-8859-9"));
        //    string results = sr.ReadToEnd();
        //    sr.Close();
        //    return results;
        //}

        //public static string mevcut_sayfa()
        //{
        //    string[] sayfalar = HttpContext.Current.Request.ServerVariables["URL"].Split('/');
        //    return sayfalar[sayfalar.Length - 1];
        //}






        private static System.Random objRandom = new System.Random(Convert.ToInt32(System.DateTime.Now.Ticks % System.Int32.MaxValue));
        public static int rastgelesayi(int enaz = 1, int encok = 100)
        {
            int sonuc = objRandom.Next(enaz, encok + 1);
            return sonuc;
        }

        public static bool DosyaKontrol(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            List<string> izinliler = new List<string>();
            izinliler.Add(".png");
            izinliler.Add(".jpg");
            izinliler.Add(".jpeg");
            izinliler.Add(".pdf");
            izinliler.Add(".docx");
            izinliler.Add(".doc");
            izinliler.Add(".xlsx");
            izinliler.Add(".xls");

            if (izinliler.Contains(ext))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public static string RandomString(int length)
        {
            const string chars = "ACDEFGHJKLMNPRSTUVWXYZ123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string DosyaAdiTemizle(string gelen)
        {
            gelen = gelen.ToLower();
            gelen = gelen.Replace("ş", "s");
            gelen = gelen.Replace("ı", "i");
            gelen = gelen.Replace("ğ", "g");
            gelen = gelen.Replace("ü", "ü");
            gelen = gelen.Replace("ö", "o");
            gelen = gelen.Replace("ç", "c");
            gelen = gelen.Replace("İ", "i");

            Regex rgx = new Regex("[^a-zA-Z0-9 -.]");
            return rgx.Replace(gelen, "");
        }

        public static string UrlAyarla(string gelen)
        {
            string sonuc;
            gelen = gelen.Replace(".aspx", "");
            gelen = gelen.Replace("Â", "a");
            gelen = gelen.Replace("â", "a");
            gelen = gelen.Replace("ü", "u");
            gelen = gelen.Replace("Ü", "u");
            gelen = gelen.Replace("Ö", "o");
            gelen = gelen.Replace("ö", "o");
            gelen = gelen.Replace("ğ", "g");
            gelen = gelen.Replace("Ğ", "g");
            gelen = gelen.Replace("Ç", "c");
            gelen = gelen.Replace("ç", "c");
            gelen = gelen.Replace("ş", "s");
            gelen = gelen.Replace("Ş", "s");
            gelen = gelen.Replace("İ", "i");
            gelen = gelen.Replace("ı", "i");
            gelen = gelen.Replace("I", "i");
            gelen = Regex.Replace(gelen, "[^a-zA-Z0-9 _]+", "");
            gelen = gelen.Replace(" ", "-");
            gelen = gelen.Replace("_", "-");
            gelen = gelen.ToLower();
            sonuc = gelen;
            return sonuc;
        }





        //public static string ayar_oku(string deger)
        //{
        //    string sonuc = "";
        //    try
        //    {

        //        var builder = new ConfigurationBuilder()
        //        .SetBasePath(Path.GetFullPath(@"./")).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        //        IConfigurationRoot xx = builder.Build();

        //        sonuc = xx.GetConnectionString(deger);
        //        if (string.IsNullOrEmpty(sonuc))
        //        {
        //            sonuc = "Değer Eksik";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("appsettings.json dosyadından ayar okunamadı : " + ex.Message);
        //    }

        //    return sonuc;
        //}

        public static string DosyaOku(string FullPath)
        {
            string sonuc = "";
            StreamReader objReader = default(StreamReader);
            try
            {
                objReader = new StreamReader(FullPath);
                sonuc = objReader.ReadToEnd();
                objReader.Close();
            }
            catch (Exception Ex)
            {
                sonuc = "HATA" + Ex.Message;
            }
            return sonuc;
        }
        public static string DosyaKaydet(string strData, string FullPath)
        {
            string sonuc = "";
            StreamWriter objReader = default(StreamWriter);
            try
            {
                objReader = new StreamWriter(FullPath, false, System.Text.Encoding.Unicode);
                objReader.Write(strData);
                objReader.Close();
                sonuc = "ok";
            }
            catch (Exception Ex)
            {
                sonuc = "HATA" + Ex.Message;
            }

            return sonuc;
        }





        //public static string proxy()
        //{
        //    string sonuc = "";
        //    try
        //    {
        //        sonuc = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
        //        if (sonuc == null)
        //        {
        //            sonuc = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        sonuc = "";
        //    }
        //    if (string.IsNullOrEmpty(sonuc) | sonuc.ToLower().ToString() == "unknown")
        //    {
        //        sonuc = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        //    }
        //    return sonuc;
        //}



        public static string ConvertUrlsToLinks(string msg)
        {
            string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
            Regex r = new Regex(regex, RegexOptions.IgnoreCase);
            return r.Replace(msg, "<a href=\"$1\" title=\"$1\" target=\"&#95;blank\">$1</a>").Replace("href=\"www", "href=\"http://www");
        }

        public static string Serialize<T>(T obj)
        {
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.UTF8.GetString(ms.ToArray());
            ms.Dispose();
            return retVal;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            ms.Dispose();
            return obj;
        }

    }




    public static class Rfc4180Writer
    {
        public static void WriteDataTable(DataTable sourceTable, TextWriter writer, bool includeHeaders)
        {
            if (includeHeaders)
            {
                IEnumerable<String> headerValues = sourceTable.Columns
                    .OfType<DataColumn>()
                    .Select(column => QuoteValue(column.ColumnName));

                writer.WriteLine(String.Join(",", headerValues));
            }

            IEnumerable<String> items = null;

            foreach (DataRow row in sourceTable.Rows)
            {
                items = row.ItemArray.Select(o => QuoteValue(o?.ToString() ?? String.Empty));
                writer.WriteLine(String.Join(",", items));
            }

            writer.Flush();
        }

        private static string QuoteValue(string value)
        {



            var _val = value;

            //Check if the value contans a comma and place it in quotes if so
            if (_val.Contains(","))
            {
                _val = string.Concat("\"", _val, "\"");
            }

            //Replace any \r or \n special characters from a new line with a space
            if (_val.Contains("\r"))
            {
                _val = _val.Replace("\r", " ");
            }

            if (_val.Contains("\n"))
            {
                _val = _val.Replace("\n", " ");
            }

            return String.Concat("\"",
          _val.Replace("\"", "\"\""), "\"");
        }
    }
}

