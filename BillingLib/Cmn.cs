using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Summary description for Cmn
/// </summary>
/// 
public class myAsyncResult : IAsyncResult
{

    object IAsyncResult.AsyncState
    {
        get { throw new NotImplementedException(); }
    }

    System.Threading.WaitHandle IAsyncResult.AsyncWaitHandle
    {
        get { throw new NotImplementedException(); }
    }

    bool IAsyncResult.CompletedSynchronously
    {
        get { return true; }
    }

    bool IAsyncResult.IsCompleted
    {
        get { return false; }
    }
}

public class Cmn
{

    const Boolean UseCache = true;

    public Cmn()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static DateTime MinDate = new DateTime(1900, 1, 1);

    public static DateTime FinStartDate
    {
        get
        {
            return new DateTime(DateTime.Today.AddYears(-1).Month <= 3 ? DateTime.Today.AddYears(-1).Year : DateTime.Today.Year, 4, 1);
        }
    }

    public static DateTime FinEndDate
    {
        get
        {
            return new DateTime((DateTime.Today.AddYears(-1).Month > 3 ? DateTime.Today.AddYears(1).Year : DateTime.Today.Year), 3, 31);
        }
    }

    public static DateTime MonthFirst(DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, 1);
    }

    public static DateTime MonthLast(DateTime dt)
    {
        return MonthFirst(dt).AddMonths(1).AddDays(-1);
    }

    public static DateTime ThisMonthFirst
    {
        get
        {
            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        }
    }

    public static DateTime ThisMonthLast
    {
        get
        {
            return ThisMonthFirst.AddMonths(1).AddDays(-1);
        }
    }

    public static void WriteFile(string str, string FileName, string CompressionType)
    {
        byte[] buffer = System.Text.Encoding.ASCII.GetBytes(str);

        switch (CompressionType)
        {
            case "gzip":
                {
                    FileStream sw = new FileStream(FileName, FileMode.Create);
                    GZipStream gz = new GZipStream(sw, CompressionMode.Compress);
                    gz.Write(buffer, 0, buffer.Length);
                    gz.Close();
                    sw.Close();
                }
                break;
            case "deflate":
                {
                    FileStream sw = new FileStream(FileName, FileMode.Create);
                    DeflateStream dz = new DeflateStream(sw, CompressionMode.Compress);
                    dz.Write(buffer, 0, buffer.Length);
                    dz.Close();
                    sw.Close();
                }
                break;
            default:
                {
                    StreamWriter sw = new StreamWriter(FileName, false);
                    sw.Write(str);
                    sw.Close();
                }
                break;
        }

        File.SetCreationTime(FileName, DateTime.Now);
    }

    public static string GetUnCompressed(byte[] Data, int Size)             //Uncompreess Data
    {
        if (Data == null)
            return string.Empty;
        MemoryStream ms = new MemoryStream(Data);
        GZipStream gz = null;
        try
        {
            gz = new GZipStream(ms, CompressionMode.Decompress);
            byte[] decompressedBuffer = new byte[Size];
            int DataLength = gz.Read(decompressedBuffer, 0, Size);
            using (MemoryStream msDec = new MemoryStream())
            {
                msDec.Write(decompressedBuffer, 0, DataLength);
                msDec.Position = 0;
                string s = new StreamReader(msDec).ReadToEnd();
                return s;
            }
        }
        catch
        {
            //return ex.Message;
        }
        finally
        {
            ms.Close();
            gz.Close();
        }
        return string.Empty;
    }
    public static void DeleteExistingFileByPath(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }


    public static void FileCheck(string path)
    {
        if (File.Exists(path + ".jpg"))
            File.Delete(path + ".jpg");
        else if (File.Exists(path + ".jpeg"))
            File.Delete(path + ".jpeg");
        else if (File.Exists(path + ".pdf"))
            File.Delete(path + ".pdf");
        else if (File.Exists(path + ".bmp"))
            File.Delete(path + ".bmp");
        else if (File.Exists(path + ".gif"))
            File.Delete(path + ".gif");
    }

    public static string CheckString(string text)
    {
        return text.Replace("'", "").Replace("\"", "").Replace("@", "").Replace("?", "").Replace("*", "");
    }



    public static string ValidateInput(string Data, int Length, Boolean CheckforValidDate, Boolean ConvertToUpper, Boolean CleanInput)
    {
        if (Length > 0)
            if (Data.Length > Length)
                Data = Data.Substring(0, Length);

        if (ConvertToUpper)
            Data = Data.ToUpper();

        if (CleanInput)
            Data = Data.Replace("'", "").Replace("%", "").Replace("*", "").Replace(" ", "");

        return Data;
    }

    public static DateTime GetIndiaTime()
    {
        return DateTime.Now.ToUniversalTime().AddHours(5).AddMinutes(30);
    }


    public static DateTime ToDate(string txt)
    {
        DateTime X;
        if (DateTime.TryParse(txt, out X) == false)
            return Cmn.MinDate;
        return X;
    }

    public static DateTime ToDate(object obj)
    {
        if (obj == null)
            return Cmn.MinDate;

        DateTime X;
        if (DateTime.TryParse(obj.ToString(), out X) == false)
            return Cmn.MinDate;

        return X;
    }



    public static decimal ToDec(string txt)
    {
        decimal X;
        if (decimal.TryParse(txt, out X) == false)
            return 0;

        return X;
    }

    public static double ToDbl(object txt)
    {
        double X;
        if (double.TryParse(txt.ToString(), out X) == false)
            return 0;

        return X;
    }


    public static int ToInt(string txt, int DefaultValue)
    {
        int X;
        if (int.TryParse(txt, out X) == false)
            return DefaultValue;
        return X;
    }

    public static int ToInt(object txt)
    {
        if (txt == null)
            return 0;
        int X;
        if (int.TryParse(txt.ToString(), out X) == false)
            return 0;

        return X;
    }

    public static int ToInt(string txt)
    {
        int X;
        if (int.TryParse(txt, out X) == false)
            return 0;

        return X;
    }

    public static string ProperCase(string str)
    {
        if (string.IsNullOrEmpty(str))
            return "";
        CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        TextInfo textInfo = cultureInfo.TextInfo;
        return textInfo.ToTitleCase(str.Trim().ToLower());
    }




    public static string ConvertNumberToWord(Int32 numberVal)
    {
        string[] powers = new string[] { "Thousand ", "Million ", "Billion " };
        string[] ones = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        string wordValue = "";
        if (numberVal == 0) return "Zero";
        if (numberVal < 0)
        {
            wordValue = "Negative ";
            numberVal = -numberVal;
        }
        long[] partStack = new long[] { 0, 0, 0, 0 };
        int partNdx = 0;
        while (numberVal > 0)
        {
            partStack[partNdx++] = numberVal % 1000;
            numberVal /= 1000;
        }
        for (int i = 3; i >= 0; i--)
        {
            long part = partStack[i];
            if (part >= 100)
            {
                wordValue += ones[part / 100 - 1] + " Hundred ";
                part %= 100;
            }
            if (part >= 20)
            {
                if ((part % 10) != 0) wordValue += tens[part / 10 - 2] + " " + ones[part % 10 - 1] + " ";
                else wordValue += tens[part / 10 - 2] + " ";
            }
            else if (part > 0) wordValue += ones[part - 1] + " ";
            if (part != 0 && i > 0) wordValue += powers[i - 1];
        }
        return wordValue;
    }

    public static double CalcDistance(double lat1, double lon1, double lat2, double lon2, char unit = 'K')    //calculate distance betwwen two loc in googlemap
    {
        var theta = lon1 - lon2;
        var dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
        dist = Math.Acos(dist);
        dist = rad2deg(dist);
        dist = dist * 60 * 1.1515;
        if (unit == 'K')
        {
            dist = dist * 1.609344;
        }
        else if (unit == 'N')
        {
            dist = dist * 0.8684;
        }
        return (dist);
    }

    private static double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    private static double rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }



    public static string GenerateSlug(string phrase)     //returns-clear-urlname
    {
        if (string.IsNullOrWhiteSpace(phrase)) phrase = string.Empty;
        string str = RemoveAccent(phrase).Replace("&", "and").ToLower();
        // invalid chars           
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim();
        // convert multiple - into one space   
        str = Regex.Replace(str, @"\-+", " ").Trim();
        // cut and trim 
        //str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-"); //replace space with hyphens   
        return str;
    }

    public static string RemoveAccent(string txt)
    {
        byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        return System.Text.Encoding.ASCII.GetString(bytes);
    }




    public static double CalculateTax(double TaxableAmount)
    {
        double f1 = 250000.0;
        double f2 = 500000.0;
        double f3 = 1000000.0;

        if (TaxableAmount <= f1)
            return 0;

        double tax = 0;

        if (TaxableAmount <= f2)
            tax = (TaxableAmount - f1) * .1;
        else if (TaxableAmount <= f3)
            tax = (TaxableAmount - f2) * .2 + 25000;
        else
            tax = (TaxableAmount - f3) * .3 + 125000;
        return Math.Round(tax);
    }

    public static double CalculateSurcharge(double tax)
    {
        double surcharge = 0;
        if (tax > 10000000)
            surcharge = tax * 0.01;
        return Math.Round(surcharge);
    }

    public static double CalculateCess(double tax)
    {
        return tax * 0.03;
    }

    public static string CreateRandomPassword(int pasLenth)
    {
        string allowedChars = "";
        allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
        allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
        allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";
        char[] sep = { ',' };
        string[] arr = allowedChars.Split(sep);
        string passwordString = "";
        string temp = "";
        Random rand = new Random();
        for (int i = 0; i < pasLenth; i++)
        {
            temp = arr[rand.Next(0, arr.Length)];
            passwordString += temp;
        }
        return passwordString;
    }

    public static string SendEmail(string Subject, string EmailTo, string Name, string mailBody)
    {
        SmtpClient client = new SmtpClient();
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = true;
        client.Host = "smtp.gmail.com";
        client.Port = 587;

        // setup Smtp authentication
        NetworkCredential credentials = new NetworkCredential("rraprop@gmail.com", "rra12345");

        client.UseDefaultCredentials = false;
        client.Credentials = credentials;

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress("rraprop@gmail.com");
        msg.To.Add(new MailAddress(EmailTo));
        msg.CC.Add(new MailAddress("rraprop@gmail.com"));
        //msg.Bcc.Add(new MailAddress("rraprop@gmail.com"));
        msg.Subject = Subject;
        msg.IsBodyHtml = true;
        msg.Body = string.Format(mailBody);
        try
        {
            client.Send(msg);
            return "Mail Send";
        }
        catch (Exception ex)
        {
            return "Error occured while sending your message." + ex.Message;
        }
    }

    public static Boolean DownloadFile(string SaveURL, string DownloadURL, Boolean isSizeCheck = false, string CustomMessage = "")
    {
        using (var client = new WebClient())
        {
            try
            {
                Boolean isDownload = true;
                if (isSizeCheck)
                {
                    isDownload = false;
                    if (CheckHTTPFileLength(DownloadURL) > 24731)
                        isDownload = true;
                }
                if (isDownload)
                    client.DownloadFile(DownloadURL, SaveURL);
                return true;

            }
            catch
            {
                //Cmn.LogError(ex, "File Download Error Check " + CustomMessage + " URL:" + DownloadURL, (companyID == 0 ? "FileDownload" : companyID.ToString()));
                return false;
            }
        }
    }

    public static long CheckHTTPFileLength(string fileName)
    {
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(fileName);
        req.Method = "HEAD";
        HttpWebResponse resp = (HttpWebResponse)(req.GetResponse());

        return resp.ContentLength;
    }


    public static string RandomString()
    {
        int length = 15;

        const int byteSize = 0x100;
        var allowedCharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

        // Guid.NewGuid and System.Random are not particularly random. By using a
        // cryptographically-secure random number generator, the caller is always
        // protected, regardless of use.
        using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
        {
            var result = new StringBuilder();
            var buf = new byte[128];
            while (result.Length < length)
            {
                rng.GetBytes(buf);
                for (var i = 0; i < buf.Length && result.Length < length; ++i)
                {
                    // Divide the byte into allowedCharSet-sized groups. If the
                    // random value falls into the last group and the last group is
                    // too small to choose from the entire allowedCharSet, ignore
                    // the value in order to avoid biasing the result.
                    var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                    if (outOfRangeStart <= buf[i]) continue;
                    result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                }
            }
            return result.ToString();
        }
    }

    public static string toCurrency(double Amount)
    {
        System.Globalization.CultureInfo info = System.Globalization.CultureInfo.GetCultureInfo("hi-IN");
        //return Amount.ToString("C2", info); //with Rs sign
        return string.Format(info, "{0:#,#}", Amount); //without rs sign
    }

    public static DateTime ConverToFirstDate(DateTime dt)
    {
        return new DateTime(dt.Year,dt.Month,1);
    }

    public static int ToFinancialYear(DateTime dateTime)
    {
        return (dateTime.Month >= 4 ? dateTime.Year + 1 : dateTime.Year);
    }

   
}
