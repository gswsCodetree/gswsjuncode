using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace gswsBackendAPI.DL.CommonHel
{
    public class Utils
    {

        public static bool IsAlphaNumeric(String strToCheck)
        {
            if (!string.IsNullOrEmpty(strToCheck))
            {
                Regex objAlphaPattern = new Regex("[^a-zA-Z 0-9/ -]");
                return !objAlphaPattern.IsMatch(strToCheck);
            }
            return true;
        }
        public static bool IsNumeric(String strToCheck)
        {
            if (!string.IsNullOrEmpty(strToCheck))
            {
                Regex objAlphaPattern = new Regex("[^0-9]");
                return !objAlphaPattern.IsMatch(strToCheck);
            }
            return true;
        }

		public static bool IsValidImage(byte[] bytes)
		{
			try
			{
				if (bytes.Length > 0)
				{
					using (MemoryStream ms = new MemoryStream(bytes))
						Image.FromStream(ms);
				}
			}
			catch (ArgumentException)
			{
				return false;
			}
			return true;
		}

		public static bool IsValidPDF(byte[] bytes)
		{
			try
			{
				if (bytes.Length > 0)
				{
					using (MemoryStream ms = new MemoryStream(bytes))
					{
						byte[] buffer = new byte[4];
						ms.Read(buffer, 0, 4);

						string data_as_hex = BitConverter.ToString(buffer);

						if (data_as_hex == "25-50-44-46")
							return true;
						else
							return false;
					}
				}
				else
					return true;
			}
			catch (ArgumentException)
			{
				return false;
			}

		}

		public static bool IsPdf(HttpPostedFileBase DocumentUpload)
        {
            var pdfString = "%PDF-";
            var pdfBytes = Encoding.ASCII.GetBytes(pdfString);
            var len = pdfBytes.Length;

            int FileLen;
            System.IO.Stream MyStream;

            FileLen = DocumentUpload.ContentLength;
            byte[] buffer = new byte[len];

            // Initialize the stream.
            MyStream = DocumentUpload.InputStream;

            // Read the file into the byte array.
            if (FileLen >= len)
                MyStream.Read(buffer, 0, len);
            else
                MyStream.Read(buffer, 0, FileLen);



            return pdfBytes.SequenceEqual(buffer);
        }
    }
}