using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace gswsBackendAPI.DL.CommonHel
{
	public static class EncryptDecryptAlgoritham
	{
		//Encryption AES
		public static string EncryptStringAES(string plainText,string key1, string iv1)
		{
			//string keystr = getHashSha256("GSWS TEST", 32);
			var keybytes = Encoding.UTF8.GetBytes(key1);
			var iv = Encoding.UTF8.GetBytes(iv1);
			

			var encryoFromJavascript = EncryptStringToBytes(plainText, keybytes, iv);
			return Convert.ToBase64String(encryoFromJavascript);
		}

		public static string DecryptStringAES(string cipherText,string key2,string iv2)
		{
			//string keystr = getHashSha256("GSWS TEST", 31);
		      var keybytes = Encoding.UTF8.GetBytes(key2);
			var iv = Encoding.UTF8.GetBytes(iv2);
			
			var encrypted = Convert.FromBase64String(cipherText);
			var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
			return decriptedFromJavascript;
		}
		private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
		{
			// Check arguments.  
			if (cipherText == null || cipherText.Length <= 0)
			{
				throw new ArgumentNullException("cipherText");
			}
			if (key == null || key.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			if (iv == null || iv.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}

			// Declare the string used to hold  
			// the decrypted text.  
			string plaintext = null;

			// Create an RijndaelManaged object  
			// with the specified key and IV.  
			using (var rijAlg = new RijndaelManaged())
			{
				//Settings  
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;

				rijAlg.Key = key;
				rijAlg.IV = iv;

				// Create a decrytor to perform the stream transform.  
				var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

				try
				{
					// Create the streams used for decryption.  
					using (var msDecrypt = new MemoryStream(cipherText))
					{
						using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))

						{

							using (var srDecrypt = new StreamReader(csDecrypt))
							{
								// Read the decrypted bytes from the decrypting stream  
								// and place them in a string.  
								plaintext = srDecrypt.ReadToEnd();

							}

						}
					}
				}
				catch
				{
					plaintext = "keyError";
				}
			}

			return plaintext;
		}
		private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
		{
			// Check arguments.  
			if (plainText == null || plainText.Length <= 0)
			{
				throw new ArgumentNullException("plainText");
			}
			if (key == null || key.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			if (iv == null || iv.Length <= 0)
			{
				throw new ArgumentNullException("key");
			}
			byte[] encrypted;
			// Create a RijndaelManaged object  
			// with the specified key and IV.  
			using (var rijAlg = new RijndaelManaged())
			{
				rijAlg.Mode = CipherMode.CBC;
				rijAlg.Padding = PaddingMode.PKCS7;
				rijAlg.FeedbackSize = 128;

				rijAlg.Key = key;
				rijAlg.IV = iv;

				// Create a decrytor to perform the stream transform.  
				var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

				// Create the streams used for encryption.  
				using (var msEncrypt = new MemoryStream())
				{
					using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (var swEncrypt = new StreamWriter(csEncrypt))
						{
							//Write all data to the stream.  
							swEncrypt.Write(plainText);
						}
						encrypted = msEncrypt.ToArray();
					}
				}
			}
			// Return the encrypted bytes from the memory stream.  
			return encrypted;
		}

		public static string getHashSha256(string text, int length)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			SHA256Managed hashstring = new SHA256Managed();
			byte[] hash = hashstring.ComputeHash(bytes);
			string hashString = string.Empty;
			foreach (byte x in hash)
			{
				hashString += String.Format("{0:x2}", x); //covert to hex string
			}
			if (length > hashString.Length)
				return hashString;
			else
				return hashString.Substring(0, length);
		}
		static readonly char[] CharacterMatrixForRandomIVStringGeneration = {
			'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
			'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
			'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
			'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
		};
		internal static string GenerateRandomIV(int length)
		{
			char[] _iv = new char[length];
			byte[] randomBytes = new byte[length];

			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(randomBytes); //Fills an array of bytes with a cryptographically strong sequence of random values. 
			}

			for (int i = 0; i < _iv.Length; i++)
			{
				int ptr = randomBytes[i] % CharacterMatrixForRandomIVStringGeneration.Length;
				_iv[i] = CharacterMatrixForRandomIVStringGeneration[ptr];
			}

			return new string(_iv);
		}
	}
}