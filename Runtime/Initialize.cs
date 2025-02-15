﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using IL_Emulator_Dynamic;
using VMExample.Instructions;

namespace NullVM
{
    public class MethodReady
    {
        public static OpCode[] oneByteOpCodes;
        public static OpCode[] twoByteOpCodes;
        public static StackTrace stackTrace;
        public static System.Reflection.Module callingModule;

        public static byte[] byteArrayResource;
        public static a bc;


        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress", ExactSpelling = true)]
        private static extern IntPtr e(IntPtr intptr, string str);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, EntryPoint = "GetModuleHandle")]
        private static extern IntPtr ab(string str);
        public delegate void a(byte[] bytes, int len, byte[] key, int keylen);
        public static void Processor(string resName)
        {

            var result = new StringBuilder();

            for (int c = 0; c < resName.Length; c++)
                result.Append((char)((uint)resName[c] ^ (uint)"0x58B_"[c % "0x58B_".Length]));

            resName = result.ToString();


            callingModule = new StackTrace().GetFrame(1).GetMethod().Module;

            byteArrayResource = extractResource(resName);
            byte[] tester = extractResource("%/?#");
            VMExample.Instructions.All.binr = new BinaryReader(new MemoryStream(tester));
            VMExample.Instructions.All.val = new ValueStack();
            VMExample.Instructions.All.val.parameters = new object[1];
            All.val.parameters[0] = byteArrayResource;

            All.val.locals = new object[10];
            VMExample.Instructions.All.run();
            IntPtr abb;
            IntPtr def;
            if (IntPtr.Size == 4)
            {
                byte[] tester2 = extractResource("+/&=");
                EmbeddedDllClass.ExtractEmbeddedDlls("0x58B.dll", tester2);
                abb = EmbeddedDllClass.LoadDll("0x58B.dll");
                def = e(abb, "_a@16");
            }
            else
            {
                byte[] tester2 = extractResource("%&=?+");
                EmbeddedDllClass.ExtractEmbeddedDlls("0x58B.dll", tester2);
                abb = EmbeddedDllClass.LoadDll("0x58B.dll");
                def = e(abb, "a");
            }



            //a(x,x,x,x) 0000000010001070 1

            bc = (a)Marshal.GetDelegateForFunctionPointer(def, typeof(a));
            byteArrayResource = (byte[])All.val.locals[1];
            //process all opcodes into fields so that they relate to the way i process them in the conversion to method
            var array = new OpCode[256];
            var array2 = new OpCode[256];
            oneByteOpCodes = array;
            twoByteOpCodes = array2;
            var typeFromHandle = typeof(OpCode);
            var typeFromHandle2 = typeof(OpCodes);
            foreach (var fieldInfo in typeFromHandle2.GetFields())
                if (fieldInfo.FieldType == typeFromHandle)
                {
                    var opCode = (OpCode)fieldInfo.GetValue(null);
                    var num = (ushort)opCode.Value;
                    if (opCode.Size == 1)
                    {
                        var b = (byte)num;
                        oneByteOpCodes[b] = opCode;
                    }
                    else
                    {
                        var b2 = (byte)(num | 65024);
                        twoByteOpCodes[b2] = opCode;
                    }
                }
        }


        private static byte[] extractResource(string resourceName)
        {
            using (Stream stream = callingModule.Assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                byte[] array = new byte[stream.Length];
                stream.Read(array, 0, array.Length);
                return array;

            }
        }
    }
}
