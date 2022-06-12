using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    // Інформація про файл, який відправляється
    [Serializable] // атрибут для серіалізації об'єкта класа
    public class FileTransferInfo
    {
        // ім'я файла
        public string Name { get; set; }
        // вміст файла
        public byte[] Data { get; set; }
    }
}
