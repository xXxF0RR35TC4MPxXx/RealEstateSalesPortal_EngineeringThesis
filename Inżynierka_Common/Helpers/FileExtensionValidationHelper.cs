using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Inżynierka_Common.Helpers
{
    public class FileExtensionValidationHelper
    {

        private static readonly Dictionary<string, List<byte[]>> _photoSignature = new Dictionary<string, List<byte[]>>
        {
            { ".png", new List<byte[]>
                { 
                    new byte[] { 0x89, 0x50, 0x4E, 0x47 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xD8 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }
                } 
            },
            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                }
            },
            { ".tiff", new List<byte[]>
                {
                    new byte[] { 0x49, 0x49, 0xA2, 0x00 },
                    new byte[] { 0x4D, 0x4D, 0x2A, 0x00 }
                }
            },
            { ".webp", new List<byte[]>
                {
                    new byte[] { 0x52, 0x49, 0x46, 0x46 },
                }
            },
        };

        public static bool ValidateIfCorrectPhotoFormat(byte[] headerBytes)
        {
            
                var JPGsignatures = _photoSignature[".jpg"];
                var PNGsignatures = _photoSignature[".png"];
                var TIFFsignatures = _photoSignature[".tiff"];
                var WEBPsignatures = _photoSignature[".webp"];

                return (
                    PNGsignatures.Any(signature => headerBytes.SequenceEqual(signature)) ||
                    TIFFsignatures.Any(signature => headerBytes.SequenceEqual(signature)) ||
                    WEBPsignatures.Any(signature => headerBytes.SequenceEqual(signature)) || 
                    JPGsignatures.Any(signature => headerBytes.SequenceEqual(signature))
                );
        }

        public static bool ValidateFileSize(MemoryStream file, int size)
        {
            var result = file.Length < size;
            return result;
        }
    }
}
