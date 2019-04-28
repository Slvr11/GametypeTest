using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IW5M_GameType_Changer
{
    public class GametypeChanger : BaseScript
    {
        public GametypeChanger()
        {
            doRotationPatch();
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, uint flAllocationType, uint flProtect);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualFree(IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);
        public IntPtr alloc(int size)
        {
            return VirtualAlloc(IntPtr.Zero, (UIntPtr)size, 0x3000, 0x40);
        }
        public bool unalloc(IntPtr address, int size)
        {
            return VirtualFree(address, (UIntPtr)size, 0x8000);
        }

        private void doRotationPatch()
        {
            // patch the rotation size
            byte[] patchBytes = BitConverter.GetBytes(1152);
            Marshal.Copy(patchBytes, 0, new IntPtr(0x5C6083), 4);

            IntPtr rotationBuffer = VirtualAlloc(IntPtr.Zero, (UIntPtr)1152, 0x3000, 0x04);
            Marshal.Copy(BitConverter.GetBytes(rotationBuffer.ToInt32()), 0, new IntPtr(0x5C35EE), 4);
            Marshal.Copy(BitConverter.GetBytes(rotationBuffer.ToInt32()), 0, new IntPtr(0x5C35F8), 4);
            Marshal.Copy(BitConverter.GetBytes(rotationBuffer.ToInt32()), 0, new IntPtr(0x5C37D0), 4);
            Marshal.Copy(BitConverter.GetBytes(rotationBuffer.ToInt32()), 0, new IntPtr(0x5C37DA), 4);
            Marshal.Copy(BitConverter.GetBytes(rotationBuffer.ToInt32()), 0, new IntPtr(0x6480E3C), 4);
            // should now reload the new larger dspl on map_rotate
        }
    }
}